using UnityEngine;

public enum BGMType
{
    Main,
    Game
}

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource bgmSource;

    [Header("BGM Clips")]
    [SerializeField] AudioClip mainBGM;
    [SerializeField] AudioClip gameBGM;

    BGMType currentBGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGMType type)
    {
        if (currentBGM == type && bgmSource.isPlaying)
            return;

        AudioClip clip = type switch
        {
            BGMType.Main => mainBGM,
            BGMType.Game => gameBGM,
            _ => null
        };

        if (clip == null)
            return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();

        currentBGM = type;
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetVolume(float volume)
    {
        bgmSource.volume = volume;
    }
}
