using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerInBattle : MonoBehaviour
{
    private GameObject regionManager;
    public void backToTiles()
    {
        regionManager = GameObject.Find("RegionManager");
        var regionScr = regionManager.GetComponent<RegionManager>();
        var num = regionScr.stageType;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.EndsWith("LastBossScene")){
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            switch (num)
            {
                case 0:
                    SceneManager.LoadScene("Stage0Scene");
                    break;
                case 1:
                    SceneManager.LoadScene("Stage1Scene");
                    break;
                case 2:
                    SceneManager.LoadScene("Stage2Scene");
                    break;
                case 3:
                    SceneManager.LoadScene("Stage3Scene");
                    break;
                case 4:
                    SceneManager.LoadScene("Stage4Scene");
                    break;
                case 5:
                    SceneManager.LoadScene("Stage5Scene");
                    break;
                case 6:
                    SceneManager.LoadScene("Stage6Scene");
                    break;
            }
        }
    }

    public void resetGame()
    {
        SceneManager.LoadScene("TitleScene");
        GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo.Initialize();
    }
}
