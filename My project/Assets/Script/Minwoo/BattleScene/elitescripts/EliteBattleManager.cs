//using System.Collections.Generic;
//using UnityEngine;
//using DamageNumbersPro;
//using TMPro;
//using UnityEngine.EventSystems;
//using System.Collections;
//using UnityEngine.SceneManagement;
//using BansheeGz.BGDatabase;
//using UnityEngine.UI;
//using System.Resources;
//using Newtonsoft.Json;
//using System.Linq;
//using Unity.VisualScripting;
//public class EliteBattleManager : MonoBehaviour, IBattleManager
//{
//    [SerializeField]
//    private CameraManager cameraManager;
//    [SerializeField]
//    private Transform turnUI;
//    [SerializeField]
//    private BattleLogManager battleLogManager;
//    [SerializeField]
//    private GameObject eliteBattleManager;
//    [SerializeField]
//    private GameObject canvas;
//    private BuffManager buffManager;

//    PointerEventData pointerEventData;
//    Coroutine runningCoroutine = null;
//    BattleState state;

//    public PlayerInfo playerInfo;
//    TextAsset skillData;
//    List<SkillData> skills;

//    List<SkillData> matchedSkill;
//    public List<SkillData> MatchedSkill => matchedSkill;
//    List<HealthBar> allyHealthBar;
//    List<HealthBar> enemyHealthBar;
//    List<ParserBar> allyDamageBar;
//    List<ParserBar> enemyDamageBar;
//    List<ParserBar> allyHealBar;
//    List<ParserBar> enemyHealBar;
//    List<TextMeshProUGUI> allyDamageText;
//    List<TextMeshProUGUI> enemyDamageText;
//    List<TextMeshProUGUI> allyHealText;
//    List<TextMeshProUGUI> enemyHealText;

//    EliteEnemyBattleSetting eliteEnemyBattleSetting;
//    EliteAllyBattleSetting eliteAllyBattleSetting;

//    List<AnimaActions> allyActions;
//    AnimaActions tmpAnima;
//    List<EnemyActions> enemyActions;

//    List<GameObject> ally;
//    List<GameObject> enemy;

//    List<int> dieAllyAnima;
//    [SerializeField]
//    DamageNumber damageNumber;
//    EventSystem eventSystem;


//    List<GameObject> turn;
//    List<AnimaDataSO> turnList;
//    List<AnimaDataSO> tmpturnList;
//    List<GameObject> isTurn;
//    List<AnimaDataSO> dropAnima;

//    GameObject animaActionUI;
//    GameObject arrow;

//    TurnManager turnManager;
//    public TurnManager TurnManager
//    {
//        get => turnManager;
//        set => turnManager = value;
//    }
//    BGRepo database;
//    BGMetaEntity animaTable;
//    BGMetaEntity skillTable;
//    UnityEngine.UI.Button attackButton;
//    UnityEngine.UI.Button skillButton;
//    Button skill1;
//    Button skill2;
//    bool isZKeyPressed = false;
//    bool isXKeyPressed = false;

//    int turnIndex = 0;
//    int enemyAnimaNum = 0;
//    int allyAnimaNum = 0;
//    int roundNum = 1;
//    int selectEnemy = 0;
//    float maxValue = 0;
//    int index = 0;
//    public List<int> DieAllyAnima => dieAllyAnima;
//    public PlayerInfo PlayerInfo => playerInfo;

//    public BattleState stat
//    {
//        get => state;
//        set => state = value;
//    }
//    public int AllyAnimaNum
//    {
//        get => allyAnimaNum;
//        set => allyAnimaNum = value;
//    }

//    public int TurnIndex
//    {
//        get => turnIndex;
//        set => turnIndex = value;
//    }

//    public int EnemyAnimaNum
//    {
//        get => enemyAnimaNum;
//        set => enemyAnimaNum = value;
//    }

//    public bool IsZKeyPressed
//    {
//        get => isZKeyPressed;
//        set => isZKeyPressed = value;
//    }

//    public Coroutine RunningCoroutine
//    {
//        get => runningCoroutine;
//        set => runningCoroutine = value;
//    }

//    public List<AnimaActions> AllyActions => allyActions;
//    public List<EnemyActions> EnemyActions => enemyActions;
//    public List<GameObject> Turn => turn;
//    public List<AnimaDataSO> TurnList => turnList;
//    public List<AnimaDataSO> TmpturnList => tmpturnList;
//    public List<GameObject> IsTurn => isTurn;
//    public List<AnimaDataSO> DropAnima => dropAnima;

//    public Button AttackButton => attackButton;
//    public Button SkillButton => skillButton;
//    public GameObject AnimaActionUI => animaActionUI;
//    public GameObject Canvas => canvas;

//    public CameraManager CameraManager => cameraManager;
//    public DamageNumber DamageNumber => damageNumber;

//    public IAllyBattleSetting AllyBattleSetting => eliteAllyBattleSetting;
//    public IEnemyBattleSetting EnemyBattleSetting => eliteEnemyBattleSetting;

//    public float MaxValue
//    {
//        get => maxValue;
//        set => maxValue = value;
//    }

//    public List<ParserBar> AllyDamageBar => allyDamageBar;
//    public List<ParserBar> EnemyDamageBar => enemyDamageBar;
//    public List<ParserBar> AllyHealBar => allyHealBar;
//    public List<ParserBar> EnemyHealBar => enemyHealBar;

//    public List<TextMeshProUGUI> AllyDamageText => allyDamageText;
//    public List<TextMeshProUGUI> EnemyDamageText => enemyDamageText;
//    public List<TextMeshProUGUI> AllyHealText => allyHealText;
//    public List<TextMeshProUGUI> EnemyHealText => enemyHealText;
//    public List<HealthBar> AllyHealthBar => allyHealthBar;
//    public List<HealthBar> EnemyHealthBar => enemyHealthBar;

//    public BGMetaEntity AnimaTable => animaTable;
//    public BattleLogManager BattleLogManager => battleLogManager;
//    public BuffManager BuffManager => buffManager;

//    private bool turnRebuild = false;

//    public bool TurnRebuild
//    {
//        get => turnRebuild;
//        set => turnRebuild = value;
//    }

//    SingleAttack singleAttack;
//    MultipleAttack multipleAttack;
//    void Start()
//    {
//        playerInfo = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
//        eventSystem = EventSystem.current;
//        pointerEventData = new PointerEventData(eventSystem);
//        dropAnima = new List<AnimaDataSO>();
//        isTurn = new List<GameObject>();
//        buffManager = new BuffManager();
//        singleAttack = this.AddComponent<SingleAttack>();
//        singleAttack.initialize(this);
//        multipleAttack = this.AddComponent<MultipleAttack>();
//        multipleAttack.initialize(this);
//        turn = new List<GameObject>();
//        skillData = Resources.Load<TextAsset>("Minwoo/SkillList");
//        skills = JsonConvert.DeserializeObject<List<SkillData>>(skillData.text);
//        dieAllyAnima = new List<int>();

//        state = BattleState.start;
//        database = BGRepo.I;
//        animaTable = database.GetMeta("Anima");
//        AnimaActionUISetting();
//        AllyBattlePrepare();
//        EnemyBattlePrepare();
//        BattleStart();
//    }
    
//    void Update()
//    {

//        if (Input.GetKeyDown(KeyCode.Z) && attackButton.interactable && !isZKeyPressed)
//        {
//            ExecuteEvents.Execute(attackButton.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);

//        }
//        if (Input.GetKeyDown(KeyCode.X) && skillButton.interactable && !isXKeyPressed)
//        {
//            ExecuteEvents.Execute(skillButton.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
//        }


//    }

//    void AllyBattlePrepare()
//    {
//        ally = new List<GameObject>();
//        allyActions = new List<AnimaActions>();
//        eliteAllyBattleSetting = gameObject.AddComponent<EliteAllyBattleSetting>();
//        allyHealthBar = new List<HealthBar>();
//        allyDamageBar = new List<ParserBar>();
//        allyHealBar = new List<ParserBar>();
//        allyDamageText = new List<TextMeshProUGUI>();
//        allyHealText = new List<TextMeshProUGUI>();
//        eliteAllyBattleSetting.initialize();
//        eliteAllyBattleSetting.SpawnAlly();
//        setAllyanima();
//        setAllyActions();
//        initializeAllyAnima();
//    }
//    void EnemyBattlePrepare()
//    {
//        enemyAnimaNum = 0;
//        if (enemy != null && enemy.Count > 0)
//        {
//            enemy.Clear();
//            enemyActions.Clear();
//            enemyHealthBar.Clear();

//        }
//        else
//        {
//            enemy = new List<GameObject>();
//            enemyActions = new List<EnemyActions>();
//            enemyHealthBar = new List<HealthBar>();
//            enemyDamageBar = new List<ParserBar>();
//            enemyHealBar = new List<ParserBar>();
//            enemyDamageText = new List<TextMeshProUGUI>();
//            enemyHealText = new List<TextMeshProUGUI>();
//        }
//        eliteEnemyBattleSetting = gameObject.AddComponent<EliteEnemyBattleSetting>();
//        eliteEnemyBattleSetting.stage = SceneManager.GetActiveScene().name.Substring(0,SceneManager.GetActiveScene().name.IndexOf("Elite"));
//        int n = 0;
//        foreach (var tmp in playerInfo.haveAnima)
//        {
//            if (tmp.level > n) n = tmp.level;
//        }
//        eliteEnemyBattleSetting.SpawnEnemy(n);
//        setEnemyanima();
//        setEnemyActions();
//        initializeEnemyAnima();
//    }
//    void setAllyanima()
//    {
//        for (int i = 0; i < eliteAllyBattleSetting.AllyObjPrefab.Count; i++)
//        {
//            ally.Add(eliteAllyBattleSetting.AllyObjPrefab[i]);
//        }
//    }
//    void setEnemyanima()
//    {
//        for (int i = 0; i < eliteEnemyBattleSetting.EnemyObjPrefab.Count; i++)
//        {
//            enemy.Add(eliteEnemyBattleSetting.EnemyObjPrefab[i]);
//        }
//    }
//    void setAllyActions()
//    {
//        for (int i = 0; i < ally.Count; i++)
//        {
//            allyActions.Add(ally[i].AddComponent<AnimaActions>());
//            allyActions[i].DN = Resources.Load<DamageNumber>("Minwoo/DamageNumber");
//            allyActions[i].HN = Resources.Load<DamageNumber>("Minwoo/HealNumber");
//        }
//    }
//    void setEnemyActions()
//    {
//        for (int i = 0; i < enemy.Count; i++)
//        {
//            enemyActions.Add(enemy[i].AddComponent<EnemyActions>());
//            enemyActions[i].DN = Resources.Load<DamageNumber>("Minwoo/DamageNumber");
//            enemyActions[i].HN = Resources.Load<DamageNumber>("Minwoo/HealNumber");
//            enemyActions[i].InitializeWeights();
//        }
//    }
//    void initializeAllyAnima()
//    {
//        dieAllyAnima.Clear();
//        for (int i = 0; i < allyActions.Count; i++)
//        {
//            allyActions[i].animaData = playerInfo.battleAnima[i];
//            allyActions[i].animaData.isAlly = true;
//            allyActions[i].animaData.location = i;
//            var allyStatus = GameObject.Find($"Ally{i}");
//            var allyParser = GameObject.Find($"Ally{i}Name");
//            allyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + allyActions[i].animaData.Objectfile);
//            allyHealthBar.Add(GameObject.Find($"AllyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());

//            allyDamageBar.Add(allyParser.transform.Find($"A{i}Damage").transform.Find($"A{i} Damage Bar").GetComponent<ParserBar>());
//            allyHealBar.Add(allyParser.transform.Find($"A{i}Heal").transform.Find($"A{i} Heal Bar").GetComponent<ParserBar>());
//            allyHealthBar[i].Initialize(allyActions[i].animaData.Maxstamina, allyActions[i].animaData.Stamina);
//            allyDamageBar[i].Initialize();
//            allyHealBar[i].Initialize();
//            allyParser.GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.Name;
//            allyDamageText.Add(allyParser.transform.Find($"A{i}Damage").GetComponent<TextMeshProUGUI>());
//            allyHealText.Add(allyParser.transform.Find($"A{i}Heal").GetComponent<TextMeshProUGUI>());
//            GameObject.Find($"AllyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.level.ToString();
//            allyAnimaNum++;

//        }
//    }
//    void initializeEnemyAnima()
//    {
//        int level = 0;
//        if (playerInfo.haveAnima.Count > 0)
//        {
//            foreach (var anima in playerInfo.haveAnima)
//            {
//                if (anima.level >= level)
//                {
//                    level = anima.level;
//                }

//            }
//        }
//        if(playerInfo.battleAnima.Count > 0)
//        {
//            foreach (var anima in playerInfo.battleAnima)
//            {
//                if (anima.level >= level)
//                {
//                    level = anima.level;
//                }
//            }
//        }
        
//        level += 3;
//        for (int i = 0; i < enemyActions.Count; i++)
//        {
//            enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
//            enemyActions[i].animaData.Initialize(eliteEnemyBattleSetting.battleEnemyAnima[i],level);
//            animaTable.ForEachEntity(entity =>
//            {
//                if (entity.Get<string>("name") == enemyActions[i].animaData.Name && entity.Get<int>("Meeted") == 0)
//                {
//                    entity.Set<int>("Meeted", 1);
//                    DBUpdater.Save();
//                }
//            });
//            enemyActions[i].animaData.location = i;
//            enemyActions[i].animaData.enemyIndex = i;
//            var enemyStatus = GameObject.Find($"Enemy{i}");
//            var enemyParser = GameObject.Find($"Enemy{i}Name");
//            enemyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + enemyActions[i].animaData.Objectfile);
//            enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
//            enemyDamageBar.Add(enemyParser.transform.Find($"E{i}Damage").transform.Find($"E{i} Damage Bar").GetComponent<ParserBar>());
//            enemyHealBar.Add(enemyParser.transform.Find($"E{i}Heal").transform.Find($"E{i} Heal Bar").GetComponent<ParserBar>());
//            enemyHealthBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Stamina);
//            enemyDamageBar[i].Initialize();
//            enemyHealBar[i].Initialize();
//            enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.Name;
//            enemyDamageText.Add(enemyParser.transform.Find($"E{i}Damage").GetComponent<TextMeshProUGUI>());
//            enemyHealText.Add(enemyParser.transform.Find($"E{i}Heal").GetComponent<TextMeshProUGUI>());
//            GameObject.Find($"EnemyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
//            enemyAnimaNum++;
//        }
//    }

//    void AnimaActionUISetting()
//    {
//        Instantiate(Resources.Load<GameObject>("Minwoo/Anima Action UI"), GameObject.Find("Anima Action UI Frame").transform.position, Quaternion.identity, GameObject.Find("Anima Action UI Frame").transform);
//        animaActionUI = GameObject.Find("Anima Action UI(Clone)");
//        Transform attackAction = animaActionUI.transform.Find("Attack Button Frame").transform.Find("Attack Button");
//        attackButton = attackAction.GetComponent<UnityEngine.UI.Button>();
//        Transform skillAction = animaActionUI.transform.Find("Skill Button Frame2").transform.Find("Skill Button2");
//        skillButton = skillAction.GetComponent<UnityEngine.UI.Button>();
//        skill1 = animaActionUI.transform.Find("Skill Button Frame0").transform.Find("Skill Button0").GetComponent<Button>();
//        skill2 = animaActionUI.transform.Find("Skill Button Frame1").transform.Find("Skill Button1").GetComponent<Button>();
//        attackButton.onClick.AddListener(eliteBattleManager.GetComponent<EliteBattleManager>().PlayerAttackButton);
//        skillButton.onClick.AddListener(eliteBattleManager.GetComponent<EliteBattleManager>().PlayerSkillButton);
//        skill1.onClick.AddListener(eliteBattleManager.GetComponent<EliteBattleManager>().SkillButton1);
//        skill2.onClick.AddListener(eliteBattleManager.GetComponent<EliteBattleManager>().SkillButton2);
//        animaActionUI.SetActive(false);

//    }
//    void RoundSetting()
//    {
//        GameObject.Find("Current Round").GetComponent<TextMeshProUGUI>().text = $"{roundNum++} Round";
//    }
//    private void BattleStart()
//    {

//        RoundSetting();
//        turnManager = null;
//        turnManager = ScriptableObject.CreateInstance<TurnManager>();
//        turnManager.ResetTurnList();
//        for (int i = 0; i < enemyActions.Count; i++)
//        {
//            turnManager.InsertAnimaData(enemyActions[i].animaData);
//        }
//        for (int i = 0; i < allyActions.Count; i++)
//        {
//            if (!allyActions[i].animaData.Animadie && eliteAllyBattleSetting.AllyInstance[allyActions[i].animaData.location].activeSelf)
//            {
//                turnManager.InsertAnimaData(allyActions[i].animaData);
//            }


//        }
//        turnList = turnManager.UpdateTurnList();
//        tmpturnList = new List<AnimaDataSO>(turnList);

//        turnIndex = 0;

//        TurnUISetting(turnList);
//        SetState(turnList);
//    }
//    public void TurnUISetting(List<AnimaDataSO> turnList)
//    {
//        if (turn.Count != 0)
//        {
//            for (int i = 0; i < turn.Count;)
//            {
//                DestroyImmediate(turn[i]);
//                turn.RemoveAt(i);
//                isTurn.RemoveAt(i);
//            }
//        }
//        for (int i = 0; i < turnList.Count; i++)
//        {
//            if (turnList[i].isAlly)
//            {
//                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Player Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
//                int index = turn[i].name.IndexOf("(Clone)");
//                turn[i].name = turn[i].name.Substring(0, index) + "" + i;
//                turnUI.transform.Find($"Player Turn Slot{i}").transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[i].Objectfile);
//                isTurn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Player Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Player Turn Slot{i}")));
//                index = isTurn[i].name.IndexOf("(Clone)");
//                isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
//                GameObject.Find($"IsTurn{i}").SetActive(false);
//            }
//            else
//            {
//                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Enemy Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
//                turn[i].transform.Rotate(0, 180f, 0);
//                int index = turn[i].name.IndexOf("(Clone)");
//                turn[i].name = turn[i].name.Substring(0, index) + "" + i;
//                turnUI.transform.Find($"Enemy Turn Slot{i}").transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[i].Objectfile);
//                isTurn.Add(Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Enemy Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Enemy Turn Slot{i}")));
//                index = isTurn[i].name.IndexOf("(Clone)");
//                isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
//                GameObject.Find($"IsTurn{i}").SetActive(false);
//            }
//        }
//        for (int i = 0; i < turnList.Count; i++)
//        {
//            if (turnList[i].turnCheck)
//            {
//                turnList.RemoveAt(i);
//            }
//        }
//    }
//    void SetState(List<AnimaDataSO> turnList)
//    {
//        if (turnList[0].isAlly)
//        {

//            animaActionUI.SetActive(true);
//            isTurn[turnIndex].SetActive(true);
//            int index = -1;
//            for (int i = 0; i < allyActions.Count; i++)
//            {
//                if (ReferenceEquals(turnList[0], allyActions[i].animaData))
//                {
//                    index = i;
//                }
//            }
//            Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(eliteAllyBattleSetting.AllyInstance[allyActions[index].animaData.location].transform.position.x, eliteAllyBattleSetting.AllyInstance[allyActions[index].animaData.location].transform.position.y + 1.2f), Quaternion.identity);
//            arrow = GameObject.Find("Arrow_down(Clone)");
//            GameObject.Find("Anima Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[0].Objectfile);
//            GameObject.Find("Currunt Anima Name").GetComponent<TextMeshProUGUI>().text = turnList[0].Name;
//            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skill\n";
//            for (int i = 0; i < turnList[0].skillName.Count; i++)
//            {
//                animaActionUI.transform.Find($"Skill Button Frame{i}").Find($"Skill Button{i}").Find($"Skill Text{i}").GetComponent<TextMeshProUGUI>().text = turnList[0].skillName[i];
//            }
//            if (turnList[0].skillName.Count == 1)
//            {
//                animaActionUI.transform.Find("Skill Button Frame1").Find("Skill Button1").GetComponent<Button>().interactable = false;
//            }
//            else
//            {
//                skillButton.interactable = true;
//            }
//            state = BattleState.playerTurn;
//        }

//        else
//        {
//            state = BattleState.enemyTurn;
//            isTurn[turnIndex].SetActive(true);
//            runningCoroutine = StartCoroutine(EnemyTurn());
//        }

//    }
//    void PlayerAttackButton()
//    {
//        selectEnemy = 0;

//        if (state != BattleState.playerTurn)
//        {
//            return;
//        }
//        if (state == BattleState.playerTurn)
//        {
//            isZKeyPressed = true;
//            DestroyImmediate(arrow);
//            runningCoroutine = StartCoroutine(PlayerAttack());
//            attackButton.interactable = false;
//            skillButton.interactable = false;
//        }

//    }
//    void PlayerSkillButton()
//    {
//        selectEnemy = 0;

//        if (state != BattleState.playerTurn)
//        {
//            return;
//        }
//        if (state == BattleState.playerTurn)
//        {
//            isZKeyPressed = true;
//            DestroyImmediate(arrow);
//            for (int i = 0; i < 2; i++) 
//            {
//                animaActionUI.transform.Find($"Skill Button Frame{i}").gameObject.SetActive(true);
//            }
//            GameObject.Find("Skill Button0").GetComponent<Button>().Select();
            
//        }

//    }
//    void SkillButton1()
//    {
//        attackButton.interactable = false;
//        skillButton.interactable = false;
//        matchedSkill = skills.Where(s => s.name == skill1.transform.Find("Skill Text0").GetComponent<TextMeshProUGUI>().text).ToList();

//        switch (matchedSkill[0].Type)
//        {
//            case "SingleAttack":
//                runningCoroutine = StartCoroutine(PlayerSingleAttackSkill( 0));
//                break;
//            case "SingleHeal":
//                runningCoroutine = StartCoroutine(PlayerSingleHeal(0));
//                break;
//            case "SingleBuff":
//                runningCoroutine = StartCoroutine(PlayerSingleBuff(0));
//                break;
//            case "SingleDebuff":
//                runningCoroutine = StartCoroutine(PlayerSingleDebuff(0));
//                break;
//            case "MultiAttack":
//                runningCoroutine = StartCoroutine(PlayerMultiSkill(0));
//                break;
//            case "MultiHeal":
//                runningCoroutine = StartCoroutine(PlayerMultiHeal(0));
//                break;
//            case "MultiBuff":
//                runningCoroutine = StartCoroutine(PlayerMultiBuff(0));
//                break;
//            case "MultiDebuff":
//                runningCoroutine = StartCoroutine(PlayerMultiDebuff(0));
//                break;
//        }
//    }
//    void SkillButton2()
//    {
//        attackButton.interactable = false;
//        skillButton.interactable = false;
//        matchedSkill = skills.Where(s => s.name == skill1.transform.Find("Skill Text1").GetComponent<TextMeshProUGUI>().text).ToList();

//        switch (matchedSkill[0].Type)
//        {
//            case "SingleAttack":
//                runningCoroutine = StartCoroutine(PlayerSingleAttackSkill( 1));
//                break;
//            case "SingleHeal":
//                runningCoroutine = StartCoroutine(PlayerSingleHeal(1));
//                break;
//            case "SingleBuff":
//                runningCoroutine = StartCoroutine(PlayerSingleBuff(1));
//                break;
//            case "SingleDebuff":
//                runningCoroutine = StartCoroutine(PlayerSingleDebuff( 1));
//                break;
//            case "MultiAttack":
//                runningCoroutine = StartCoroutine(PlayerMultiSkill(1));
//                break;
//            case "MultiHeal":
//                runningCoroutine = StartCoroutine(PlayerMultiHeal(1));
//                break;
//            case "MultiBuff":
//                runningCoroutine = StartCoroutine(PlayerMultiBuff(1));
//                break;
//            case "MultiDebuff":
//                runningCoroutine = StartCoroutine(PlayerMultiDebuff(1));
//                break;
//        }
//    }
//    IEnumerator PlayerAttack()
//    {
//        yield return AttackCursorInit();

//        tmpAnima = PresentAllyTurn();

//        yield return StartCoroutine(singleAttack.SingleAllyAttack(tmpAnima, selectEnemy));

//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }

//    }
//    IEnumerator PlayerSingleAttackSkill(int skillNum)
//    {

//        yield return AttackCursorInit();

//        tmpAnima = PresentAllyTurn();

//        yield return StartCoroutine(singleAttack.SingleAllySkill(tmpAnima, selectEnemy, skillNum));
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator PlayerMultiSkill(int skillNum)
//    {
//        yield return MultiAttackCursorInit();
//        tmpAnima = PresentAllyTurn();

//        yield return StartCoroutine(multipleAttack.MultiAllySkill(tmpAnima, skillNum));
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }

//    }
//    IEnumerator PlayerSingleHeal(int skillNum)
//    {
//        yield return BuffCursorInit();

//        tmpAnima = PresentAllyTurn();
//        yield return StartCoroutine(singleAttack.SingleAllyHeal(tmpAnima, selectEnemy, skillNum));
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator PlayerMultiHeal(int skillNum)
//    {
//        yield return MultiBuffCursorInit();

//        tmpAnima = PresentAllyTurn();
//        yield return StartCoroutine(multipleAttack.MultiAllyHeal(tmpAnima, skillNum));
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator PlayerSingleBuff(int skillNum)
//    {
//        yield return BuffCursorInit();
//        tmpAnima = PresentAllyTurn();
//        yield return StartCoroutine(singleAttack.SingleAllyBuff(tmpAnima, selectEnemy, skillNum));
//        Buff buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, AllyActions[selectEnemy].animaData, 1);
//        buffManager.AddOrRenuwBuff(buff);
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator PlayerMultiBuff(int skillNum)
//    {
//        yield return MultiBuffCursorInit();
//        tmpAnima = PresentAllyTurn();
//        yield return StartCoroutine(multipleAttack.MultiAllyBuff(tmpAnima, skillNum));
//        Buff buff;
//        for (int i = 0; i < allyActions.Count; i++)
//        {
//            if (allyActions[i].animaData.Animadie) continue;

//            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[i].animaData, 0);
//            buffManager.AddOrRenuwBuff(buff);
//        }
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator PlayerSingleDebuff(int skillNum)
//    {
//        yield return AttackCursorInit();
//        tmpAnima = PresentAllyTurn();
//        yield return StartCoroutine(singleAttack.SingleAllyDebuff(tmpAnima, selectEnemy, skillNum));
//        Buff buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
//        buffManager.AddOrRenuwBuff(buff);
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator PlayerMultiDebuff(int skillNum)
//    {
//        yield return MultiAttackCursorInit();

//        tmpAnima = PresentAllyTurn();
//        yield return StartCoroutine(multipleAttack.MultiAllyDebuff(tmpAnima, skillNum));
//        Buff buff;
//        for (int i = 0; i < enemyActions.Count; i++)
//        {
//            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[i].animaData, 1);
//            buffManager.AddOrRenuwBuff(buff);
//        }
//        if (enemyActions.Count > 0 && turnList.Count == 0)
//        {
//            runningCoroutine = null;
//            BattleStart();
//        }
//        else if (enemyActions.Count > 0 && turnList.Count != 0)
//        {
//            runningCoroutine = null;
//            SetState(turnList);
//        }
//    }
//    IEnumerator EnemyTurn()
//    {
//        int selectAlly = selectNoDieAnima();
//        selectEnemy = Random.Range(0, enemyActions.Count);
//        foreach (EnemyActions enemy in enemyActions)
//        {
//            if (turnList.Count == 0)
//            {
//                break;
//            }

//            if (ReferenceEquals(turnList[0], enemy.animaData))
//            {
//                enemy.DecideAction();
//                if (enemy.performance.Equals("Attack"))
//                {
//                    yield return StartCoroutine(singleAttack.SingleEnemyAttack(enemy, selectAlly));
//                }
//                else if (enemy.performance.Equals("Skill"))
//                {
//                    matchedSkill = skills.Where(s => s.name == enemy.animaData.skillName[0]).ToList();
//                    Buff buff;
//                    switch (matchedSkill[0].Type)
//                    {
//                        case "SingleAttack":
//                            yield return StartCoroutine(singleAttack.SingleEnemySkill(enemy, selectAlly));
//                            break;
//                        case "SingleHeal":
//                            yield return StartCoroutine(singleAttack.SingleEnemyHeal(enemy, selectEnemy));
//                            break;
//                        case "SingleBuff":
//                            yield return StartCoroutine(singleAttack.SingleEnemyBuff(enemy, selectEnemy));
//                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 0);
//                            buffManager.AddOrRenuwBuff(buff);
//                            break;
//                        case "SingleDebuff":
//                            runningCoroutine = StartCoroutine(singleAttack.SingleEnemyDebuff(enemy,selectAlly));
//                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[selectAlly].animaData, 1);
//                            buffManager.AddOrRenuwBuff(buff);
//                            break;
//                        case "MultiAttack":
//                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemySkill(enemy));
//                            break;
//                        case "MultiHeal":
//                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemyHeal(enemy));
//                            break;
//                        case "MultiBuff":
//                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemyBuff(enemy));
//                            for (int i = 0; i < enemyActions.Count; i++)
//                            {
//                                buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[i].animaData, 0);
//                                buffManager.AddOrRenuwBuff(buff);
//                            }
//                            break;
//                        case "MultiDebuff":
//                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemyDebuff(enemy));
//                            for (int i = 0; i < allyActions.Count; i++)
//                            {
//                                if (allyActions[i].animaData.Animadie) continue;
//                                buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[i].animaData, 1);
//                                buffManager.AddOrRenuwBuff(buff);
//                            }

//                            break;
//                    }

//                }
//                break;
//            }
//        }
//        runningCoroutine = null;
//        if (turnList.Count == 0)
//        {
//            BattleStart();
//        }
//        else
//        {
//            SetState(turnList);
//        }
//    }
//    public int selectNoDieAnima()
//    {
//        int randomNumber;
//        do
//        {
//            randomNumber = UnityEngine.Random.Range(0, allyActions.Count);
//        } while (dieAllyAnima.Contains(randomNumber));
//        return randomNumber;
//    }
//    public List<AnimaActions> GetAllyActions()
//    {
//        return allyActions;
//    }
//    public List<EnemyActions> GetEnemyActions()
//    {
//        return enemyActions;
//    }
//    public void WinBattle()
//    {

//        Instantiate(Resources.Load<GameObject>("Minwoo/Battle Win UI"), canvas.transform);
//        for (int i = 0; i < allyActions.Count; i++)
//        {
//            GameObject animaImage = GameObject.Find("Entry Anima List").transform.Find($"Anima {i}").gameObject;
//            animaImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + allyActions[i].animaData.Objectfile);
//            animaImage.SetActive(true);
//        }
//        for (int i = 0; i < dropAnima.Count; i++)
//        {
//            GameObject dropAnimaImage = GameObject.Find("Drop Anima List").transform.Find($"Anima {i}").gameObject;
//            dropAnimaImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + dropAnima[i].Objectfile);
//            dropAnimaImage.SetActive(true);
//        }

//    }
//    public void LoseBattle()
//    {
//        Instantiate(Resources.Load<GameObject>("Minwoo/Game Over UI"), canvas.transform);
//    }

//    AnimaActions PresentAllyTurn()
//    {
//        foreach (AnimaActions anima in allyActions)
//        {
//            if (TurnList.Count == 0)
//            {
//                BattleStart(); 
//            }
//            if (ReferenceEquals(TurnList[0], anima.animaData))
//            {
//                return anima;
//            }
//        }
//        return null;
//    }
//    IEnumerator AttackCursorInit()
//    {
//        arrow = GameObject.Find("Arrow_down(Clone)");
//        DestroyImmediate(arrow);
//        index = 0;
//        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(eliteEnemyBattleSetting.EnemyInstance[index].transform.position.x, eliteEnemyBattleSetting.EnemyInstance[index].transform.position.y + 1.2f), Quaternion.identity);
//        arrow = GameObject.Find("Arrow_down(Clone)");
//        while (true)
//        {
//            if (index != 2 && index < (enemyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
//            {
//                index++;
//                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(eliteEnemyBattleSetting.EnemyInstance[index].transform.position.x, eliteEnemyBattleSetting.EnemyInstance[index].transform.position.y + 1.2f);
//            }
//            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
//            {
//                index--;
//                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(eliteEnemyBattleSetting.EnemyInstance[index].transform.position.x, eliteEnemyBattleSetting.EnemyInstance[index].transform.position.y + 1.2f);
//            }
//            else if (Input.GetKeyDown(KeyCode.Z) && !attackButton.interactable)
//            {
//                selectEnemy = index;
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                break;
//            }
//            else if (Input.GetKeyDown(KeyCode.C))
//            {
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                isZKeyPressed = false;
//                attackButton.interactable = true;
//                skillButton.interactable = true;
//                SetState(turnList);
//                StopCoroutine(runningCoroutine);
//                runningCoroutine = null;
//                yield break;
//            }
//            yield return null;
//        }

//    }
//    IEnumerator MultiAttackCursorInit()
//    {
//        arrow = GameObject.Find("Arrow_down(Clone)");
//        DestroyImmediate(arrow);
//        List<GameObject> tmp = new List<GameObject>();
//        for (int i = 0; i < enemyActions.Count; i++)
//        {
//            tmp.Add(Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(eliteEnemyBattleSetting.EnemyInstance[i].transform.position.x, eliteEnemyBattleSetting.EnemyInstance[i].transform.position.y + 1.2f), Quaternion.identity));
//        }

//        while (true)
//        {
//            if (Input.GetKeyDown(KeyCode.Z) && !attackButton.interactable)
//            {
//                selectEnemy = index;
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                break;
//            }
//            else if (Input.GetKeyDown(KeyCode.C))
//            {
//                foreach (var aa in tmp)
//                {
//                    DestroyImmediate(aa);
//                }
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                isZKeyPressed = false;
//                attackButton.interactable = true;
//                skillButton.interactable = true;
//                SetState(turnList);
//                StopCoroutine(runningCoroutine);
//                runningCoroutine = null;
//                break;
//            }
//            yield return null;
//        }
//    }
//    IEnumerator BuffCursorInit()
//    {
//        arrow = GameObject.Find("Arrow_down(Clone)");
//        DestroyImmediate(arrow);
//        index = 0;
//        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(eliteAllyBattleSetting.AllyInstance[index].transform.position.x, eliteAllyBattleSetting.AllyInstance[index].transform.position.y + 1.2f), Quaternion.identity);
//        arrow = GameObject.Find("Arrow_down(Clone)");
//        while (true)
//        {
//            if (index != 2 && index < (allyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
//            {
//                index++;
//                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(eliteAllyBattleSetting.AllyInstance[index].transform.position.x, eliteAllyBattleSetting.AllyInstance[index].transform.position.y + 1.2f);
//            }
//            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
//            {
//                index--;
//                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(eliteAllyBattleSetting.AllyInstance[index].transform.position.x, eliteAllyBattleSetting.AllyInstance[index].transform.position.y + 1.2f);
//            }
//            else if (Input.GetKeyDown(KeyCode.Z) && !attackButton.interactable)
//            {
//                selectEnemy = index;
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                break;
//            }
//            else if (Input.GetKeyDown(KeyCode.C))
//            {
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                isZKeyPressed = false;
//                attackButton.interactable = true;
//                skillButton.interactable = true;
//                SetState(turnList);
//                StopCoroutine(runningCoroutine);
//                runningCoroutine = null;
//                yield break;
//            }
//            yield return null;
//        }
//    }
//    IEnumerator MultiBuffCursorInit()
//    {
//        arrow = GameObject.Find("Arrow_down(Clone)");
//        DestroyImmediate(arrow);
//        List<GameObject> tmp = new List<GameObject>();
//        for (int i = 0; i < allyActions.Count; i++)
//        {
//            tmp.Add(Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(eliteAllyBattleSetting.AllyInstance[i].transform.position.x, eliteAllyBattleSetting.AllyInstance[i].transform.position.y + 1.2f), Quaternion.identity));
//        }
//        while (true)
//        {
//            if (Input.GetKeyDown(KeyCode.Z) && !attackButton.interactable)
//            {
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                break;
//            }
//            else if (Input.GetKeyDown(KeyCode.C))
//            {
//                DestroyImmediate(arrow);
//                yield return new WaitForSeconds(Time.deltaTime * 30);
//                isZKeyPressed = false;
//                attackButton.interactable = true;
//                skillButton.interactable = true;
//                SetState(turnList);
//                StopCoroutine(runningCoroutine);
//                runningCoroutine = null;
//                break;
//            }
//            yield return null;
//        }
//    }
//}



