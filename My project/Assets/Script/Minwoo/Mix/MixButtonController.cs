using UnityEngine;

public class MixButtonController : MonoBehaviour
{
    [SerializeField]
    GameObject mixPanel;
    [SerializeField]
    GameObject exitButton;
    [SerializeField]
    MixManager mixManager;
    [SerializeField]
    GameObject resultCanvas;
    public void ExitPanel()
    {
        mixManager.Revert();
        mixPanel.SetActive(false);
        exitButton.SetActive(true);
    }
    public void EnterPanel()
    {
        mixPanel.SetActive(true);
        mixManager.Init();
        exitButton.SetActive(false);
    }
    public void ExitMix()
    {
        resultCanvas.SetActive(false);
    }
}
