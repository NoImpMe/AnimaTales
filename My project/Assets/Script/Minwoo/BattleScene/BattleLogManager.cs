using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleLogManager : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform content;
    public GameObject allyDamageLog;
    public GameObject enemyDamageLog;

    public void AddLog(string message, bool isAlly)
    {
        GameObject entry = Instantiate(isAlly ? allyDamageLog : enemyDamageLog, content);

        entry.GetComponent<TextMeshProUGUI>().text = message;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
    

}
