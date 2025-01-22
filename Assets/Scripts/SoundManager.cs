using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [Header("Charlie sounds")]
    [SerializeField] private AudioSource charlieMusicSource;
    [SerializeField] private int maxAudioSources = 10;
    private AudioSource[] _audioSources;
    private int _currentAudioSourceIndex = 0;
    
    [Header("Clips")]
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private AudioClip CharlieJumpClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip levelResetClip;
    [SerializeField] private AudioClip moneyCollectionClip;
    
    [Header("Background Music")]
    [SerializeField] private AudioSource backgroundMusicSource;
    
    private bool _gameStarted = false;

    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        }
        if (charlieMusicSource == null)
        {
            charlieMusicSource = gameObject.AddComponent<AudioSource>();
        }
        
        
        _audioSources = new AudioSource[maxAudioSources];
        for (int i = 0; i < maxAudioSources; i++)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void Start()
    {
        _gameStarted = true;
        PlayBackgroundMusic();
    }
    
    private void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicClip != null)
        {
            backgroundMusicSource.clip = backgroundMusicClip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.spatialBlend = 0f; 
            backgroundMusicSource.volume = 0.05f;
            backgroundMusicSource.pitch = 1f;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.LogError("Background music source or clip is missing!");
        }

    }
    
    public void PlaySFX(AudioClip clip,  Transform objTransform, bool isSpatial = false, float delay = 0, float volume = 0.8f)
    {
        if (!_gameStarted || clip == null)
            return;
        
        // Get the next AudioSource
        var audioSource = _audioSources[_currentAudioSourceIndex];
        _currentAudioSourceIndex = (_currentAudioSourceIndex + 1) % _audioSources.Length;

        // Configure the AudioSource
        audioSource.transform.position = objTransform.position;
        audioSource.spatialBlend = isSpatial ? 1f : 0f;
        
        audioSource.pitch = 1f;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.PlayDelayed(delay);
    }
    
    public void PlayCharlieJumpSound(Transform objTransform)
    {
        PlaySFX(CharlieJumpClip, objTransform, true, 0, 1f);
    }

    public void PlayWinSound(Transform objTransform)
    {
        PlaySFX(winClip, objTransform, true, 0, 1f);
    }

    public void PlayHitSound(Transform objTransform)
    {
        PlaySFX(hitClip, objTransform, true, 0, 1f);
    }

    public void PlayLevelResetSound(Transform objTransform)
    {
        PlaySFX(levelResetClip, objTransform, true, 0.5f, 1f);
    }

    public void PlayMoneyCollectionSound(Transform objTransform)
    {
        PlaySFX(moneyCollectionClip, objTransform, true, 0, 1f);
    }
}
