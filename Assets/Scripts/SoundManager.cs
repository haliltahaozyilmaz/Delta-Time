using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Mixer Ayarı")]
    public AudioMixerGroup sfxGroup;

    [Header("Havuz Ayarları")]
    public int poolSize = 20; 
    private List<AudioSource> sfxPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializePool()
    {
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    AudioSource CreateNewAudioSource()
    {
        GameObject go = new GameObject("PooledSFX");
        go.transform.SetParent(transform);
        AudioSource source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = sfxGroup;
        source.playOnAwake = false;
        sfxPool.Add(source);
        return source;
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        
        source.spatialBlend = 0f; 
        
        source.Play();
    }

    AudioSource GetAvailableSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }

        return CreateNewAudioSource();
    }
}