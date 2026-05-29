using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class AllyBattleSetting : MonoBehaviour,IAllyBattleSetting
{
    private GameObject canvas;
    private List<GameObject> allyObjPrefab;
    private List <GameObject> allyInstance;
    private List<GameObject> allyInfoPrefab;
    private List <GameObject> allyInfoInstance;
    private string objname;
    private PlayerInfo playerinfo;
    private GameObject prefab;
    private List<GameObject> allyHpPrefab;
    private List<GameObject> allyHpInstance;
    private List<float> damagex;
    private List<float> damagey;
    BattleManager battleManager;
    List<GameObject> allyParserPrefab;
    List<GameObject> allyParserInstance;
    GameObject battleParser;
    public GameObject Canvas
    {
        get => canvas;
        set => canvas = value;
    }
    public List<GameObject> AllyObjPrefab
    {
        get => allyObjPrefab;
        set => allyObjPrefab = value;
    }
    public List<GameObject> AllyInstance
    {
        get => allyInstance;
        set => allyInstance = value;
    }
    public List<GameObject> AllyInfoPrefab
    {
        get => allyInfoPrefab;
        set => allyInfoPrefab = value;
    }
    public List<GameObject> AllyInfoInstance
    {
        get => allyInfoInstance;
        set => allyInfoInstance = value;
    }
    public string ObjName
    {
        get => objname;
        set => objname = value;
    }
    public PlayerInfo PlayerInfo => playerinfo;
    public GameObject Prefab
    {
        get => prefab;
        set => prefab = value;
    }
    public List<GameObject> AllyHpPrefab
    {
        get => allyHpPrefab;
        set => allyHpPrefab = value;
    }
    public List<GameObject> AllyHpInstance
    {
        get => allyHpInstance;
        set => allyHpInstance = value;
    }
    
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

    public List<GameObject> AllyParserPrefab
    {
        get => allyParserPrefab;
        set => allyParserPrefab = value;
    }
    public List<GameObject> AllyParserInstance
    {
        get => allyParserInstance;
        set => allyParserInstance = value;
    }
    public GameObject BattleParser
    {
        get => battleParser;
        set => battleParser = value;
    }
    public BattleManager BattleManager => battleManager;
    public void initialize()
    {
        allyInstance = new List<GameObject>();
        allyObjPrefab = new List<GameObject>();
        allyHpPrefab = new List<GameObject>();
        allyHpInstance = new List<GameObject>();
        allyInfoInstance = new List<GameObject>();
        allyInfoPrefab = new List<GameObject>();
        allyParserPrefab = new List<GameObject>();
        allyParserInstance = new List<GameObject>();
        damagex = new List<float>();
        damagey = new List<float>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        battleParser = GameObject.Find("Battle Parser");
        playerinfo = battleManager.playerInfo; 
        canvas = GameObject.Find("Main Battle UI");
    }
    public void SpawnAlly()
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        for(int i = 0; i < playerinfo.battleAnima.Count ; i++)
        {
            allyObjPrefab.Add(Resources.Load<GameObject>("Anima/" + playerinfo.battleAnima[i].Objectfile));
            allyHpPrefab.Add(Resources.Load<GameObject>("Minwoo/AllyAnimaHP"));
            allyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Ally{i}"));
            allyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Ally{i}Name"));
        }
        if (allyObjPrefab != null)
        {
            if (allyObjPrefab.Count == 3)
            {
                for (int i = 0; i < allyObjPrefab.Count; i++)
                {
                    allyInstance.Add(Instantiate(allyObjPrefab[i], new Vector3((i * 3.5f) - 3.5f, -2.2f, 0f), Quaternion.identity));
                    allyInstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                    int index = allyInstance[i].name.IndexOf("(Clone)");
                    allyInstance[i].name = allyInstance[i].name.Substring(0, index);
                    allyHpInstance.Add(Instantiate(allyHpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                    allyHpInstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i*380f) - 380f, -390f, 0f);
                    index = allyHpInstance[i].name.IndexOf("(Clone)");
                    allyHpInstance[i].name = allyHpInstance[i].name.Substring(0, index) + i;
                    allyInfoInstance.Add(Instantiate(allyInfoPrefab[i], canvas.transform));
                    index = allyInfoInstance[i].name.IndexOf("(Clone)");
                    allyInfoInstance[i].name = allyInfoInstance[i].name.Substring(0, index);
                    allyParserInstance.Add(Instantiate(allyParserPrefab[i], battleParser.transform));
                    index = allyParserInstance[i].name.IndexOf("(Clone)");
                    allyParserInstance[i].name = allyParserInstance[i].name.Substring(0, index);
                }
            }
            else if ( allyObjPrefab.Count == 2)
            {
                for (int i = 0; i < allyObjPrefab.Count; i++)
                {
                    allyInstance.Add(Instantiate(allyObjPrefab[i], new Vector3((i * 3.5f) - 1.75f, -2.2f, 0f), Quaternion.identity));
                    allyInstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                    int index = allyInstance[i].name.IndexOf("(Clone)");
                    allyInstance[i].name = allyInstance[i].name.Substring(0, index);
                    allyHpInstance.Add(Instantiate(allyHpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                    allyHpInstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 200f, -390f, 0f);
                    index = allyHpInstance[i].name.IndexOf("(Clone)");
                    allyHpInstance[i].name = allyHpInstance[i].name.Substring(0, index) + i;
                    allyInfoInstance.Add(Instantiate(allyInfoPrefab[i], canvas.transform));
                    index = allyInfoInstance[i].name.IndexOf("(Clone)");
                    allyInfoInstance[i].name = allyInfoInstance[i].name.Substring(0, index);
                    allyParserInstance.Add(Instantiate(allyParserPrefab[i], battleParser.transform));
                    index = allyParserInstance[i].name.IndexOf("(Clone)");
                    allyParserInstance[i].name = allyParserInstance[i].name.Substring(0, index);
                }
            }
            else
            {
                allyInstance.Add(Instantiate(allyObjPrefab[0], new Vector3(0f, -2.2f, 0), Quaternion.identity));
                allyInstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
                int index = allyInstance[0].name.IndexOf("(Clone)");
                allyInstance[0].name = allyInstance[0].name.Substring(0, index);
                allyInstance[0].transform.Rotate(0, 180f, 0);
                allyHpInstance.Add(Instantiate(allyHpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
                allyHpInstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -390f, 0f);
                index = allyHpInstance[0].name.IndexOf("(Clone)");
                allyHpInstance[0].name = allyHpInstance[0].name.Substring(0, index) + 0;
                allyInfoInstance.Add(Instantiate(allyInfoPrefab[0], canvas.transform));
                index = allyInfoInstance[0].name.IndexOf("(Clone)");
                allyInfoInstance[0].name = allyInfoInstance[0].name.Substring(0, index);
                allyParserInstance.Add(Instantiate(allyParserPrefab[0], battleParser.transform));
                index = allyParserInstance[0].name.IndexOf("(Clone)");
                allyParserInstance[0].name = allyParserInstance[0].name.Substring(0, index);
            }
        }

        }
       
    }
