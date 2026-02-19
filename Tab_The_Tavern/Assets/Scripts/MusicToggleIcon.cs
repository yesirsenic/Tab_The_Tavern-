using UnityEngine;
using UnityEngine.UI;

public class MusicToggleIcon : MonoBehaviour
{
    [Header("UI")]
    public Image musicIcon;

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Visual Settings")]
    [Range(0f, 1f)]
    public float offAlpha = 0.4f;

    private bool isMuted = false;
    private float originalVolume;

    void Start()
    {
        originalVolume = bgmSource.volume;
    }

    public void OnClickMusicButton()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            bgmSource.volume = 0f;
            sfxSource.volume = 0f;
            SetIconAlpha(offAlpha);
        }
        else
        {
            bgmSource.volume = originalVolume;
            sfxSource.volume = originalVolume;
            SetIconAlpha(1f);
        }
    }

    void SetIconAlpha(float alpha)
    {
        Color c = musicIcon.color;
        c.a = alpha;
        musicIcon.color = c;
    }
}
