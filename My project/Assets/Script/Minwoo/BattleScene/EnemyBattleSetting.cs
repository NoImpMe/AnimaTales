using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class EnemyBattleSetting : MonoBehaviour, IEnemyBattleSetting
{
    private List<float> damagex;
    private List<float> damagey;
    private List<GameObject> enemyObjPrefab;
    private List<GameObject> enemyInstance;
    private string objname;
    private List<string> objectfileList;
    private List<string> battleEnemyAnima;
    private List<GameObject> enemyHpPrefab;
    private List<GameObject> enemyHpInstance;
    private List<GameObject> enemyInfoPrefab;
    private List<GameObject> enemyInfoInstance;
    List<GameObject> enemyParserPrefab;
    List<GameObject> enemyParserInstance;
    private GameObject canvas;
    GameObject battleParser;
    public bool isElite = false;
    public bool isBoss = false;
    BattleManager battleManager;
    public BattleManager BattleManager => battleManager;
    private string stage;
    public List<float> DamageX
    {
        get => damagex;
        set => damagex = value;
    }
    public List<float> DamageY
    {
        get => damagey;
        set => damagey = value;
    }
    public List<GameObject> EnemyObjPrefab
    {
        get => enemyObjPrefab;
        set => enemyObjPrefab = value;
    }
    public List<GameObject> EnemyInstance
    {
        get => enemyInstance;
        set => enemyInstance = value;
    }
    public string ObjName
    {
        get => objname;
        set => objname = value;
    }
    public List<string> ObjectFileList
    {
        get => objectfileList;
        set => objectfileList = value;
    }
    public List<string> BattleEnemyAnima
    {
        get => battleEnemyAnima;
        set => battleEnemyAnima = value;
    }
    public List<GameObject> EnemyHpPrefab
    {
        get => enemyHpPrefab;
        set => enemyHpPrefab = value;
    }
    public List<GameObject> EnemyHpInstance
    {
        get => enemyHpInstance;
        set => enemyHpInstance = value;
    }
    public List<GameObject> EnemyInfoPrefab
    {
        get => enemyInfoPrefab;
        set => enemyInfoPrefab = value;
    }
    public List<GameObject> EnemyInfoInstance
    {
        get => enemyInfoInstance;
        set => enemyInfoInstance = value;
    }
    public List<GameObject> EnemyParserPrefab
    {
        get => enemyParserPrefab;
        set => enemyParserPrefab = value;
    }
    public List<GameObject> EnemyParserInstance
    {
        get => enemyParserInstance;
        set => enemyParserInstance = value;
    }
    public GameObject Canvas
    {
        get => canvas;
        set => canvas = value;
    }
    public GameObject BattleParser
    {
        get => battleParser;
        set => battleParser = value;
    }
    public string Stage
    {
        get => stage;
        set => stage = value;
    }
    public void SpawnEnemy(int level)
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("Main Battle UI");
        if (objectfileList != null)
        {
            objectfileList.Clear();
            enemyObjPrefab.Clear();
            enemyInstance.Clear();
            enemyHpPrefab.Clear();
            enemyHpInstance.Clear();
            damagex.Clear();
            damagey.Clear();
            battleEnemyAnima.Clear();
            enemyObjPrefab.Clear();
        }
        else
        {
            objectfileList = new List<string>();
            enemyObjPrefab = new List<GameObject>();
            enemyInstance = new List<GameObject>();
            enemyHpPrefab = new List<GameObject>();
            enemyHpInstance = new List<GameObject>();
            enemyInfoInstance = new List<GameObject>();
            enemyInfoPrefab = new List<GameObject>();
            enemyParserPrefab = new List<GameObject>();
            enemyParserInstance = new List<GameObject>();
            damagex = new List<float>();
            damagey = new List<float>();
            battleEnemyAnima = new List<string>();
            enemyObjPrefab = new List<GameObject>();
            battleParser = GameObject.Find("Battle Parser");
        }
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        animaTable.ForEachEntity(entity =>
        {
            if (entity.Get<string>("Type") == stage)
                objectfileList.Add(entity.Get<string>("Objectfile"));
        });
        int numberOfObjectsToAdd = Random.Range(1, 4);
            int mood = 0;
            if (level < 7)
            {
                mood = 2;
            }
            else if (level < 11)
            {
                mood = 3;
            }
            else if (level < 16)
            {
                mood = 4;
            }
            else if (level <= 20)
            {
                mood = 5;
            }
            else
            {
                mood = 6;
            }
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                int randomIndex = Random.Range(1, mood + 2);
                enemyObjPrefab.Add(Resources.Load<GameObject>("Anima/" + objectfileList[randomIndex]));
                enemyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
                enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
                enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
                battleEnemyAnima.Add(objectfileList[randomIndex]);
            }
            if (enemyObjPrefab.Count == 3 && enemyObjPrefab != null && enemyHpPrefab != null)
            {
                for (int i = 0; i < enemyObjPrefab.Count; i++)
                {
                    enemyInstance.Add(Instantiate(enemyObjPrefab[i], new Vector3((i * 3.5f) - 3.5f, 1.2f, 0), Quaternion.identity));
                    enemyInstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                    int index = enemyInstance[i].name.IndexOf("(Clone)");
                    enemyInstance[i].name = enemyInstance[i].name.Substring(0, index) + (i + 3);
                    enemyInstance[i].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    enemyHpInstance.Add(Instantiate(enemyHpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                    enemyHpInstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 380f, -22f, 0f);
                    enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[i], canvas.transform));
                    index = enemyHpInstance[i].name.IndexOf("(Clone)");
                    enemyHpInstance[i].name = enemyHpInstance[i].name.Substring(0, index) + i;
                    index = enemyInfoInstance[i].name.IndexOf("(Clone)");
                    enemyInfoInstance[i].name = enemyInfoInstance[i].name.Substring(0, index);
                    enemyParserInstance.Add(Instantiate(enemyParserPrefab[i], battleParser.transform));
                    index = enemyParserInstance[i].name.IndexOf("(Clone)");
                    enemyParserInstance[i].name = enemyParserInstance[i].name.Substring(0, index);
                }

            }
            if (enemyObjPrefab.Count == 2 && enemyObjPrefab != null && enemyHpPrefab != null)
            {
                for (int i = 0; i < enemyObjPrefab.Count; i++)
                {
                    enemyInstance.Add(Instantiate(enemyObjPrefab[i], new Vector3((i * 3.5f) - 1.75f, 1.2f, 0), Quaternion.identity));
                    enemyInstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                    int index = enemyInstance[i].name.IndexOf("(Clone)");
                    enemyInstance[i].name = enemyInstance[i].name.Substring(0, index) + (i + 3);
                    enemyInstance[i].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    enemyHpInstance.Add(Instantiate(enemyHpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                    enemyHpInstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 200f, -22f, 0f);
                    enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[i], canvas.transform));
                    index = enemyHpInstance[i].name.IndexOf("(Clone)");
                    enemyHpInstance[i].name = enemyHpInstance[i].name.Substring(0, index) + i;
                    index = enemyInfoInstance[i].name.IndexOf("(Clone)");
                    enemyInfoInstance[i].name = enemyInfoInstance[i].name.Substring(0, index);
                    enemyParserInstance.Add(Instantiate(enemyParserPrefab[i], battleParser.transform));
                    index = enemyParserInstance[i].name.IndexOf("(Clone)");
                    enemyParserInstance[i].name = enemyParserInstance[i].name.Substring(0, index);
                }

            }
            else if (enemyObjPrefab.Count == 1 && enemyObjPrefab != null && enemyHpPrefab != null)
            {
                enemyInstance.Add(Instantiate(enemyObjPrefab[0], new Vector3(0f, 1.2f, 0), Quaternion.identity));
                enemyInstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
                int index = enemyInstance[0].name.IndexOf("(Clone)");
                enemyInstance[0].name = enemyInstance[0].name.Substring(0, index) + 3;
                enemyInstance[0].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);//-195f 185f 
                enemyHpInstance.Add(Instantiate(enemyHpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
                enemyHpInstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -22f, 0f);
                enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[0], canvas.transform));
                index = enemyHpInstance[0].name.IndexOf("(Clone)");
                enemyHpInstance[0].name = enemyHpInstance[0].name.Substring(0, index) + 0;
                index = enemyInfoInstance[0].name.IndexOf("(Clone)");
                enemyInfoInstance[0].name = enemyInfoInstance[0].name.Substring(0, index);
                enemyParserInstance.Add(Instantiate(enemyParserPrefab[0], battleParser.transform));
                index = enemyParserInstance[0].name.IndexOf("(Clone)");
                enemyParserInstance[0].name = enemyParserInstance[0].name.Substring(0, index);
            }
        
    }
    public void SpawnElite(int level)
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("Main Battle UI");
        if (objectfileList != null)
        {
            objectfileList.Clear();
            enemyObjPrefab.Clear();
            enemyInstance.Clear();
            enemyHpPrefab.Clear();
            enemyHpInstance.Clear();
            damagex.Clear();
            damagey.Clear();
            battleEnemyAnima.Clear();
            enemyObjPrefab.Clear();
        }
        else
        {
            objectfileList = new List<string>();
            enemyObjPrefab = new List<GameObject>();
            enemyInstance = new List<GameObject>();
            enemyHpPrefab = new List<GameObject>();
            enemyHpInstance = new List<GameObject>();
            enemyInfoInstance = new List<GameObject>();
            enemyInfoPrefab = new List<GameObject>();
            enemyParserPrefab = new List<GameObject>();
            enemyParserInstance = new List<GameObject>();
            damagex = new List<float>();
            damagey = new List<float>();
            battleEnemyAnima = new List<string>();
            enemyObjPrefab = new List<GameObject>();
            battleParser = GameObject.Find("Battle Parser");
        }
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        animaTable.ForEachEntity(entity =>
        {
            if (entity.Get<string>("Type") == stage)
                objectfileList.Add(entity.Get<string>("Objectfile"));
        });
        int mood = 0;
        if (level < 7)
        {
            mood = 3;
        }
        else if (level < 11)
        {
            mood = 4;
        }
        else if (level < 16)
        {
            mood = 5;
        }
        else if (level < 20)
        {
            mood = 6;
        }
        else
        {
            mood = 7;
        }
        
        for (int i = 0; i < 1; i++)
        {
            int randomIndex = Random.Range(mood, mood + 1);
            enemyObjPrefab.Add(Resources.Load<GameObject>("Anima/" + objectfileList[randomIndex]));
            enemyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
            enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
            battleEnemyAnima.Add(objectfileList[randomIndex]);
        }


        if (enemyObjPrefab.Count == 1 && enemyObjPrefab != null && enemyHpPrefab != null)
        {
            enemyInstance.Add(Instantiate(enemyObjPrefab[0], new Vector3(0f, 1.2f, 0), Quaternion.identity));
            enemyInstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
            int index = enemyInstance[0].name.IndexOf("(Clone)");
            enemyInstance[0].name = enemyInstance[0].name.Substring(0, index) + 3;
            enemyInstance[0].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            enemyHpInstance.Add(Instantiate(enemyHpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
            enemyHpInstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -22f, 0f);
            enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[0], canvas.transform));
            index = enemyHpInstance[0].name.IndexOf("(Clone)");
            enemyHpInstance[0].name = enemyHpInstance[0].name.Substring(0, index) + 0;
            index = enemyInfoInstance[0].name.IndexOf("(Clone)");
            enemyInfoInstance[0].name = enemyInfoInstance[0].name.Substring(0, index);
            enemyParserInstance.Add(Instantiate(enemyParserPrefab[0], battleParser.transform));
            index = enemyParserInstance[0].name.IndexOf("(Clone)");
            enemyParserInstance[0].name = enemyParserInstance[0].name.Substring(0, index);
        }
    }
    public void SpawnBoss()
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("Main Battle UI");
        if (objectfileList != null)
        {
            objectfileList.Clear();
            enemyObjPrefab.Clear();
            enemyInstance.Clear();
            enemyHpPrefab.Clear();
            enemyHpInstance.Clear();
            damagex.Clear();
            damagey.Clear();
            battleEnemyAnima.Clear();
            enemyObjPrefab.Clear();
        }
        else
        {
            objectfileList = new List<string>();
            enemyObjPrefab = new List<GameObject>();
            enemyInstance = new List<GameObject>();
            enemyHpPrefab = new List<GameObject>();
            enemyHpInstance = new List<GameObject>();
            enemyInfoInstance = new List<GameObject>();
            enemyInfoPrefab = new List<GameObject>();
            enemyParserPrefab = new List<GameObject>();
            enemyParserInstance = new List<GameObject>();
            damagex = new List<float>();
            damagey = new List<float>();
            battleEnemyAnima = new List<string>();
            enemyObjPrefab = new List<GameObject>();
            battleParser = GameObject.Find("Battle Parser");
        }
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        animaTable.ForEachEntity(entity =>
        {
            
            if (entity.Get<string>("Type") == stage && entity.Get<int>("IsBoss") == 1)
            {
                objectfileList.Add(entity.Get<string>("Objectfile"));
            }
        });
        if(stage == "Lacrima")
        {
            animaTable.ForEachEntity(entity =>
            {

                if (entity.Get<string>("name") == "tombstone0")
                {
                    objectfileList.Add(entity.Get<string>("Objectfile"));
                }
            });
        }
        for (int i = 0; i < objectfileList.Count; i++)
        {
            
            enemyObjPrefab.Add(Resources.Load<GameObject>("Anima/" + objectfileList[i]));
            enemyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
            enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
            battleEnemyAnima.Add(objectfileList[i]);
        }

        if (enemyObjPrefab.Count == 2)
        {
            for (int i = 0; i < enemyObjPrefab.Count; i++)
            {
                enemyInstance.Add(Instantiate(enemyObjPrefab[i], new Vector3((i * 5.3f), 1.5f, 0), Quaternion.identity));
                enemyInstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                int index = enemyInstance[i].name.IndexOf("(Clone)");
                enemyInstance[i].name = enemyInstance[i].name.Substring(0, index) + (i + 3);
                enemyInstance[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                enemyHpInstance.Add(Instantiate(enemyHpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                enemyHpInstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f)+ (200f * i), -22f, 0f);
                enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[i], canvas.transform));
                index = enemyHpInstance[i].name.IndexOf("(Clone)");
                enemyHpInstance[i].name = enemyHpInstance[i].name.Substring(0, index) + i;
                index = enemyInfoInstance[i].name.IndexOf("(Clone)");
                enemyInfoInstance[i].name = enemyInfoInstance[i].name.Substring(0, index);
                enemyParserInstance.Add(Instantiate(enemyParserPrefab[i], battleParser.transform));
                index = enemyParserInstance[i].name.IndexOf("(Clone)");
                enemyParserInstance[i].name = enemyParserInstance[i].name.Substring(0, index);
            }
        }
            
        if (enemyObjPrefab.Count == 1 )
        {
            enemyInstance.Add(Instantiate(enemyObjPrefab[0], new Vector3(0f, 1.5f, 0), Quaternion.identity));
            enemyInstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
            int index = enemyInstance[0].name.IndexOf("(Clone)");
            enemyInstance[0].name = enemyInstance[0].name.Substring(0, index) + 3;
            enemyInstance[0].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            enemyHpInstance.Add(Instantiate(enemyHpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
            enemyHpInstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -22f, 0f);
            enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[0], canvas.transform));
            index = enemyHpInstance[0].name.IndexOf("(Clone)");
            enemyHpInstance[0].name = enemyHpInstance[0].name.Substring(0, index) + 0;
            index = enemyInfoInstance[0].name.IndexOf("(Clone)");
            enemyInfoInstance[0].name = enemyInfoInstance[0].name.Substring(0, index);
            enemyParserInstance.Add(Instantiate(enemyParserPrefab[0], battleParser.transform));
            index = enemyParserInstance[0].name.IndexOf("(Clone)");
            enemyParserInstance[0].name = enemyParserInstance[0].name.Substring(0, index);
        }
    }
    public void AmareSpawn(string charmedAnima)
    {
        enemyObjPrefab.Add(Resources.Load<GameObject>("Anima/" + charmedAnima));
        enemyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
        enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy1"));
        enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy1Name"));
        battleEnemyAnima.Add(charmedAnima);
        enemyInstance.Add(Instantiate(enemyObjPrefab[1], new Vector3(5.3f, 1.5f, 0), Quaternion.identity));
        enemyInstance[1].GetComponent<SpriteRenderer>().sortingOrder = -1;
        int index = enemyInstance[1].name.IndexOf("(Clone)");
        enemyInstance[1].name = enemyInstance[1].name.Substring(0, index) + "4";
        enemyInstance[1].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        enemyHpInstance.Add(Instantiate(enemyHpPrefab[1], Vector3.zero, Quaternion.identity, canvas.transform));
        enemyHpInstance[1].GetComponent<RectTransform>().anchoredPosition = new Vector3((380f) + (200f), -22f, 0f);
        enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[1], canvas.transform));
        index = enemyHpInstance[1].name.IndexOf("(Clone)");
        enemyHpInstance[1].name = enemyHpInstance[1].name.Substring(0, index) + "1";
        index = enemyInfoInstance[1].name.IndexOf("(Clone)");
        enemyInfoInstance[1].name = enemyInfoInstance[1].name.Substring(0, index);
        enemyParserInstance.Add(Instantiate(enemyParserPrefab[1], battleParser.transform));
        index = enemyParserInstance[1].name.IndexOf("(Clone)");
        enemyParserInstance[1].name = enemyParserInstance[1].name.Substring(0, index);
    }
}
