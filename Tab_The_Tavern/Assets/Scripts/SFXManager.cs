using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX List")]
    [SerializeField] private List<SFXData> sfxList;

    private Dictionary<SFXType, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxDict = new Dictionary<SFXType, AudioClip>();
        foreach (var sfx in sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.type))
                sfxDict.Add(sfx.type, sfx.clip);
        }
    }

    public void PlaySFX(SFXType type, float volume = 1f)
    {
        if (!sfxDict.TryGetValue(type, out var clip))
        {
            Debug.LogWarning($"SFX ¾øÀ½ : {type}");
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }
}
