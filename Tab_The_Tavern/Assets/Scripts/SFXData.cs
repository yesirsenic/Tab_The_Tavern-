using UnityEngine;
using UnityEngine.Audio;

public enum SFXType
{
    ButtonClick,
    Success,
    Explosion,
    Speed,
    Up,
    Down,
    Normal
       
}

[System.Serializable]
public class SFXData 
{
    public SFXType type;
    public AudioClip clip;
}
