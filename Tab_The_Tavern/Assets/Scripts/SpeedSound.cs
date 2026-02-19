using UnityEngine;

public class SpeedSound : MonoBehaviour
{
    public void PlaySpeed()
    {
        SFXManager.Instance.PlaySFX(SFXType.Speed);
    }

    public void PlayUp()
    {
        SFXManager.Instance.PlaySFX(SFXType.Up);
    }

    public void PlayDown()
    {
        SFXManager.Instance.PlaySFX(SFXType.Down);
    }

    public void PlayNormal()
    {
        SFXManager.Instance.PlaySFX(SFXType.Normal);
    }
}
