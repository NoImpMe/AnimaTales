using UnityEngine;

public class CorridorUIManager : MonoBehaviour
{
    [Header("UI List")]
    [SerializeField] private GameObject animaDex;
    [SerializeField] private GameObject mixDex;
    [SerializeField] private GameObject mixDetail;

    [Header("버튼 사운드")]
    [SerializeField] private AudioClip btnClip;

    public void OpenAnimaDex()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        animaDex.SetActive(true);
    }
    public void CloseAnimaDex() 
    {
        AudioManager.Instance.PlaySFX(btnClip);
        animaDex.SetActive(false);
    }
    public void OpenMixDex()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        mixDex.SetActive(true);
    }
    public void CloseMixDex() 
    {
        AudioManager.Instance.PlaySFX(btnClip);
        mixDetail.SetActive(false);
        mixDex.SetActive(false);
    }
    public void ToggleChanged()
    {
        AudioManager.Instance.PlaySFX(btnClip);
    }
}