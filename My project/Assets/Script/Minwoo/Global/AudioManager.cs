using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer; 
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmSource.loop = true;
        sfxSource.outputAudioMixerGroup = sfxGroup;
        sfxSource.loop = false;
        // PreferenceData에서 이벤트 구독
        PreferenceData.AddListenerBgmVolumeChangeEvent(ApplyBgmVolume);
        PreferenceData.AddListenerSfxVolumeChangeEvent(ApplySfxVolume);

        // 게임 시작 시 현재 설정 반영
        ApplyAllVolume();
    }

    private void OnDestroy()
    {
        PreferenceData.RemoveListenerBgmVolumeChangeEvent(ApplyBgmVolume);
        PreferenceData.RemoveListenerSfxVolumeChangeEvent(ApplySfxVolume);
    }

    private void ApplyAllVolume()
    {
        ApplyBgmVolume();
        ApplySfxVolume();
    }

    private void ApplyBgmVolume()
    {
        float bgm = PreferenceData.BgmVolume / 100f;
        float master = PreferenceData.MasterVolume / 100f;
        float bgmVolume = bgm * master;
        float dB = (bgmVolume <= 0f) ? -80f : Mathf.Log10(bgmVolume) * 20f;
        audioMixer.SetFloat("BGMVolume", dB);
    }

    private void ApplySfxVolume()
    {
        float sfx = PreferenceData.SfxVolume / 100f;
        float master = PreferenceData.MasterVolume / 100f;
        float sfxVolume = sfx * master;
        float dB = (sfxVolume <= 0f) ? -80f : Mathf.Log10(sfxVolume) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);
    }
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
