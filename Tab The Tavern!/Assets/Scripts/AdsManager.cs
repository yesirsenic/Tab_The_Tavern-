using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [Header("Ad Settings")]
    [SerializeField] private int showAdEveryDeath = 3;
    private int deathCount = 0;

    private InterstitialAd interstitialAd;

// ⚠️ 실제 배포 시 본인 광고 ID로 교체
#if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // 테스트 ID
#elif UNITY_IOS
    private string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string adUnitId = "unused";
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        if(interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var request = new AdRequest();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Interstitial load failed");
                return;
            }

            interstitialAd = ad;

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                LoadInterstitial();
            };


        });
    }

    public void OnplayerDied()
    {
        deathCount++;

        if(deathCount >= showAdEveryDeath)
        {
            deathCount = 0;
            ShowInterstitial();
        }
    }

    private void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }

        else
        {
            Debug.Log("Interstitial not ready");
            LoadInterstitial();
        }
    }
}
