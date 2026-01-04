using UnityEngine;
using UnityEngine.UI;

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

    [Header("Images")]
    [SerializeField] Sprite red_Ball;
    [SerializeField] Sprite Yellow_Ball;

    float timer;
    float prevX;
    float prevDir;
    float beatToCenter = 0.5f;
    float minBeat = 0.35f;
    bool isYellow;
    bool dirCheckReady = false;


    private void Start()
    {
        speed = baseSpeed;
        normalSpeed = speed;
        isYellow = false;

        prevX = pointer.anchoredPosition.x;
        prevDir = 1f;
        dirCheckReady = false;
        beatToCenter = 0.5f;
    }

    private void Update()
    {
        if (!GameManager.Instance.isRunning)
            return;

        timer += Time.deltaTime * speed;

        float x = Mathf.PingPong(timer, barWidth) - barWidth / 2f;
        pointer.anchoredPosition = new Vector2(x, YPos);

        if (!dirCheckReady)
        {
            prevX = x;
            dirCheckReady = true;
            return;
        }

        float deltaX = x - prevX;
        float currentDir = Mathf.Sign(deltaX);



        if (prevDir != 0f && currentDir != prevDir)
        {
            OnPointerDirectionChanged(currentDir);
        }

        prevDir = currentDir;
        prevX = x;
    }

    public void StartGame()
    {
        timer = 0f;
        GameManager.Instance.isRunning = true;
        speed = baseSpeed;
        normalSpeed = speed;
    }

    public void StopGame()
    {
        GameManager.Instance.isRunning = false;
    }

    public void CheckResult()
    {
        if (isYellow)
            return;

        if (IsSuccess())
        {
            GameManager.Instance.score++;
            ClickEffect();
            SFXManager.Instance.PlaySFX(SFXType.Success);
            Debug.Log("SUCCESS!");
        }

        else
        {
            GameManager.Instance.GameEndCorutineStart();
            SFXManager.Instance.PlaySFX(SFXType.Explosion);
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

        pointer.gameObject.GetComponent<Image>().sprite = Yellow_Ball;

        isYellow = true;



    }

    void OnPointerDirectionChanged(float newDir)
    {

        if (newDir < 0)
        {
            if (!isYellow)
            {
                CheckResult();
                return;
            }

            pointer.gameObject.GetComponent<Image>().sprite = red_Ball;
            isYellow = false;
        }

        else
        {
            if (!isYellow)
            {
                CheckResult();
                return;
            }

            pointer.gameObject.GetComponent<Image>().sprite = red_Ball;
            isYellow = false;
        }
    }

    public void SetEnd()
    {
        pointer.anchoredPosition = new Vector2(startXPos, YPos);
        speed = baseSpeed;
        normalSpeed = speed;
        isYellow = false;
        prevX = pointer.anchoredPosition.x;
        prevDir = 1f;
        dirCheckReady = false;
        beatToCenter = 0.5f;

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
        beatToCenter *= 0.9f;
        beatToCenter = Mathf.Max(minBeat, beatToCenter);

        normalSpeed = (barWidth / 2f) / beatToCenter;
    }

    public float AnimSpeedSet()
    {
        return speed / baseSpeed;
    }

    public float SpeedAnimSpeedSet()
    {
        return normalSpeed / baseSpeed;
    }

    
}
