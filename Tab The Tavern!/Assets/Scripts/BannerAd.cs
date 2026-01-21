using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour, IUnityAdsInitializationListener
{
    public static BannerAd Instance;

#if UNITY_ANDROID
    private const string GAME_ID = "6030245";
    private const string BANNER_ID = "Banner_Android";
#elif UNITY_IOS
    private const string GAME_ID = "YOUR_IOS_GAME_ID";
    private const string BANNER_ID = "Banner_iOS";
#endif

    private bool isInitialized = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(GAME_ID, true, this);
        }
        else
        {
            isInitialized = true;
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialized");
        isInitialized = true;

        Show();
    }

    public void OnInitializationFailed(
        UnityAdsInitializationError error,
        string message)
    {
        Debug.LogError($"Ads Init Failed: {error} - {message}");
    }

    public void Show()
    {
        if (!isInitialized)
        {
            Debug.Log("Ads not initialized yet");
            return;
        }

        Advertisement.Banner.Load(BANNER_ID, new BannerLoadOptions
        {
            loadCallback = () =>
            {
                Advertisement.Banner.Show(BANNER_ID);
            },
            errorCallback = (error) =>
            {
                Debug.Log("Banner Load Error: " + error);
            }
        });
    }

    public void Hide()
    {
        Advertisement.Banner.Hide();
    }
}
