using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button corridorButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitGameButton;

    [Header("오디오")]
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("씬 전환 효과")]
    [SerializeField] private FadeEffect fadePanel;
    
    [Header("옵션 설정")]
    [SerializeField] private GameObject optionsPanel;

    [Header("씬 매니저")]
    [SerializeField] private SceneManagerCorridor sceneManagerCorridor;
    private AudioSource _sfxSource;

    private void Awake()
    {
        _sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnNewGameClick()
    {
        PlayButtonSound();
        SetButtonsInteractable(false);
        StartCoroutine(fadePanel.LoadSceneWithFade("Stage0Scene"));
    }

    public void OnCorridorClick()
    {
        PlayButtonSound();
        SceneManager.sceneUnloaded -= sceneManagerCorridor.OnSceneUnloaded;
        SceneManager.sceneUnloaded += sceneManagerCorridor.OnSceneUnloaded; 
        sceneManagerCorridor.sceneName = "TitleScene";
        StartCoroutine(fadePanel.LoadSceneWithFade("CorridorScene"));
    }

    public void OnOptionsClick()
    {
        PlayButtonSound();
        ToggleOptionsPanel();
    }

    public void OnQuitGameClick()
    {
        PlayButtonSound();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null && _sfxSource != null)
            _sfxSource.PlayOneShot(buttonClickSound);
    }
    
    

    private void SetButtonsInteractable(bool interactable)
    {
        if (newGameButton != null) newGameButton.interactable = interactable;
        if (corridorButton != null) corridorButton.interactable = interactable;
        if (optionsButton != null) optionsButton.interactable = interactable;
        if (quitGameButton != null) quitGameButton.interactable = interactable;
    }
    
    public void ToggleOptionsPanel()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
}