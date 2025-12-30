using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TimeGamingTouch : MonoBehaviour
{

    [SerializeField]
    GameObject timingGameController;

    TimingGameController timingGame;

    private void Awake()
    {
        timingGame = timingGameController.GetComponent<TimingGameController>();
    }
    private void Update()
    {
        if(Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            if (IsTouchOnUIButton())
                return;

            OnTap();
        }

#if UNITY_EDITOR
        // 🖱 에디터 / PC 클릭
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (IsTouchOnUIButton())
                return;

            OnTap();
        }

#endif
    }

    void OnTap()
    {
        if (!GameManager.Instance.isRunning)
            return;

        CheckResult();


    }

    bool IsTouchOnUIButton()
    {
        if (EventSystem.current == null)
            return false;

        
        if (Touchscreen.current != null)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            bool overUI = EventSystem.current.IsPointerOverGameObject(touchId);
            return overUI;
        }

#if UNITY_EDITOR
        
        bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();
        return mouseOverUI;
#else
    return false;
#endif
    }

    void CheckResult()
    {
        timingGame.CheckResult();
    }
}
