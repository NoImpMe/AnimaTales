using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatusSync : MonoBehaviour
{
    IBattleManager battleManager;
    List<AnimaActions> battleAlly;
    List<EnemyActions> battleEnemy;
    string objname;
    public int idx = -1;
    public int dieanima = 0;
    StringBuilder allyBuff = new StringBuilder();
    StringBuilder enemyBuff = new StringBuilder();
    StringBuilder allyDebuff = new StringBuilder();
    StringBuilder enemyDebuff = new StringBuilder();
    void Awake()
    {
        objname = this.transform.parent.name;
        battleManager = GameObject.Find("BattleManager").GetComponent<IBattleManager>();
        idx = int.Parse(objname.Substring(objname.Length - 1, 1) + "");
        var status = GameObject.Find(objname);
    }

    private void FixedUpdate()
    {
        if(this != null)
        {
            this.enabled = false;
            this.enabled = true;
        }
    }
    void OnEnable()
    {
        if(idx != 0)
        {
            idx = int.Parse(objname.Substring(objname.Length - 1, 1) + "") - dieanima;
        }
        if (objname.StartsWith("A"))
        {
            battleAlly = battleManager.AllyActions;
            this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.Name;
            this.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Lv. " + battleAlly[idx].animaData.level.ToString();
            this.transform.Find("Exp").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.EXP.ToString();
            this.transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(battleAlly[idx].animaData.Stamina) + " / " + battleAlly[idx].animaData.Maxstamina.ToString();            
            for(int i = 0; i < battleManager.BuffManager.GetBuffList().Count; i++)
            {
                if (ReferenceEquals(battleManager.BuffManager.GetBuffList()[i].target, battleAlly[idx].animaData))
                {
                    if (battleManager.BuffManager.GetBuffList()[i].distinct == 0)
                    {
                        allyBuff.AppendLine(string.Join(", ", battleManager.BuffManager.GetBuffList()[i].type));
                    }
                    else
                    {
                        allyDebuff.AppendLine(string.Join(", ", battleManager.BuffManager.GetBuffList()[i].type));
                    }
                }
            }
            this.transform.Find("Buff").GetComponent<TextMeshProUGUI>().text = allyBuff.ToString();
            this.transform.Find("Debuff").GetComponent<TextMeshProUGUI>().text = allyDebuff.ToString();
            allyBuff.Clear();
            allyDebuff.Clear();
        }
        else
        {
            battleEnemy = battleManager.EnemyActions;
            this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.Name;
            this.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Lv. " + battleEnemy[idx].animaData.level.ToString();
            this.transform.Find("Exp").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.EXP.ToString();
            this.transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(battleEnemy[idx].animaData.Stamina) + " / " + battleEnemy[idx].animaData.Maxstamina.ToString();
            for (int i = 0; i < battleManager.BuffManager.GetBuffList().Count; i++)
            {
                if (ReferenceEquals(battleManager.BuffManager.GetBuffList()[i].target, battleEnemy[idx].animaData))
                {
                    if (battleManager.BuffManager.GetBuffList()[i].distinct == 0)
                    {
                        enemyBuff.AppendLine(string.Join(", ", battleManager.BuffManager.GetBuffList()[i].type));
                    }
                    else
                    {
                        enemyDebuff.AppendLine(string.Join(", ", battleManager.BuffManager.GetBuffList()[i].type));
                    }
                }
            }
            this.transform.Find("Buff").GetComponent<TextMeshProUGUI>().text = enemyBuff.ToString();
            this.transform.Find("Debuff").GetComponent<TextMeshProUGUI>().text = enemyDebuff.ToString();
            enemyBuff.Clear();
            enemyDebuff.Clear();
        }
    }
}
