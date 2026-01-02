using UnityEngine;

public class TimingGameController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] RectTransform pointer;
    [SerializeField] RectTransform successZone;
    [SerializeField] RectTransform clickEffect;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float normalSpeed;
    [SerializeField] float baseSpeed = 600f;
    [SerializeField] float barWidth = 600f;
    [SerializeField] float YPos = 5f;
    [SerializeField] float startXPos = -358f;

    float timer;

    private void Start()
    {
        speed = baseSpeed;
        normalSpeed = speed;
    }

    private void Update()
    {
        if (!GameManager.Instance.isRunning)
            return;

        timer += Time.deltaTime * speed;

        float x = Mathf.PingPong(timer, barWidth) - barWidth / 2f;
        pointer.anchoredPosition = new Vector2(x, YPos);
    }

    public void StartGame()
    {
        timer = 0f;
        GameManager.Instance.isRunning = true;
    }

    public void StopGame()
    {
        GameManager.Instance.isRunning = false;
    }

    public void CheckResult()
    {

        if (IsSuccess())
        {
            GameManager.Instance.score++;
            ClickEffect();
            Debug.Log("SUCCESS!");
        }

        else
        {
            GameManager.Instance.GameEnd();
            Debug.Log("Fail!!");
        }
    }

    bool IsSuccess()
    {
        float px = pointer.anchoredPosition.x;

        float left = successZone.anchoredPosition.x - successZone.rect.width / 2f;
        float right = successZone.anchoredPosition.x + successZone.rect.width / 2f;

        return px >= left && px <= right;
        
    }

    void ClickEffect()
    {
        RectTransform vfx = Instantiate(
        clickEffect,
        pointer.parent   
        );

        vfx.anchoredPosition = pointer.anchoredPosition;

    }

    public void SetEnd()
    {
        pointer.anchoredPosition = new Vector2(startXPos, YPos);
        speed = 600f;
        normalSpeed = speed;

    }

    public void SpeedChange()
    {
        switch(GameManager.Instance.speedState)
        {
            case GameManager.SpeedState.Normal:
                speed = normalSpeed;
                break;

            case GameManager.SpeedState.Fast:
                speed *= 2;
                break;

            case GameManager.SpeedState.Slow:
                speed /= 2;
                break;
        }
    }

    public void NormalSpeedUP()
    {
        normalSpeed += 100;
    }

    public float AnimSpeedSet()
    {
        return speed / baseSpeed;
    }

    
}
