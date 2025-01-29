using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Manages all in-game sounds, including background music and sound effects.
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    /// <summary>
    /// Enum defining different sound types available in the game.
    /// </summary>
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
    /// <summary>
    /// Initializes the singleton instance and sets up audio sources.
    /// </summary>
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
    /// <summary>
    /// Marks the game as started to allow playing sounds.
    /// </summary>
    private void Start()
    {
        _gameStarted = true;
    }
    /// <summary>
    /// Plays a specific sound effect.
    /// </summary>
    /// <param name="soundType">The type of sound to play.</param>
    /// <param name="objTransform">The transform of the object triggering the sound.</param>
    /// <param name="isSpatial">Determines if the sound should be spatialized.</param>
    /// <param name="delay">Delay before playing the sound.</param>
    /// <param name="volume">Volume of the sound.</param>
    /// <param name="isLoop">Indicates if the sound should loop.</param>
    public void PlaySound(SoundType soundType, Transform objTransform, bool isSpatial = false, float delay = 0, float volume = 0.8f, bool isLoop = false)
    {
        if (!_gameStarted || !soundClips.TryGetValue(soundType, out AudioClip clip) || clip == null)
        {
            Debug.LogWarning($"Sound '{soundType}' not found or game not started.");
            return;
        }

        var audioSource = _audioSources[_currentAudioSourceIndex];
        _currentAudioSourceIndex = (_currentAudioSourceIndex + 1) % _audioSources.Length;

        audioSource.transform.position = objTransform.position;
        audioSource.spatialBlend = isSpatial ? 1f : 0f;
        audioSource.pitch = 1f;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = isLoop;
        audioSource.Pause();
        audioSource.PlayDelayed(delay);
    }
    /// <summary>
    /// Plays the background music if it's not already playing.
    /// </summary>
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
    /// <summary>
    /// Stops the background music if it's currently playing.
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }
}
