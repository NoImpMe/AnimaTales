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
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("씬 전환 효과")]
    [SerializeField] private FadeEffect fadeEffect;
    
    [Header("옵션 설정")]
    [SerializeField] private GameObject optionsPanel;

    [Header("씬 매니저")]
    [SerializeField] private SceneManagerCorridor sceneManagerCorridor;
    private AudioSource _sfxSource;

    private void Awake()
    {
        _sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        SetupButtonEvents();

        if (bgmSource != null && bgmSource.clip != null)
            bgmSource.Play();
    }

    private void SetupButtonEvents()
    {
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameClick);

        if (corridorButton != null)
            corridorButton.onClick.AddListener(OnCorridorClick);

        if (optionsButton != null)
            optionsButton.onClick.AddListener(OnOptionsClick);

        if (quitGameButton != null)
            quitGameButton.onClick.AddListener(OnQuitGameClick);
    }

    private void OnNewGameClick()
    {
        PlayButtonSound();
        
        LoadScene("Stage0Scene");
    }

    private void OnCorridorClick()
    {
        PlayButtonSound();
        SceneManager.sceneUnloaded -= sceneManagerCorridor.OnSceneUnloaded;
        SceneManager.sceneUnloaded += sceneManagerCorridor.OnSceneUnloaded; 
        sceneManagerCorridor.sceneName = "TitleScene";
        LoadScene("CorridorScene");
    }

    private void OnOptionsClick()
    {
        PlayButtonSound();
        ToggleOptionsPanel();

    }

    private void OnQuitGameClick()
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
    
    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }
    
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        SetButtonsInteractable(false);
    
        if (fadeEffect != null)
            yield return fadeEffect.FadeIn();
    
        SceneManager.LoadScene(sceneName);
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