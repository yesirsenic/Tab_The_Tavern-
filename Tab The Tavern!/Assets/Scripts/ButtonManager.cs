using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void ButtonSFXPlay()
    {
        SFXManager.Instance.PlaySFX(SFXType.ButtonClick);
    }
}
