using System.Collections;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    private int lastTriggeredScore = 0;
    private Coroutine speedCoroutine;

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

        if(currentScore >= lastTriggeredScore +10)
        {
            lastTriggeredScore = (currentScore / 10) * 10;

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

        speedCoroutine = null;
    }

    public void GameEnd()
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(SpeedChangeRoutine());
            speedCoroutine = null;
        }

        lastTriggeredScore = 0;
    }
}
