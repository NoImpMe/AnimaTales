//using System.Collections;
//using System.Collections.Generic;
//using BansheeGz.BGDatabase;
//using UnityEngine;

//public class EliteEnemyBattleSetting : MonoBehaviour, IEnemyBattleSetting 
//{
//    public List<float> damagex;
//    public List<float> damagey;
//    public List<GameObject> enemyObjPrefab;
//    public List<GameObject> enemyInstance;
//    public string objname;
//    private List<string> objectfileList;
//    public List<string> battleEnemyAnima;
//    public List<Animator> animator;
//    public List<GameObject> enemyHpPrefab;
//    public List<GameObject> enemyHpInstance;
//    public List<GameObject> enemyInfoPrefab;
//    public List<GameObject> enemyInfoInstance;
//    List<GameObject> enemyParserPrefab;
//    List<GameObject> enemyParserInstance;
//    public GameObject canvas;
//    GameObject battleParser;
//    public string stage;
//    EliteBattleManager eliteBattleManager;
//    BattleManager battleManager;
//    public BattleManager BattleManager => battleManager;

//    public EliteBattleManager EliteBattleManager
//    {
//        get => eliteBattleManager;
//    }
//    public List<float> DamageX 
//    {
//        get => damagex;
//        set => damagex = value;
//    }
//    public List<float> DamageY
//    {
//        get => damagey;
//        set => damagey = value;
//    }
//    public List<GameObject> EnemyObjPrefab
//    {
//        get => enemyObjPrefab;
//        set => enemyObjPrefab = value;

//    }
//    public List<GameObject> EnemyInstance
//    {
//        get => enemyInstance;
//        set => enemyInstance = value;
//    }
//    public string ObjName
//    {
//        get => objname;
//        set => objname = value;
//    }
//    public List<string> ObjectFileList
//    {
//        get => objectfileList;
//        set => objectfileList = value;
//    }
//    public List<string> BattleEnemyAnima
//    {
//        get => battleEnemyAnima;
//        set => battleEnemyAnima = value;
//    }
//    public List<Animator> AnimatorList {get => animator; }
//    public List<GameObject> EnemyHpPrefab
//    {
//        get => enemyHpPrefab;
//        set => enemyHpPrefab = value;
//    }
//    public List<GameObject> EnemyHpInstance
//    {
//        get => enemyHpInstance;
//        set => enemyHpInstance = value;
//    }
//    public List<GameObject> EnemyInfoPrefab
//    {
//        get => enemyInfoPrefab;
//        set => enemyInfoPrefab = value;
//    }
//    public List<GameObject> EnemyInfoInstance
//    {
//        get => enemyInfoInstance;
//        set => enemyInfoInstance = value;
//    }
//    public List<GameObject> EnemyParserPrefab
//    {
//        get => enemyParserPrefab;
//        set => enemyParserPrefab = value;
//    }
//    public List<GameObject> EnemyParserInstance
//    {
//        get => enemyParserInstance;
//        set => enemyParserInstance = value;
//    }
//    public GameObject Canvas
//    {
//        get => canvas;
//        set => canvas = value;
//    }
//    public GameObject BattleParser
//    {
//        get => battleParser;
//        set => battleParser = value;
//    }
//    public string Stage
//    {
//        get => stage;
//        set => stage = value;
//    }
//    public void SpawnEnemy(int level)
//    {
//        animator = new List<Animator>();
//        var database = BGRepo.I;
//        var animaTable = database.GetMeta("Anima");
//        canvas = GameObject.Find("Main Battle UI");
//        if (objectfileList != null)
//        {
//            objectfileList.Clear();
//            enemyObjPrefab.Clear();
//            enemyInstance.Clear();
//            enemyHpPrefab.Clear();
//            enemyHpInstance.Clear();
//            damagex.Clear();
//            damagey.Clear();
//            battleEnemyAnima.Clear();
//            enemyObjPrefab.Clear();
//        }
//        else
//        {
//            objectfileList = new List<string>();
//            enemyObjPrefab = new List<GameObject>();
//            enemyInstance = new List<GameObject>();
//            enemyHpPrefab = new List<GameObject>();
//            enemyHpInstance = new List<GameObject>();
//            enemyInfoInstance = new List<GameObject>();
//            enemyInfoPrefab = new List<GameObject>();
//            enemyParserPrefab = new List<GameObject>();
//            enemyParserInstance = new List<GameObject>();
//            damagex = new List<float>();
//            damagey = new List<float>();
//            battleEnemyAnima = new List<string>();
//            enemyObjPrefab = new List<GameObject>();
//            battleParser = GameObject.Find("Battle Parser");
//        }
//        eliteBattleManager = GameObject.Find("BattleManager").GetComponent<EliteBattleManager>();

//        animaTable.ForEachEntity(entity =>
//        {
//            if (entity.Get<string>("Type") == stage)
//            {
//                objectfileList.Add(entity.Get<string>("Objectfile"));
                
//            }
            
//        });
//        int mood = 0;
//        if (level <= 8)
//        {
//            mood = 3;
//        }
//        else if (level <= 12)
//        {
//            mood = 4;
//        }
//        else if (level <= 16)
//        {
//            mood = 5;
//        }
//        else if (level <= 20)
//        {
//            mood = 6;
//        }
//        else
//        {
//            mood = 7;
//        }
//        int numberOfObjectsToAdd = Random.Range(1, 2);
//        for (int i = 0; i < numberOfObjectsToAdd; i++)
//        {
//            int randomIndex = Random.Range(mood, mood+1);
//            enemyObjPrefab.Add(Resources.Load<GameObject>("Minwoo/Portrait/" + objectfileList[randomIndex]));
//            enemyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
//            enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
//            enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
//            battleEnemyAnima.Add(objectfileList[randomIndex]);
//        }


//        if (enemyObjPrefab.Count == 1 && enemyObjPrefab != null && enemyHpPrefab != null)
//        {
//            enemyInstance.Add(Instantiate(enemyObjPrefab[0], new Vector3(0f, 1.2f, 0), Quaternion.identity));
//            enemyInstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
//            int index = enemyInstance[0].name.IndexOf("(Clone)");
//            enemyInstance[0].name = enemyInstance[0].name.Substring(0, index) + 3;
//            enemyInstance[0].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
//            enemyHpInstance.Add(Instantiate(enemyHpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
//            enemyHpInstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -22f, 0f);
//            enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[0], canvas.transform));
//            index = enemyHpInstance[0].name.IndexOf("(Clone)");
//            enemyHpInstance[0].name = enemyHpInstance[0].name.Substring(0, index) + 0;
//            index = enemyInfoInstance[0].name.IndexOf("(Clone)");
//            enemyInfoInstance[0].name = enemyInfoInstance[0].name.Substring(0, index);
//            enemyParserInstance.Add(Instantiate(enemyParserPrefab[0], battleParser.transform));
//            index = enemyParserInstance[0].name.IndexOf("(Clone)");
//            enemyParserInstance[0].name = enemyParserInstance[0].name.Substring(0, index);
//            animator.Add(enemyInstance[0].GetComponent<Animator>());


//        }


//    }
    
//}
