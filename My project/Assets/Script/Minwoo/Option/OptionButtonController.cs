using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionButtonController : MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    Button applyBtn;
    
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }
    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    

    private void OnGUI()    //PreferenceData에 저장된 값 초기화 용도. 개발 도중 변경되어 이전 값에 의해 진행이 막힐때 용도.
    {
        if (GUI.Button(new Rect(10, 10, 200, 30), "디버깅용 저장값 초기화 버튼"))
            PlayerPrefs.DeleteAll();
    }

}
