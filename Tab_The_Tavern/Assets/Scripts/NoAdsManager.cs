using UnityEngine;

public class NoAdsManager : MonoBehaviour
{
    public static NoAdsManager Instance;

    private const string NO_ADS_KEY = "NO_ADS_PURCHASED";

    public bool HasNoAds { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Load()
    {
        HasNoAds = PlayerPrefs.GetInt(NO_ADS_KEY, 0) == 1;
    }

    public void SetNoAdsPurchased()
    {
        HasNoAds = true;
        PlayerPrefs.SetInt(NO_ADS_KEY, 1);
        PlayerPrefs.Save();

        GameManager.Instance.PurchasedNoAds();
    }

    public void ResetNoAdsPurchased()
    {
        HasNoAds = false;
        PlayerPrefs.SetInt(NO_ADS_KEY, 0);
        PlayerPrefs.Save();
    }
}
