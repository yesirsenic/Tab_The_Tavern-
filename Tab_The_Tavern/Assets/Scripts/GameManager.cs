using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] TimingGameController timingGameController;
    [SerializeField] CharacterAnimator characterAnimator;
    [SerializeField] SpeedManager speedManager;
    [SerializeField] GameObject StartButton;
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject NoAdsButton;
    [SerializeField] GameObject NoAdsPopup;
    [SerializeField] GameObject NoAdsBlackPanel;

    public enum SpeedState
    {
        Normal, Fast, Slow
    }

    public SpeedState speedState = SpeedState.Normal;
    public bool isRunning = true;
    public int score = 0;
    public int bestScore = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        GetBestScore();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
#if UNITY_WEBGL
        NoAdsButton.SetActive(false);
#endif

        CheckPurchasedNoAds();
        BGMManager.Instance.PlayBGM(BGMType.Main);
    }

    private void Update()
    {
        if(scoreText.text != score.ToString())
        {
            scoreText.text = score.ToString();
        }
    }

    private void GetBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = bestScore.ToString();
        
    }

    private void SetBestScore(int num)
    {
        if(num > bestScore)
        {
            bestScore = num;
            PlayerPrefs.SetInt("BestScore", bestScore);
            bestScoreText.text = bestScore.ToString();
        }
    }

    private void GameEnd()
    {
        timingGameController.SetEnd();
        speedManager.GameAfterEnd();
        StartButton.SetActive(true);
        characterAnimator.EndGame();
        SetBestScore(score);
        score = 0;
        AdsManager.Instance.OnplayerDied();
        BGMManager.Instance.PlayBGM(BGMType.Main);
    }

    private void GamePreEnd()
    {
        isRunning = false;
        speedManager.GameEnd();
        speedState = SpeedState.Normal;
        BGMManager.Instance.StopBGM();
    }

    IEnumerator GameEndAnim()
    {
        explosion.SetActive(true);
        characterAnimator.gameObject.SetActive(false);
        GamePreEnd();
        Animator animator = explosion.GetComponent<Animator>();
        animator.Play("Explosion");

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        float length = info.length;
        float speed = animator.speed == 0 ? 1f : animator.speed;

        yield return new WaitForSeconds(length / speed);

        explosion.SetActive(false);

        yield return new WaitForSeconds(1f);

        characterAnimator.gameObject.SetActive(true);

        GameEnd();
    }

    private void CheckPurchasedNoAds()
    {
        if(NoAdsManager.Instance.HasNoAds ==true)
        {
            NoAdsButton.SetActive(false);
            NoAdsPopup.SetActive(false);
            BannerAd.Instance.Hide();
        }
    }

    public void AnimSpeedUp()
    {
        Animator characterAnim = characterAnimator.GetAnimator();
        Animator speedAnim = speedManager.GetSpeedAnimator();

        characterAnim.speed = timingGameController.AnimSpeedSet();
        speedAnim.speed = timingGameController.SpeedAnimSpeedSet();
    }

    public void StartGame()
    {
        timingGameController.StartGame();
        characterAnimator.StartGame();
        speedManager.GameStart();
        BGMManager.Instance.PlayBGM(BGMType.Game);
    }

    

    public void GameEndCorutineStart()
    {
        StartCoroutine(GameEndAnim());
    }

    public void PurchasedNoAds()
    {
        NoAdsButton.SetActive(false);
        NoAdsPopup.SetActive(false);  
        NoAdsBlackPanel.SetActive(false);

        BannerAd.Instance.Hide();
    }

    
    
    

   

    
}
