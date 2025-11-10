using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType { Master, BGM, SFX }
    public VolumeType volumeType;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        switch (volumeType)
        {
            case VolumeType.Master: PreferenceData.MasterVolume = (int)value; break;
            case VolumeType.BGM: PreferenceData.BgmVolume = (int)value; break;
            case VolumeType.SFX: PreferenceData.SfxVolume = (int)value; break;
        }
    }
}
