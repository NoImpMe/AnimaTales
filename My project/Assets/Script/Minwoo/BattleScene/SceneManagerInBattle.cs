using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerInBattle : MonoBehaviour
{
    private GameObject regionManager;
    private FadeEffect fadePanel;

    private void Start()
    {
        fadePanel = GameObject.Find("Fade Panel").GetComponent<FadeEffect>();
    }
    public void backToTiles()
    {
        regionManager = GameObject.Find("RegionManager");
        var regionScr = regionManager.GetComponent<RegionManager>();
        var num = regionScr.stageType;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.EndsWith("LastBossScene")){
            StartCoroutine(fadePanel.LoadSceneWithFade("TitleScene"));
        }
        else
        {
            switch (num)
            {
                case 0:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage0Scene"));
                    break;
                case 1:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage1Scene"));
                    break;
                case 2:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage2Scene"));
                    break;
                case 3:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage3Scene"));
                    break;
                case 4:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage4Scene"));
                    break;
                case 5:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage5Scene"));
                    break;
                case 6:
                    StartCoroutine(fadePanel.LoadSceneWithFade("Stage6Scene"));
                    break;
            }
        }
    }

    public void resetGame()
    {
        StartCoroutine(fadePanel.LoadSceneWithFade("TitleScene"));
        GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo.Initialize();
    }
}
