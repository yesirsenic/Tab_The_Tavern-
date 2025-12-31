using UnityEngine;

public class TimingGameController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] RectTransform pointer;
    [SerializeField] RectTransform successZone;

    [Header("Movement")]
    [SerializeField] float speed = 600f;
    [SerializeField] float barWidth = 600f;
    [SerializeField] float YPos = 5f;

    float timer;

    private void Start()
    {
        StartGame();
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
            Debug.Log("SUCCESS!");
        }

        else
        {
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

    
}
