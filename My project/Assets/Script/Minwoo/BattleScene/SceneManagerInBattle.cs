using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerInBattle : MonoBehaviour
{
    private GameObject regionManager;
    private FadeEffect fadePanel;
    [SerializeField] private AudioClip[] bgmClips;
    private void Start()
    {
        fadePanel = GameObject.Find("Fade Panel").GetComponent<FadeEffect>();
    }
    public void backToTiles()
    {
        var regionScr = RegionManager.Instance;
        regionScr.isClicked = false;
        var num = regionScr.stageNum;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.EndsWith("LastBossScene")){
            StartCoroutine(fadePanel.LoadSceneWithFade("TitleScene"));
        }
        else
        {
            AudioManager.Instance.PlayBGM(bgmClips[regionScr.currentStageType]);
            StartCoroutine(fadePanel.LoadSceneWithFade("Stage0Scene"));
        }
    }

    public void resetGame()
    {
        StartCoroutine(fadePanel.LoadSceneWithFade("TitleScene"));
        GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo.Initialize();
        GoldManager.Instance.Init();
    }
}
