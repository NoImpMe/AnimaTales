//using System.Collections.Generic;
//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;

//public class EliteStatusSync : MonoBehaviour
//{
//    EliteBattleManager battleManager;
//    List<AnimaActions> battleAlly;
//    List<EnemyActions> battleEnemy;
//    string objname;
//    public int idx = -1;
//    public int dieanima = 0;
//    void Awake()
//    {
//        objname = this.transform.parent.name;
//        battleManager = GameObject.Find("BattleManager").GetComponent<EliteBattleManager>();
//        idx = int.Parse(objname.Substring(objname.Length - 1, 1) + "");
//        var status = GameObject.Find(objname);
//    }

//    private void Update()
//    {
//        if (this != null)
//        {
//            this.enabled = false;
//            this.enabled = true;
//        }

//    }
//    // Update is called once per frame
//    void OnEnable()
//    {
//        if (idx != 0)
//        {
//            idx = int.Parse(objname.Substring(objname.Length - 1, 1) + "") - dieanima;
//        }
//        if (objname.StartsWith("A"))
//        {
//            battleAlly = battleManager.GetAllyActions();
//            this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.Name;
//            this.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Lv. " + battleAlly[idx].animaData.level.ToString();
//            this.transform.Find("Exp").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.EXP.ToString();
//            this.transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(battleAlly[idx].animaData.Stamina) + " / " + battleAlly[idx].animaData.Maxstamina.ToString();
//        }
//        else
//        {
//            battleEnemy = battleManager.GetEnemyActions();
//            this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.Name;
//            this.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Lv. " + battleEnemy[idx].animaData.level.ToString();
//            this.transform.Find("Exp").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.EXP.ToString();
//            this.transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(battleEnemy[idx].animaData.Stamina) + " / " + battleEnemy[idx].animaData.Maxstamina.ToString();
//        }
//    }
//}
