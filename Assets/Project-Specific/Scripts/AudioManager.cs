using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource effectsSource;

    [Header("Audio Clips")]
    public AudioClip[] backgroundMusic;
    public AudioClip[] soundEffects;

    [Header("UI Elements")]
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;

    private bool musicIsOn;
    private bool effectsAreOn;

    private Dictionary<string, AudioClip> effectsDictionary;

    private const string MusicPrefKey = "MusicIsOn";
    private const string EffectsPrefKey = "EffectsAreOn";
    private const string MusicVolumePrefKey = "MusicVolume";
    private const string EffectsVolumePrefKey = "EffectsVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeEffectsDictionary();

        // Initialize music and effects settings based on PlayerPrefs
        musicIsOn = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        effectsAreOn = PlayerPrefs.GetInt(EffectsPrefKey, 1) == 1;

        // Initialize volume settings based on PlayerPrefs
        musicSource.volume = PlayerPrefs.GetFloat(MusicVolumePrefKey, 1.0f);
        effectsSource.volume = PlayerPrefs.GetFloat(EffectsVolumePrefKey, 1.0f);

        // Initialize UI sliders
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicSource.volume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (effectsVolumeSlider != null)
        {
            effectsVolumeSlider.value = effectsSource.volume;
            effectsVolumeSlider.onValueChanged.AddListener(SetEffectsVolume);
        }
    }

    public void PlayMusicMenu()
    {
        if (musicIsOn)
        {
            PlayMusic(backgroundMusic[0]);
        }
    }

    public void PlayMusicRun()
    {
        if (musicIsOn)
        {
            PlayMusic(backgroundMusic[1]);
        }
    }

    private void InitializeEffectsDictionary()
    {
        effectsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundEffects)
        {
            effectsDictionary[clip.name] = clip;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicIsOn)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ToggleMusic()
    {
        musicIsOn = !musicIsOn;
        PlayerPrefs.SetInt(MusicPrefKey, musicIsOn ? 1 : 0);
        PlayerPrefs.Save();

        if (musicIsOn)
        {
            PlayMusic(GameManager.Instance.GetCurrentState() == GameState.StartMenu ? backgroundMusic[0] : backgroundMusic[1]);
        }
        else
        {
            StopMusic();
        }
    }

    public void PlayEffect(string clipName)
    {
        if (effectsAreOn && effectsDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            effectsSource.PlayOneShot(clip);
        }
        else if (!effectsAreOn)
        {
            Debug.LogWarning("Sound effects are turned off");
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + clipName);
        }
    }

    public void StopEffects()
    {
        effectsSource.Stop();
    }

    public void ToggleEffects()
    {
        effectsAreOn = !effectsAreOn;
        PlayerPrefs.SetInt(EffectsPrefKey, effectsAreOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    public void SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
        PlayerPrefs.SetFloat(EffectsVolumePrefKey, volume);
        PlayerPrefs.Save();
    }
}