using UnityEngine;

public class SuccessEffect : MonoBehaviour
{
    public void OnVFXEnd()
    {
        Destroy(gameObject);
    }
}
