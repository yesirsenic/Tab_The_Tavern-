using System.Collections;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    private int lastTriggeredScore = 0;
    private int nextTriggeredScore = 10;
    private Coroutine speedCoroutine;
    private bool isAnimation = false;
    private int speedChangeCount = 0;

    [SerializeField]
    Animator SpeedAnimator;

    [SerializeField]
    TimingGameController gameController;

    [SerializeField]
    string[] speedAnimations =
    {
        "Speed Normal",
        "Speed Up",
        "Speed Down"
    };

    private enum AnimKey
    {
        Normal,Fast,Slow
    }

    private void Update()
    {
        CheckScore();
    }

    void CheckScore()
    {
        int currentScore = GameManager.Instance.score;

        if(currentScore >= nextTriggeredScore && !isAnimation)
        {
            lastTriggeredScore = nextTriggeredScore;

            isAnimation = true;
            StartSpeedChangeCoroutine();
        }
    }

    void StartSpeedChangeCoroutine()
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(SpeedChangeRoutine());
    }

    IEnumerator SpeedChangeRoutine()
    {
        SpeedAnimator.gameObject.SetActive(true);

        switch (GameManager.Instance.speedState)
        {
            case GameManager.SpeedState.Normal:
                GameManager.Instance.speedState =
                    Random.value < 0.5f ? GameManager.SpeedState.Fast
                    : GameManager.SpeedState.Slow;
                break;

            case GameManager.SpeedState.Fast:
                GameManager.Instance.speedState = GameManager.SpeedState.Normal;
                break;

            case GameManager.SpeedState.Slow:
                GameManager.Instance.speedState = GameManager.SpeedState.Normal;
                break;
        }

        switch (GameManager.Instance.speedState)
        {
            case GameManager.SpeedState.Normal:
                SpeedAnimator.Play(speedAnimations[(int)AnimKey.Normal], 0, 0f);
                break;

            case GameManager.SpeedState.Fast:
                SpeedAnimator.Play(speedAnimations[(int)AnimKey.Fast], 0, 0f);
                break;

            case GameManager.SpeedState.Slow:
                SpeedAnimator.Play(speedAnimations[(int)AnimKey.Slow], 0, 0f);
                break;
        }

        yield return null;

        AnimatorStateInfo info = SpeedAnimator.GetCurrentAnimatorStateInfo(0);

        float length = info.length;
        float speed = SpeedAnimator.speed == 0 ? 1f : SpeedAnimator.speed;

        yield return new WaitForSeconds(length / speed);

        SpeedAnimator.gameObject.SetActive(false);
        gameController.SpeedChange();
        SetNextTriggerScore();
        UpNormalSpeed();
        GameManager.Instance.AnimSpeedUp();
        isAnimation = false;
        speedCoroutine = null;
    }

    private void SetNextTriggerScore()
    {
        switch(GameManager.Instance.speedState)
        {
            case GameManager.SpeedState.Normal:
                int[] candidates = { 10, 15, 20 };
                nextTriggeredScore = lastTriggeredScore + candidates[Random.Range(0, candidates.Length)];
                break;

            case GameManager.SpeedState.Fast:
                int[] candidates_Fast = { 20, 25, 30,35 };
                nextTriggeredScore = lastTriggeredScore + candidates_Fast[Random.Range(0, candidates_Fast.Length)];
                break;

            case GameManager.SpeedState.Slow:
                int[] candidates_Slow = { 5,10 };
                nextTriggeredScore = lastTriggeredScore + candidates_Slow[Random.Range(0, candidates_Slow.Length)];
                break;

        }
    }

    private void UpNormalSpeed()
    {
        speedChangeCount++;

        if(speedChangeCount >=2)
        {
            speedChangeCount = 0;
            gameController.NormalSpeedUP();
        }
    }    

    public void GameEnd()
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(SpeedChangeRoutine());
            speedCoroutine = null;
        }

        lastTriggeredScore = 0;
        nextTriggeredScore = 10;
        isAnimation = false;
        speedChangeCount = 0;
        SpeedAnimator.speed = 1f;
    }

    public Animator GetSpeedAnimator()
    {
        return SpeedAnimator;
    }
}
