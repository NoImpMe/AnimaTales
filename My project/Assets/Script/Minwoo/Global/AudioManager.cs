using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer; // ?? 인스펙터에 AudioMixer 할당
    [SerializeField] private List<AudioClip> audioClips;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

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
        float dbValue = Mathf.Log10(PreferenceData.BgmVolume / 100f) * 20;
        float masterDb = Mathf.Log10(PreferenceData.MasterVolume / 100f) * 20;
        audioMixer.SetFloat("BGMVolume", dbValue + masterDb);
    }

    private void ApplySfxVolume()
    {
        float dbValue = Mathf.Log10(PreferenceData.SfxVolume / 100f) * 20;
        float masterDb = Mathf.Log10(PreferenceData.MasterVolume / 100f) * 20;
        audioMixer.SetFloat("SFXVolume", dbValue + masterDb);
    }
    public void BgmChange()
    {
        
    }
}
