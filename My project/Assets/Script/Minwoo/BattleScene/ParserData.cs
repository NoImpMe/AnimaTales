using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParserData : MonoBehaviour
{
    bool parsercheck = false;
    [SerializeField]
    GameObject battleParser;
    [SerializeField]
    GameObject battleLog;
    [SerializeField]
    GameObject maxButton;
    [SerializeField]
    GameObject minButton;
    [SerializeField]
    Button damageButton;
    [SerializeField]
    Button healButton;
    List<GameObject> allyDamage = new List<GameObject>();
    List<GameObject> allyHeal = new List<GameObject>();
    List<GameObject> enemyDamage= new List<GameObject>();
    List<GameObject> enemyHeal = new List<GameObject>();
    
    public void OnClickParser()
    {
        if (!parsercheck)
        {
            battleParser.GetComponent<CanvasGroup>().alpha = 1;
            battleParser.GetComponent<CanvasGroup>().interactable = true;
            parsercheck = true;
            for (int i = 0; i < 3; i++)
            {
                if (GameObject.Find($"Ally{i}Name") != null)
                {
                    allyDamage.Add(GameObject.Find($"A{i}Damage"));
                    allyHeal.Add(GameObject.Find($"A{i}Heal"));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (battleParser.transform.Find($"Enemy{i}Name") != null)
                {
                    enemyDamage.Add(GameObject.Find($"E{i}Damage"));
                    enemyHeal.Add(GameObject.Find($"E{i}Heal"));
                }
            }
        }
        else
        {
            battleParser.GetComponent<CanvasGroup>().alpha = 0;
            battleParser.GetComponent<CanvasGroup>().interactable = false;
            parsercheck = false;
        }
    }
    public void DamageButton()
    {
        damageButton.interactable = false;
        healButton.interactable = true;
        for (int i = 0; i < allyDamage.Count; i++)
        {
            allyDamage[i].GetComponent<CanvasGroup>().alpha = 1;
        }
        for (int i = 0; i < enemyDamage.Count; i++)
        {
            enemyDamage[i].GetComponent<CanvasGroup>().alpha = 1;
        }
        for (int i = 0; i < allyHeal.Count; i++)
        {
            allyHeal[i].GetComponent<CanvasGroup>().alpha = 0;
        }
        for (int i = 0; i < enemyHeal.Count; i++)
        {
            enemyHeal[i].GetComponent<CanvasGroup>().alpha = 0;
        }

    }
    public void HealButton()
    {
        healButton.interactable = false;
        damageButton.interactable = true;
        for (int i = 0; i < allyDamage.Count; i++)
        {
            allyDamage[i].GetComponent<CanvasGroup>().alpha = 0;
        }
        for (int i = 0; i < enemyDamage.Count; i++)
        {
            enemyDamage[i].GetComponent<CanvasGroup>().alpha = 0;
        }
        for (int i = 0; i < allyHeal.Count; i++)
        {
            allyHeal[i].GetComponent<CanvasGroup>().alpha = 1;
        }
        for (int i = 0; i < enemyHeal.Count; i++)
        {
            enemyHeal[i].GetComponent<CanvasGroup>().alpha = 1;
        }
    }
    public void OpenLog()
    {
        battleLog.GetComponent<CanvasGroup>().alpha = 1;
        battleLog.GetComponent <CanvasGroup>().interactable = true;
        maxButton.SetActive(false);
        minButton.SetActive(true);
    }
    public void CloseLog()
    {
        battleLog.GetComponent<CanvasGroup>().alpha = 0;
        battleLog.GetComponent<CanvasGroup>().interactable = false;
        minButton.SetActive(false);
        maxButton.SetActive(true) ;
    }
}
