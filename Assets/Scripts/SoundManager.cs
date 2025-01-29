using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public enum SoundType
    {
        Jump,
        Clap,
        Hit,
        LevelReset,
        MoneyCollection,
        CoinSpawn,
        PointUp
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private int maxAudioSources = 10;
    private AudioSource[] _audioSources;
    private int _currentAudioSourceIndex = 0;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip clapClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip levelResetClip;
    [SerializeField] private AudioClip moneyCollectionClip;
    [SerializeField] private AudioClip coinSpawnClip;
    [SerializeField] private AudioClip pointUpClip;

    private Dictionary<SoundType, AudioClip> soundClips;
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

        _audioSources = new AudioSource[maxAudioSources];
        for (int i = 0; i < maxAudioSources; i++)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
        }

        // מילוי המפה בצלילים
        soundClips = new Dictionary<SoundType, AudioClip>
        {
            { SoundType.Jump, jumpClip },
            { SoundType.Clap, clapClip },
            { SoundType.Hit, hitClip },
            { SoundType.LevelReset, levelResetClip },
            { SoundType.MoneyCollection, moneyCollectionClip },
            { SoundType.CoinSpawn, coinSpawnClip },
            { SoundType.PointUp, pointUpClip }
        };
    }

    private void Start()
    {
        _gameStarted = true;
    }

    public void PlaySound(SoundType soundType, Transform objTransform, bool isSpatial = false, float delay = 0, float volume = 0.8f, bool isLoop = false)
    {
        if (!_gameStarted || !soundClips.TryGetValue(soundType, out AudioClip clip) || clip == null)
        {
            Debug.LogWarning($"Sound '{soundType}' not found or game not started.");
            return;
        }

        // בחירת מקור שמע זמין
        var audioSource = _audioSources[_currentAudioSourceIndex];
        _currentAudioSourceIndex = (_currentAudioSourceIndex + 1) % _audioSources.Length;

        // הגדרת הפרמטרים של השמע
        audioSource.transform.position = objTransform.position;
        audioSource.spatialBlend = isSpatial ? 1f : 0f;
        audioSource.pitch = 1f;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = isLoop;
        audioSource.Pause();
        audioSource.PlayDelayed(delay);
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicClip != null && !backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.clip = backgroundMusicClip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.spatialBlend = 0f; 
            backgroundMusicSource.volume = 0.2f;
            backgroundMusicSource.pitch = 1f;
            backgroundMusicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }
}
