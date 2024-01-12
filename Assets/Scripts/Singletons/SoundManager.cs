using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class SoundManager : SingletonPersistent<SoundManager> {
    private const string VOLUME_SFX = "volume_sfx";
    private const string VOLUME_MUSIC = "volume_music";
    private const int SOUND_POOL_LENGTH = 10;

    [SerializeField] bool isMusicEnabled = true;
    [SerializeField] bool isSFXEnabled = true;
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] GameObject audioSourcePrefab;

    [SerializeField, FoldoutGroup("Audio Clips", Expanded = false)] AudioClip buttonSFX;
    [SerializeField, FoldoutGroup("Audio Clips", Expanded = false)] AudioClip titleBGM;
    [SerializeField, FoldoutGroup("Audio Clips", Expanded = false)] AudioClip gameBGM;

    private float volumeSFX;
    private float volumeMusic;
    private List<GameObject> sourcePool = new List<GameObject>();

    public bool IsMusicEnabled { get { return isMusicEnabled; } set { isMusicEnabled = value; } }
    public bool IsSFXEnabled { get { return isSFXEnabled; } set { isSFXEnabled = value; } }


    public override void Awake() {
        base.Awake();

        //SFX ObjectPool
        CreateSourcePool();
    }

    private void Start() {
        UpdateVolume();

        GameManager.Instance.OnGamePaused += OnPause;
        GameManager.Instance.OnGameResumed += OnResume;
    }

    private void OnDestroy() {
        GameManager.Instance.OnGamePaused -= OnPause;
        GameManager.Instance.OnGameResumed -= OnResume;
    }

    private void OnPause() {
        if (musicAudioSource != null) {
            musicAudioSource.Pause();
        }
    }

    private void OnResume() {
        if (musicAudioSource != null) {
            musicAudioSource.UnPause();
        }
    }

    private void CreateSourcePool() {
        for (int i = 0;i < SOUND_POOL_LENGTH;i++) {
            GameObject obj = Instantiate(audioSourcePrefab);
            obj.transform.parent = transform;
            sourcePool.Add(obj);
        }
    }

    private void UpdateVolume() {
        volumeSFX = PlayerPrefs.GetFloat(VOLUME_SFX);
        volumeMusic = PlayerPrefs.GetFloat(VOLUME_MUSIC);
        mixer.SetFloat(MixerGroup.SFX.ToString(), Mathf.Log10(volumeSFX) * 20);
        mixer.SetFloat(MixerGroup.Music.ToString(), Mathf.Log10(volumeMusic) * 20);
    }

    public void UpdateSFXVolume(float volume) {
        volumeSFX = volume;
        PlayerPrefs.SetFloat(VOLUME_SFX, volume);
        mixer.SetFloat(MixerGroup.SFX.ToString(), Mathf.Log10(volumeSFX) * 20);
    }

    public void UpdateMusicVolume(float volume) {
        volumeMusic = volume;
        PlayerPrefs.SetFloat(VOLUME_MUSIC, volume);
        mixer.SetFloat(MixerGroup.Music.ToString(), Mathf.Log10(volumeMusic) * 20);
    }

    public void PlaySound(AudioClip clip) {
        if (!isMusicEnabled)
            return;

        if (musicAudioSource != null) {
            if (musicAudioSource.isPlaying) {
                musicAudioSource.Stop();
            }

            musicAudioSource.loop = true;
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip) {
        if (!isSFXEnabled)
            return;

        AudioSource sfxSource = GetSourceFromPool();
        sfxSource.clip = clip;
        sfxSource.PlayOneShot(clip);
    }

    private AudioSource GetSourceFromPool() {
        AudioSource freeSource = null;
        foreach (GameObject obj in sourcePool) {
            AudioSource source = obj.GetComponent<AudioSource>();
            if (!source.isPlaying) {
                freeSource = source;
                break;
            }
        }

        return freeSource;
    }

    //Temporary
    [Button]
    public void PlayTitleBGM() {
        PlaySound(titleBGM);
    }

    [Button]
    public void PlayGameBGM() {
        PlaySound(gameBGM);
    }

    [Button]
    public void PlayBtnSFX() {
        PlaySFX(buttonSFX);
    }
}

public enum MixerGroup {
    Music,
    SFX
}

