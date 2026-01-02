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

    public void AnimSpeedUp()
    {
        Animator characterAnim = characterAnimator.GetAnimator();
        Animator speedAnim = speedManager.GetSpeedAnimator();

        characterAnim.speed = timingGameController.AnimSpeedSet();
        speedAnim.speed = timingGameController.AnimSpeedSet();
    }

    public void StartGame()
    {
        timingGameController.StartGame();
        characterAnimator.StartGame();
    }

    public void GameEnd()
    {
        isRunning = false;
        StartButton.SetActive(true);
        timingGameController.SetEnd();
        speedManager.GameEnd();
        characterAnimator.EndGame();
        SetBestScore(score);
        score = 0;
        speedState = SpeedState.Normal;
    }

   

    
}
