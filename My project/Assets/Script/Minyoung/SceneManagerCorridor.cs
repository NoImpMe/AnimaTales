using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerCorridor : MonoBehaviour
{
    public string sceneName;
    public string tileSceneName;

    void Awake()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        if (scene.name.Contains("Stage"))
        {
            tileSceneName = scene.name;
        }
        sceneName = scene.name;
    }
}
