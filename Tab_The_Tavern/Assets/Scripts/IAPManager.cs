using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }

    // ===== Product IDs =====
    private const string PRODUCT_NO_ADS = "no_ads";

    private const string NO_ADS_KEY = "NO_ADS_PURCHASED";

    private StoreController store;
    private bool initialized;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

#if !UNITY_WEBGL

    private async void Start()
    {
        await InitializeIAP();
    }

#endif

    private async Task InitializeIAP()
    {
        store = UnityIAPServices.StoreController();

        store.OnPurchasePending += OnPurchasePending;
        store.OnProductsFetched += OnProductsFetched;
        store.OnPurchasesFetched += OnPurchasesFetched;

        store.OnPurchaseFailed += failed =>
            Debug.LogWarning($"[IAP] Purchase failed: {failed}");

        store.OnProductsFetchFailed += failed =>
            Debug.LogWarning($"[IAP] Products fetch failed: {failed}");

        store.OnPurchasesFetchFailed += failed =>
            Debug.LogWarning($"[IAP] Purchases fetch failed: {failed}");

        await store.Connect();

        var products = new List<ProductDefinition>
        {
            new ProductDefinition(PRODUCT_NO_ADS, ProductType.NonConsumable)
        };

        store.FetchProducts(products);
    }

    private void OnProductsFetched(List<Product> products)
    {
        initialized = true;
        store.FetchPurchases(); 
    }

    private void OnPurchasesFetched(Orders orders)
    {
        foreach (var confirmed in orders.ConfirmedOrders)
        {
            if (OrderContainsProduct(confirmed, PRODUCT_NO_ADS))
            {
                NoAdsManager.Instance.SetNoAdsPurchased();
                return;
            }
        }
    }

    private void OnPurchasePending(PendingOrder pending)
    {
        if (OrderContainsProduct(pending, PRODUCT_NO_ADS))
        {
            NoAdsManager.Instance.SetNoAdsPurchased();
        }

        store.ConfirmPurchase(pending);
    }

    public void BuyNoAds()
    {
        if (!initialized)
        {
            Debug.LogWarning("[IAP] Not initialized yet");
            return;
        }

        if (NoAdsManager.Instance.HasNoAds)
        {
            Debug.Log("[IAP] No Ads already owned");
            return;
        }

        store.PurchaseProduct(PRODUCT_NO_ADS);
    }

    private bool OrderContainsProduct(Order order, string productId)
    {
        var list = order.Info?.PurchasedProductInfo;
        if (list == null) return false;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].productId == productId)
                return true;
        }

        return false;
    }

    public void DebugUnlockNoAds()
    {
        Debug.Log("[IAP][DEBUG] Force unlock No Ads");

        PlayerPrefs.SetInt(NO_ADS_KEY, 1);
        PlayerPrefs.Save();

        NoAdsManager.Instance.SetNoAdsPurchased();
    }

    public void DebugResetNoAds()
    {
        Debug.Log("[IAP][DEBUG] Reset No Ads");

        PlayerPrefs.DeleteKey(NO_ADS_KEY);
        PlayerPrefs.Save();

        // 즉시 반영
        NoAdsManager.Instance.ResetNoAdsPurchased(); 
    }
}
