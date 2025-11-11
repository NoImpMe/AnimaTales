using System.Collections.Generic;
using UnityEngine;

public class BackToScene : MonoBehaviour
{

    private GameObject gameManager;
    private List<string> tileType = new List<string> { "Amare", "Felix", "Havet", "Irascor", "Lacrima", "Phobia" };
    [SerializeField]
    private FadeEffect fadePanel;
    [SerializeField]
    private AudioClip btnClip;

    public void backToScenes()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        gameManager = GameObject.Find("Game Manager");
        var gameM = gameManager.GetComponent<SceneManagerCorridor>();
        StartCoroutine(fadePanel.LoadSceneWithFade(gameM.sceneName));
    }
    public void BackToTiles()
    {
        AudioManager.Instance.PlaySFX(btnClip);
        gameManager = GameObject.Find("Game Manager");
        var gameM = gameManager.GetComponent<SceneManagerCorridor>();   
        StartCoroutine(fadePanel.LoadSceneWithFade(gameM.tileSceneName));
    }
}
