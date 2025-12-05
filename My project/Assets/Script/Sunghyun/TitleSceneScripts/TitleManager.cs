using BansheeGz.BGDatabase;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button corridorButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button tutorialButton;

    [Header("오디오")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip btnClip;

    [Header("씬 전환 효과")]
    [SerializeField] private FadeEffect fadePanel;
    
    [Header("옵션 설정")]
    [SerializeField] private GameObject optionsPanel;

    [Header("씬 매니저")]
    [SerializeField] private SceneManagerCorridor sceneManagerCorridor;
    
    private void Awake()
    {
        AudioManager.Instance.PlayBGM(bgmClip);
        var database = BGRepo.I;
        var table = database.GetMeta("GoldData");
        table.ForEachEntity(e =>
        {
            if (!e.Get<bool>("TutoCleared"))
            {
                tutorialButton.gameObject.SetActive(true);
            }
        });
    }
    public void TutorialClick()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        GameObject.Find("Game Manager").GetComponent<GoldManager>().Init();
        GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo.TutorInitialize();
        DestroyImmediate(tutorialButton);
        SetButtonsInteractable(false);
        StartCoroutine(fadePanel.LoadSceneWithFade("Stage0Scene"));
    }
    public void OnNewGameClick()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        GameObject.Find("Game Manager").GetComponent<GoldManager>().Init();
        GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo.Initialize();
        SetButtonsInteractable(false);
        StartCoroutine(fadePanel.LoadSceneWithFade("Stage0Scene"));
    }

    public void OnCorridorClick()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        SceneManager.sceneUnloaded -= sceneManagerCorridor.OnSceneUnloaded;
        SceneManager.sceneUnloaded += sceneManagerCorridor.OnSceneUnloaded; 
        sceneManagerCorridor.sceneName = "TitleScene";
        StartCoroutine(fadePanel.LoadSceneWithFade("CorridorScene"));
    }

    public void OnOptionsClick()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        ToggleOptionsPanel();
    }

    public void OnQuitGameClick()
    {
        AudioManager.Instance.PlaySFX(btnClip);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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