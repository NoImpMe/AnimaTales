using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToScene : MonoBehaviour
{

    private GameObject gameManager;
    private List<string> tileType = new List<string> { "Amare", "Felix", "Havet", "Irascor", "Lacrima", "Phobia" };
    [SerializeField]
    private FadeEffect fadePanel;

    public void backToScenes()
    {
        gameManager = GameObject.Find("Game Manager");
        var gameM = gameManager.GetComponent<SceneManagerCorridor>();
        StartCoroutine(fadePanel.LoadSceneWithFade(gameM.sceneName));
    }
    public void BackToTiles()
    {
        gameManager = GameObject.Find("Game Manager");
        var gameM = gameManager.GetComponent<SceneManagerCorridor>();   
        StartCoroutine(LoadTileSceneWithFade(gameM.tileSceneName));
    }
    private IEnumerator LoadTileSceneWithFade(string sceneName)
    {
        if (fadePanel != null)
            yield return fadePanel.FadeIn();
        for (int i = 0; i < 6; i++)
        {
            GameObject.Find("Tiles").transform.Find($"{tileType[i]}").gameObject.SetActive(true);
        }
        SceneManager.LoadScene(sceneName);
    }
}
