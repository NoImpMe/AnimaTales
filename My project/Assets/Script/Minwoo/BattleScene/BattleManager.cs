using BansheeGz.BGDatabase;
using DamageNumbersPro;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour, IBattleManager
{
    [SerializeField]
    CameraManager cameraManager;
    AudioClip bgmClip;
    AudioClip btnClip;
    AudioClip winClip;
    AudioClip loseClip;
    public bool isElite = false;
    public bool isBoss = false;
    private bool isDefeat = false;
    public bool IsDefeat
    {
        get => isDefeat;
        set => isDefeat = value;
    }


    public bool IsBoss => isBoss;
    private int bossStage = 0;
    public CameraManager CameraManager => cameraManager;
    [SerializeField]
    Transform turnUI;
    [SerializeField]
    BattleLogManager battleLogManager;
    public BattleLogManager BattleLogManager => battleLogManager;
    [SerializeField]
    GameObject battleManager;
    [SerializeField]
    GameObject canvas;
    private BuffManager buffManager;
    public BuffManager BuffManager => buffManager;
    public GameObject Canvas => canvas;
    
    PointerEventData pointerEventData;
    Coroutine runningCoroutine = null;
    public Coroutine RunningCoroutine
    {
        get => runningCoroutine; 
        set => runningCoroutine = value;
    }
    BattleState state;
    public BattleState stat
    {
        get => state;
        set => state = value;
    }
    TextAsset skillData;
    List<SkillData> skills;
    List<SkillData> matchedSkill;
    public List<SkillData> MatchedSkill => matchedSkill;
    public PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo => playerInfo;
    List<HealthBar> allyHealthBar;
    public List<HealthBar> AllyHealthBar => allyHealthBar;
    List<HealthBar> enemyHealthBar;
    public List<HealthBar> EnemyHealthBar => enemyHealthBar;
    List<ShieldBar> allyShieldBar;
    public List<ShieldBar> AllyShieldBar => allyShieldBar;
    List<ShieldBar> enemyShieldBar;
    public List<ShieldBar> EnemyShieldBar => enemyShieldBar;
    List<ParserBar> allyDamageBar;

    public List<ParserBar> AllyDamageBar => allyDamageBar;
    List<ParserBar> enemyDamageBar;
    public List<ParserBar> EnemyDamageBar => enemyDamageBar;
    List<ParserBar> allyHealBar;
    public List<ParserBar> AllyHealBar => allyHealBar;
    List<ParserBar> enemyHealBar;
    public List<ParserBar> EnemyHealBar => enemyHealBar;
    List<TextMeshProUGUI> allyDamageText;
    public List<TextMeshProUGUI> AllyDamageText => allyDamageText;
    List<TextMeshProUGUI> enemyDamageText;
    public List<TextMeshProUGUI> EnemyDamageText => enemyDamageText;
    List<TextMeshProUGUI> allyHealText;
    public List<TextMeshProUGUI> AllyHealText => allyHealText;
    List<TextMeshProUGUI> enemyHealText;
    public List<TextMeshProUGUI> EnemyHealText => enemyHealText;

    EnemyBattleSetting enemyBattleSetting;
    public IEnemyBattleSetting EnemyBattleSetting => enemyBattleSetting;
    AllyBattleSetting allyBattleSetting;
    public IAllyBattleSetting AllyBattleSetting => allyBattleSetting;
    AnimaActions tmpAnima;
    List<AnimaActions> allyActions;
    public List<AnimaActions> AllyActions => allyActions;
    List<EnemyActions> enemyActions;
    public List<EnemyActions> EnemyActions => enemyActions;

    List<GameObject> ally;
    List<GameObject> enemy;

    List<int> dieAllyAnima;
    public List<int> DieAllyAnima => dieAllyAnima;

    EventSystem eventSystem;

    
    List<GameObject> turn;
    public List<GameObject> Turn => turn;

    List<AnimaDataSO> turnList;
    public List<AnimaDataSO> TurnList => turnList;

    List<AnimaDataSO> tmpturnList;
    public List<AnimaDataSO> TmpturnList => tmpturnList;

    List<GameObject> isTurn;
    public List<GameObject> IsTurn => isTurn;

    
    List<AnimaDataSO> dropAnima;
    public List<AnimaDataSO> DropAnima => dropAnima;

    GameObject animaActionUI;
    AnimaActionUIController animaActionUIController;
    public AnimaActionUIController AnimaActionUIController
    {
        get => animaActionUIController;
        set => animaActionUIController = value;
    }
    public GameObject AnimaActionUI => animaActionUI;

    GameObject arrow;
    
    GameObject rebuild;

    TurnManager turnManager;
    public TurnManager TurnManager
    {
        get => turnManager; 
        set => turnManager = value;
    }
    Button attackButton;
    public Button AttackButton => attackButton;
    Button skillButton;
    public Button SkillButton => skillButton;
    Button skill1;
    public Button Skill1 => skill1;
    Button skill2;
    public Button Skill2 => skill2;
    
    BGRepo database;
    BGMetaEntity animaTable;
    public BGMetaEntity AnimaTable => animaTable;
    int turnIndex = 0;
    public int TurnIndex
    {
        get => turnIndex;
        set => turnIndex = value;
    }
    int enemyAnimaNum = 0;
    public int EnemyAnimaNum
    {
        get => enemyAnimaNum;
        set => enemyAnimaNum = value;
    }
    
    int allyAnimaNum = 0;
    public int AllyAnimaNum
    {
        get => allyAnimaNum;
        set => allyAnimaNum = value;
    }
    
    int roundNum = 0;
    
    int selectEnemy = 0;
    int index = 0;
    float maxValue = 0;
    public float MaxValue
    {
        get => maxValue;
        set => maxValue = value;
    }
    
    private bool turnRebuild = false;

    public bool TurnRebuild
    {
        get => turnRebuild;
        set => turnRebuild = value;
    }

    SingleAttack singleAttack;
    MultipleAttack multipleAttack;
    bool checkZ = false;
    bool checkSkill = false;
    bool cleared = false;

    void Start()
    {
        btnClip = Resources.Load<AudioClip>("Minwoo/Sounds/SFX/AttackButtonSFX");
        winClip = Resources.Load<AudioClip>("Minwoo/Sounds/SFX/BattleWinSFX");
        loseClip = Resources.Load<AudioClip>("Minwoo/Sounds/SFX/BattleLoseSFX"); 
        AudioManager.Instance.PlayBGM(bgmClip);
        if (isBoss) bossStage += 1;
        playerInfo = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
        eventSystem = EventSystem.current;
        pointerEventData = new PointerEventData(eventSystem);
        singleAttack = this.AddComponent<SingleAttack>();
        singleAttack.initialize(this);
        multipleAttack = this.AddComponent<MultipleAttack>();
        multipleAttack.initialize(this);
        isTurn = new List<GameObject>();
        buffManager = new BuffManager();
        turn = new List<GameObject>();
        skillData = Resources.Load<TextAsset>("Minwoo/SkillList");
        skills = JsonConvert.DeserializeObject<List<SkillData>>(skillData.text);
        dieAllyAnima = new List<int>();
        dropAnima = new List<AnimaDataSO>();
        state = BattleState.start;
        database = BGRepo.I;
        animaTable = database.GetMeta("Anima");
        playerInfo.MaxLevelChanged();
        AnimaActionUISetting();
        AllyBattlePrepare();
        EnemyBattlePrepare();
        AudioManager.Instance.PlayBGM(bgmClip);
        StartCoroutine(BattleStart());
    }
    void Update()
    {
        if (animaActionUIController.selectAction.activeSelf && !cleared && !checkZ)
        {
            var current = EventSystem.current.currentSelectedGameObject;

            if (current == null || !current.transform.IsChildOf(animaActionUIController.selectAction.transform))
            {
                animaActionUIController.attackButton.Select();
            }
            
            if (Input.GetKeyUp(KeyCode.Z) && current.GetComponent<Button>().interactable)
            {
                checkZ = true;
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
                attackButton.interactable = false;
                skillButton.interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        if (!animaActionUIController.selectAction.activeSelf && !animaActionUIController.skill1Frame.activeSelf && !cleared)
        {
            animaActionUIController.cancleButton.Select();
        }
        else if (animaActionUIController.selectSkill.activeSelf && !cleared && !checkSkill)
        {
            var current = EventSystem.current.currentSelectedGameObject;
            if (current == null || !current.transform.IsChildOf(animaActionUIController.selectSkill.transform))
            {
                animaActionUIController.skill1.Select();
            }
        }
    }

    void AllyBattlePrepare()
    {
        ally = new List<GameObject>();
        allyActions = new List<AnimaActions>();
        allyBattleSetting = gameObject.AddComponent<AllyBattleSetting>();
        allyHealthBar = new List<HealthBar>();
        allyShieldBar = new List<ShieldBar>();
        allyDamageBar = new List<ParserBar>();
        allyHealBar = new List<ParserBar>();
        allyDamageText = new List<TextMeshProUGUI>();
        allyHealText = new List<TextMeshProUGUI>();
        allyBattleSetting.initialize();
        allyBattleSetting.SpawnAlly();
        setAllyanima();
        setAllyActions();
        initializeAllyAnima();
    }
    void EnemyBattlePrepare()
    {
        enemyAnimaNum = 0;
        if (enemy != null &&  enemy.Count > 0)
        {
            enemy.Clear();
            enemyActions.Clear();
            enemyHealthBar.Clear();
        }
        else
        {
            enemy = new List<GameObject>();
            enemyActions = new List<EnemyActions>();
            enemyHealthBar = new List<HealthBar>();
            enemyShieldBar = new List<ShieldBar>();
            enemyDamageBar = new List<ParserBar>();
            enemyHealBar = new List<ParserBar>();
            enemyDamageText = new List<TextMeshProUGUI>();
            enemyHealText = new List<TextMeshProUGUI>();
        }
        enemyBattleSetting = gameObject.AddComponent<EnemyBattleSetting>();
        int level = playerInfo.maxLevel;

        if (isElite)
        {
            enemyBattleSetting.Stage = SceneManager.GetActiveScene().name.Substring(0, SceneManager.GetActiveScene().name.IndexOf("EliteBattle"));
            enemyBattleSetting.SpawnElite(level);
            setEnemyanima();
            setEnemyActions();
            initializeEnemyAnima();
        }
        else if (isBoss)
        {
            enemyBattleSetting.Stage = SceneManager.GetActiveScene().name.Substring(0, SceneManager.GetActiveScene().name.IndexOf("BossBattle"));
            enemyBattleSetting.SpawnBoss();
            setEnemyanima();
            setEnemyActions();
            initializeEnemyAnima();
        }
        else
        {
            enemyBattleSetting.Stage = SceneManager.GetActiveScene().name.Substring(0, SceneManager.GetActiveScene().name.IndexOf("Battle"));
            enemyBattleSetting.SpawnEnemy(level);
            setEnemyanima();
            setEnemyActions();
            initializeEnemyAnima();
        }
        string bgm = enemyBattleSetting.Stage + ((isElite) ? "Elite" : "") + ((isBoss) ? "Boss" : "") + "BGM";
        bgmClip = Resources.Load<AudioClip>($"Minwoo/Sounds/BGM/BattleBGM");
    }
    void setAllyanima()
    {
        for (int i = 0; i < allyBattleSetting.AllyObjPrefab.Count; i++)
        {
            ally.Add(allyBattleSetting.AllyObjPrefab[i]);
        }
    }
    void setEnemyanima()
    {
        for (int i = 0; i < enemyBattleSetting.EnemyObjPrefab.Count; i++)
        {
            enemy.Add(enemyBattleSetting.EnemyObjPrefab[i]);
        }
    }
    void setAllyActions()
    {
        for (int i = 0; i < ally.Count; i++)
        {
            allyActions.Add(ally[i].AddComponent<AnimaActions>());
            allyActions[i].DN = Resources.Load<DamageNumber>("Minwoo/DamageNumber");
            allyActions[i].HN = Resources.Load<DamageNumber>("Minwoo/HealNumber");
        }
    }
    void setEnemyActions()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            enemyActions.Add(enemy[i].AddComponent<EnemyActions>());
            enemyActions[i].DN = Resources.Load<DamageNumber>("Minwoo/DamageNumber");
            enemyActions[i].HN = Resources.Load<DamageNumber>("Minwoo/HealNumber");
            enemyActions[i].InitializeWeights();
        }
    }
    void initializeAllyAnima()
    {
        dieAllyAnima.Clear();
        for (int i = 0; i < allyActions.Count; i++)
        {
            allyActions[i].animaData = playerInfo.battleAnima[i];
            allyActions[i].animaData.isAlly = true;
            allyActions[i].animaData.location = i;
            var allyStatus = GameObject.Find($"Ally{i}");
            var allyParser = GameObject.Find($"Ally{i}Name");
            allyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite= Resources.Load<Sprite>("Anima_Sprites/" + allyActions[i].animaData.Objectfile);
            allyHealthBar.Add(GameObject.Find($"AllyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
            allyShieldBar.Add(GameObject.Find($"AllyAnimaHP{i}").transform.Find("SD").GetComponent<ShieldBar>());
            allyDamageBar.Add(allyParser.transform.Find($"A{i}Damage").transform.Find($"A{i} Damage Bar").GetComponent<ParserBar>());
            allyHealBar.Add(allyParser.transform.Find($"A{i}Heal").transform.Find($"A{i} Heal Bar").GetComponent<ParserBar>());
            allyHealthBar[i].Initialize(allyActions[i].animaData.Maxstamina, allyActions[i].animaData.Stamina);
            allyShieldBar[i].Initialize(allyActions[i].animaData.Maxstamina, allyActions[i].animaData.Shield);
            allyDamageBar[i].Initialize();
            allyHealBar[i].Initialize();
            allyParser.GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.Name;
            allyDamageText.Add(allyParser.transform.Find($"A{i}Damage").GetComponent<TextMeshProUGUI>());
            allyHealText.Add(allyParser.transform.Find($"A{i}Heal").GetComponent<TextMeshProUGUI>());
            GameObject.Find($"AllyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.level.ToString();
            allyAnimaNum++;

        }
    }
    void initializeEnemyAnima()
    {
        int level =  playerInfo.maxLevel;
        if (isElite)
        {
            level += 3;
            for (int i = 0; i < enemyActions.Count; i++)
            {
                enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
                enemyActions[i].animaData.Initialize(enemyBattleSetting.BattleEnemyAnima[i], level);
                animaTable.ForEachEntity(entity =>
                {
                    if (entity.Get<string>("name") == enemyActions[i].animaData.Name && entity.Get<int>("Meeted") == 0)
                    {
                        entity.Set<int>("Meeted", 1);
                        DBUpdater.Save();
                    }
                });
                enemyActions[i].animaData.location = i;
                enemyActions[i].animaData.enemyIndex = i;
                var enemyStatus = GameObject.Find($"Enemy{i}");
                var enemyParser = GameObject.Find($"Enemy{i}Name");
                enemyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + enemyActions[i].animaData.Objectfile);
                enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
                enemyShieldBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("SD").GetComponent<ShieldBar>());
                enemyDamageBar.Add(enemyParser.transform.Find($"E{i}Damage").transform.Find($"E{i} Damage Bar").GetComponent<ParserBar>());
                enemyHealBar.Add(enemyParser.transform.Find($"E{i}Heal").transform.Find($"E{i} Heal Bar").GetComponent<ParserBar>());
                enemyHealthBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Stamina);
                enemyShieldBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Shield);
                enemyDamageBar[i].Initialize();
                enemyHealBar[i].Initialize();
                enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.Name;
                enemyDamageText.Add(enemyParser.transform.Find($"E{i}Damage").GetComponent<TextMeshProUGUI>());
                enemyHealText.Add(enemyParser.transform.Find($"E{i}Heal").GetComponent<TextMeshProUGUI>());
                GameObject.Find($"EnemyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
                enemyAnimaNum++;
            }
        }
        else if (isBoss)
        {
            level += 8;
            for (int i = 0; i < enemyActions.Count; i++)
            {
                enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
                enemyActions[i].animaData.Initialize(enemyBattleSetting.BattleEnemyAnima[i], level);
                animaTable.ForEachEntity(entity =>
                {
                    if (entity.Get<string>("name") == enemyActions[i].animaData.Name && entity.Get<int>("Meeted") == 0)
                    {
                        entity.Set<int>("Meeted", 1);
                        DBUpdater.Save();
                    }
                });
                if (enemyActions[i].animaData.Name == "tombstone0") enemyActions[i].animaData.isTomb = true;
                enemyActions[i].animaData.location = i;
                enemyActions[i].animaData.enemyIndex = i;
                enemyActions[i].animaData.isBoss = true;
                var enemyStatus = GameObject.Find($"Enemy{i}");
                var enemyParser = GameObject.Find($"Enemy{i}Name");
                enemyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + enemyActions[i].animaData.Objectfile);
                enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
                enemyShieldBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("SD").GetComponent<ShieldBar>());
                enemyDamageBar.Add(enemyParser.transform.Find($"E{i}Damage").transform.Find($"E{i} Damage Bar").GetComponent<ParserBar>());
                enemyHealBar.Add(enemyParser.transform.Find($"E{i}Heal").transform.Find($"E{i} Heal Bar").GetComponent<ParserBar>());
                enemyHealthBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Stamina);
                enemyShieldBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Shield);
                enemyDamageBar[i].Initialize();
                enemyHealBar[i].Initialize();
                enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.Name;
                enemyDamageText.Add(enemyParser.transform.Find($"E{i}Damage").GetComponent<TextMeshProUGUI>());
                enemyHealText.Add(enemyParser.transform.Find($"E{i}Heal").GetComponent<TextMeshProUGUI>());
                GameObject.Find($"EnemyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
                enemyAnimaNum++;
            }
        }
        else
        {
            switch (enemyActions.Count)
            {
                case 1:
                    level -= 1;
                    break;
                case 2:
                    level -= 2;
                    break;
                case 3:
                    level -= 3;
                    break;
            }
            for (int i = 0; i < enemyActions.Count; i++)
            {
                enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
                enemyActions[i].animaData.Initialize(enemyBattleSetting.BattleEnemyAnima[i], level);
                animaTable.ForEachEntity(entity =>
                {
                    if (entity.Get<string>("name") == enemyActions[i].animaData.Name && entity.Get<int>("Meeted") == 0)
                    {
                        entity.Set<int>("Meeted", 1);
                        DBUpdater.Save();
                    }

                });
                enemyActions[i].animaData.location = i;
                enemyActions[i].animaData.enemyIndex = i;
                var enemyStatus = GameObject.Find($"Enemy{i}");
                var enemyParser = GameObject.Find($"Enemy{i}Name");
                enemyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + enemyActions[i].animaData.Objectfile);
                enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
                enemyShieldBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("SD").GetComponent<ShieldBar>());
                enemyDamageBar.Add(enemyParser.transform.Find($"E{i}Damage").transform.Find($"E{i} Damage Bar").GetComponent<ParserBar>());
                enemyHealBar.Add(enemyParser.transform.Find($"E{i}Heal").transform.Find($"E{i} Heal Bar").GetComponent<ParserBar>());
                enemyHealthBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Stamina);
                enemyShieldBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Shield);
                enemyDamageBar[i].Initialize();
                enemyHealBar[i].Initialize();
                enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.Name;
                enemyDamageText.Add(enemyParser.transform.Find($"E{i}Damage").GetComponent<TextMeshProUGUI>());
                enemyHealText.Add(enemyParser.transform.Find($"E{i}Heal").GetComponent<TextMeshProUGUI>());
                GameObject.Find($"EnemyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
                enemyAnimaNum++;
            }
        }
            
    }
     void AnimaActionUISetting()
    {
        animaActionUI = GameObject.Find("Anima Action UI Frame").transform.Find("Anima Action UI").gameObject;
        animaActionUIController = animaActionUI.GetComponent<AnimaActionUIController>();
        attackButton = animaActionUIController.attackButton;
        skillButton = animaActionUIController.skillButton;
        skill1 = animaActionUIController.skill1;
        skill2 = animaActionUIController.skill2;
        attackButton.onClick.AddListener(PlayerAttackButton);
        skillButton.onClick.AddListener(PlayerSkillButton);
        skill1.onClick.AddListener(SkillButton1);
        skill2.onClick.AddListener(SkillButton2);
    }
    void RoundSetting()
    {
        GameObject.Find("Current Round").GetComponent<TextMeshProUGUI>().text = $"{++roundNum} Round";
    }
    private IEnumerator BattleStart()
    {
        
        RoundSetting();
        if (isBoss && roundNum > 1)
        {
            //Buff buff;
            int selectAlly = selectNoDieAnima();

            switch (enemyBattleSetting.Stage)
            {
                case "Felix":
                    yield return singleAttack.FelixRoundSkill(enemyActions[0], selectAlly);
                    break;
                case "Phobia":
                    yield return singleAttack.PhobiaRoundSkill(enemyActions[0], turnList, selectAlly);
                    break;
                case "Amare":
                    if (enemyActions[0].animaData.Name != "amare5")
                    {
                        yield break;
                    }
                    if (enemyActions.Count > 1)
                    {
                        DestroyImmediate(enemyBattleSetting.EnemyHpInstance[1]);
                        enemyBattleSetting.EnemyHpInstance.RemoveAt(1);
                        enemyHealthBar.RemoveAt(1);
                        enemyActions.RemoveAt(1);
                        DestroyImmediate(enemyBattleSetting.EnemyInstance[1]);
                        DestroyImmediate(enemyBattleSetting.EnemyInfoInstance[1]);
                        enemyBattleSetting.EnemyInstance.RemoveAt(1);
                        enemyBattleSetting.EnemyInfoInstance.RemoveAt(1);
                        enemyAnimaNum -= 1;
                    }
                    while (enemyBattleSetting.EnemyInfoInstance.Count != 1)
                    {
                        yield return null;
                    }
                    int rannum = Random.Range(0, playerInfo.haveAnima.Count);

                    if (PlayerInfo.haveAnima.Count > 0)
                    {
                        enemyBattleSetting.AmareSpawn(playerInfo.haveAnima[rannum].Objectfile);
                    }
                    enemy.Add(enemyBattleSetting.EnemyObjPrefab[1]);
                    enemyActions.Add(enemy[1].AddComponent<EnemyActions>());
                    enemyActions[1].DN = Resources.Load<DamageNumber>("Minwoo/DamageNumber");
                    enemyActions[1].HN = Resources.Load<DamageNumber>("Minwoo/HealNumber");
                    enemyActions[1].InitializeWeights();
                    enemyActions[1].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
                    enemyActions[1].animaData.Initialize(enemyBattleSetting.BattleEnemyAnima[1], playerInfo.haveAnima[rannum].level);
                    enemyActions[1].animaData.location = 1;
                    enemyActions[1].animaData.enemyIndex = 1;
                    enemyActions[1].animaData.isClone = true;
                    var enemyStatus = GameObject.Find($"Enemy{1}");
                    var enemyParser = GameObject.Find($"Enemy{1}Name");
                    enemyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + enemyActions[1].animaData.Objectfile);
                    enemyDamageBar.Add(enemyParser.transform.Find($"E{1}Damage").transform.Find($"E{1} Damage Bar").GetComponent<ParserBar>());
                    enemyHealBar.Add(enemyParser.transform.Find($"E{1}Heal").transform.Find($"E{1} Heal Bar").GetComponent<ParserBar>());
                    enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{1}").transform.Find("HP").GetComponent<HealthBar>());
                    enemyShieldBar.Add(GameObject.Find($"EnemyAnimaHP{1}").transform.Find("SD").GetComponent<ShieldBar>());
                    enemyHealthBar[1].Initialize(enemyActions[1].animaData.Maxstamina, enemyActions[1].animaData.Stamina);
                    enemyShieldBar[1].Initialize(enemyActions[1].animaData.Maxstamina, enemyActions[1].animaData.Shield);
                    enemyDamageBar[1].Initialize();
                    enemyHealBar[1].Initialize();
                    enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[1].animaData.Name;
                    enemyDamageText.Add(enemyParser.transform.Find($"E{1}Damage").GetComponent<TextMeshProUGUI>());
                    enemyHealText.Add(enemyParser.transform.Find($"E{1}Heal").GetComponent<TextMeshProUGUI>());
                    GameObject.Find($"EnemyAnimaHP{1}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[1].animaData.level.ToString();
                    enemyAnimaNum += 1;
                    break;
                case "Irascor":
                    yield return multipleAttack.IrascorRoundSkill(enemyActions[0]);
                    break;
                case "Havet":
                    yield return multipleAttack.HavetRoundSkill(enemyActions[0]);
                    break;
            }
        }
        turnManager = null;
        turnManager = ScriptableObject.CreateInstance<TurnManager>();
        turnManager.ResetTurnList();
        for (int i = 0; i < enemyActions.Count; i++)
        {
            if (enemyActions[i].animaData.isTomb) continue;
            turnManager.InsertAnimaData(enemyActions[i].animaData);
        }
        for (int i = 0; i < allyActions.Count; i++)
        {
            if (!allyActions[i].animaData.Animadie && allyBattleSetting.AllyInstance[allyActions[i].animaData.location].activeSelf)
            {
                turnManager.InsertAnimaData(allyActions[i].animaData);
            }
                

        }
        turnList = turnManager.UpdateTurnList();
        tmpturnList = new List<AnimaDataSO>(turnList);

        turnIndex = 0;
        
        TurnUISetting(turnList);
        
        SetState(turnList);
    }
    public void TurnUISetting(List<AnimaDataSO> turnList)
    {
        if (turn.Count != 0)
        {
            for (int i = 0; i < turn.Count;)
            {
                DestroyImmediate(turn[i]);
                turn.RemoveAt(i);
                isTurn.RemoveAt(i);
            }
        }
        int tmp = 0;
        if (turnRebuild)
        {
            for (int i = 0; i < tmpturnList.Count; i++)
            {
                if (tmpturnList[i].turnCheck)
                {
                    if (tmpturnList[i].isAlly)
                    {
                        turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Player Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                        int index = turn[i].name.IndexOf("(Clone)");
                        turn[i].name = turn[i].name.Substring(0, index) + "" + i;
                        turnUI.transform.Find($"Player Turn Slot{i}").transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + tmpturnList[i].Objectfile);
                        isTurn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Player Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Player Turn Slot{i}")));
                        index = isTurn[i].name.IndexOf("(Clone)");
                        isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
                        GameObject.Find($"IsTurn{i}").SetActive(false);
                    }
                    else
                    {
                        turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Enemy Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                        turn[i].transform.Rotate(0, 180f, 0);
                        int index = turn[i].name.IndexOf("(Clone)");
                        turn[i].name = turn[i].name.Substring(0, index) + "" + i;
                        turnUI.transform.Find($"Enemy Turn Slot{i}").transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + tmpturnList[i].Objectfile);
                        isTurn.Add(Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Enemy Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Enemy Turn Slot{i}")));
                        index = isTurn[i].name.IndexOf("(Clone)");
                        isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
                        GameObject.Find($"IsTurn{i}").SetActive(false);
                    }
                    tmp += 1;
                }
            }
        }

        for (int i = 0; i < turnList.Count; i++)
        {
            if (turnList[i].isAlly)
            {
                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Player Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                int index = turn[tmp].name.IndexOf("(Clone)");
                turn[tmp].name = turn[tmp].name.Substring(0, index) + "" + tmp;
                turnUI.transform.Find($"Player Turn Slot{tmp}").transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + turnList[i].Objectfile);
                isTurn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Player Turn Slot{tmp}").transform.position, Quaternion.identity, turnUI.transform.Find($"Player Turn Slot{tmp}")));
                index = isTurn[tmp].name.IndexOf("(Clone)");
                isTurn[tmp].name = isTurn[tmp].name.Substring(0, index) + "" + tmp;
                GameObject.Find($"IsTurn{tmp}").SetActive(false);
            }
            else
            {
                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Enemy Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                turn[tmp].transform.Rotate(0, 180f, 0);
                int index = turn[tmp].name.IndexOf("(Clone)");
                turn[tmp].name = turn[tmp].name.Substring(0, index) + "" + tmp;
                turnUI.transform.Find($"Enemy Turn Slot{tmp}").transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + turnList[i].Objectfile);
                isTurn.Add(Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Enemy Turn Slot{tmp}").transform.position, Quaternion.identity, turnUI.transform.Find($"Enemy Turn Slot{tmp}")));
                index = isTurn[tmp].name.IndexOf("(Clone)");
                isTurn[tmp].name = isTurn[tmp].name.Substring(0, index) + "" + tmp;
                GameObject.Find($"IsTurn{tmp}").SetActive(false);
            }
            tmp += 1;
        }
        if (!turnRebuild)
        {
            for (int i = 0; i < turnList.Count; i++)
            {
                if (turnList[i].turnCheck)
                {
                    turnList[i].turnCheck = false;
                }
            }
        }
        turnRebuild = false;
    }
    void SetState(List<AnimaDataSO> turnList)
    {
        if(enemyBattleSetting.Stage == "Amare" && isBoss)
        {
            if (enemyActions[0].animaData.Name != "amare5")
            {
                return;
            }
        }
        skill1.interactable = true;
        skill2.interactable = true;
        animaActionUIController.cancleButton.interactable = true;
        animaActionUIController.selectSkill.SetActive(false);
        animaActionUIController.selectAction.SetActive(true);
        attackButton.interactable = true;
        skillButton.interactable = true;
        checkZ = false;
        checkSkill = false;
        if (arrow != null)
        {
            DestroyImmediate(arrow);
        }
        if (turnList[0].turnCheck)
        {
            turnList.RemoveAt(0);
            if(turnList.Count == 0)
            {
                turnIndex += 1;
                runningCoroutine = StartCoroutine(BattleStart());
                return;
            }

        }
        
        if (turnList[0].isAlly)
        {
            animaActionUI.SetActive(true);
            attackButton.Select();
            isTurn[turnIndex].SetActive(true);
            int index = -1;
            for (int i = 0; i < allyActions.Count; i++)
            {
                if (ReferenceEquals(turnList[0], allyActions[i].animaData))
                {
                    index = i;
                }
            }
            Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(allyBattleSetting.AllyInstance[allyActions[index].animaData.location].transform.position.x, allyBattleSetting.AllyInstance[allyActions[index].animaData.location].transform.position.y + 1.2f), Quaternion.identity);
            arrow = GameObject.Find("Arrow_down(Clone)");
            animaActionUIController.portrait.sprite = Resources.Load<Sprite>("Anima_Sprites/" + turnList[0].Objectfile);
            animaActionUIController.animaName.text = turnList[0].Name;
            animaActionUIController.selectSkill.SetActive(true);
            animaActionUIController.skill1Frame.SetActive(false);
            animaActionUIController.skill2Frame.SetActive(false);
            if (turnList[0].skillName.Count > 0)
            {
                animaActionUIController.skill1Des.text = turnList[0].skillName[0];
                animaActionUIController.skill1Frame.SetActive(true);
                animaActionUIController.skill1Image.sprite = turnList[0].skillSprite[0];
            }
            if (turnList[0].skillName.Count == 2)
            {
                animaActionUIController.skill2Des.text = turnList[0].skillName[1];
                animaActionUIController.skill2Frame.SetActive(true);
                animaActionUIController.skill2Image.sprite = turnList[0].skillSprite[1];
            }
            animaActionUIController.selectSkill.SetActive(false);
            if (TombAlive()) skillButton.interactable = false;
            state = BattleState.playerTurn;
        }
        else
        {
            state = BattleState.enemyTurn;
            isTurn[turnIndex].SetActive(true);
            runningCoroutine = StartCoroutine(EnemyTurn());
        }
    }
    public void PlayerAttackButton()
    {
        if(state != BattleState.playerTurn)
        {
            return;
        }
        selectEnemy = 0;
        
        if (state == BattleState.playerTurn)
        {
            DestroyImmediate(arrow);
            runningCoroutine = StartCoroutine(PlayerAttack());
        }

    }
    public void PlayerSkillButton()
    {
       
        selectEnemy = 0;
        if (state != BattleState.playerTurn)
        {
            return;
        }
        runningCoroutine = StartCoroutine(SkillButtonClicked());
    }
    IEnumerator SkillButtonClicked()
    {
        yield return null;
        animaActionUIController.selectAction.SetActive(false);
        animaActionUIController.selectSkill.SetActive(true);
        if (animaActionUIController.skill1Frame.activeSelf) skill1.Select();
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Z) )
            {
                if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name.Contains("Cancle"))
                {
                    AudioManager.Instance.PlaySFX(btnClip);
                    EventSystem.current.currentSelectedGameObject.GetComponent<UIButtonHighlight>().DeactivateBorder();
                    EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
                    attackButton.interactable = true;
                    skillButton.interactable = true;
                    SetState(turnList);
                    if (runningCoroutine != null) StopCoroutine(runningCoroutine);
                    runningCoroutine = null;
                    break;
                }
                else
                {
                    AudioManager.Instance.PlaySFX(btnClip);
                    EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
                    break;
                }
            }
            yield return null;
        }
    }
    public void SkillButton1()
    {
        skill1.interactable = false;
        skill2.interactable = false;
        animaActionUIController.cancleButton.interactable = false;
        checkSkill = true;
        matchedSkill = skills.Where(s => s.name == animaActionUIController.skill1Des.text).ToList();

        switch (matchedSkill[0].Type)
        {
            case "SingleAttack":
                runningCoroutine = StartCoroutine(PlayerSingleAttackSkill( 0));
                break;
            case "SingleHeal":
                runningCoroutine = StartCoroutine(PlayerSingleHeal( 0));
                break;
            case "SingleShield":
                runningCoroutine = StartCoroutine(PlayerSingleShield(0));
                break;
            case "SingleBuff":
                runningCoroutine = StartCoroutine(PlayerSingleBuff(0));
                break;
            case "SingleDebuff":
                runningCoroutine = StartCoroutine(PlayerSingleDebuff(0));
                break;
            case "MultiAttack":
                runningCoroutine = StartCoroutine(PlayerMultiSkill(0));
                break;
            case "MultiHeal":
                runningCoroutine = StartCoroutine(PlayerMultiHeal(0));
                break;
            case "MultiShield":
                runningCoroutine = StartCoroutine(PlayerMultiShield(0));
                break;
            case "MultiBuff":
                runningCoroutine = StartCoroutine(PlayerMultiBuff(0));
                break;
            case "MultiDebuff":
                runningCoroutine = StartCoroutine(PlayerMultiDebuff(0));
                break;
            
        }
        
        
    }
    public void SkillButton2()
    {
        skill1.interactable = false;
        skill2.interactable = false;
        animaActionUIController.cancleButton.interactable = false;
        checkSkill = true;
        matchedSkill = skills.Where(s => s.name == animaActionUIController.skill2Des.text).ToList();
        switch (matchedSkill[0].Type)
        {
            case "SingleAttack":
                runningCoroutine = StartCoroutine(PlayerSingleAttackSkill( 1));
                break;
            case "SingleHeal":
                runningCoroutine = StartCoroutine(PlayerSingleHeal( 1));
                break;
            case "SingleShield":
                runningCoroutine = StartCoroutine(PlayerSingleShield(1));
                break;
            case "SingleBuff":
                runningCoroutine = StartCoroutine(PlayerSingleBuff(1));
                break;
            case "SingleDebuff":
                runningCoroutine = StartCoroutine(PlayerSingleDebuff(1));
                break;
            case "MultiAttack":
                runningCoroutine = StartCoroutine(PlayerMultiSkill(1));
                break;
            case "MultiHeal":
                runningCoroutine = StartCoroutine(PlayerMultiHeal(1));
                break;
            case "MultiShield":
                runningCoroutine = StartCoroutine(PlayerMultiShield(1));
                break;
            case "MultiBuff":
                runningCoroutine = StartCoroutine(PlayerMultiBuff(1));
                break;
            case "MultiDebuff":
                runningCoroutine = StartCoroutine(PlayerMultiDebuff(1));
                break;
        }
    }
    IEnumerator PlayerAttack()
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(AttackCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        
        yield return runningCoroutine = StartCoroutine(singleAttack.SingleAllyAttack(tmpAnima, selectEnemy));
        
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }

    }
    
    IEnumerator PlayerSingleAttackSkill( int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(AttackCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        
        yield return runningCoroutine = StartCoroutine(singleAttack.SingleAllySkill(tmpAnima, selectEnemy, skillNum, matchedSkill[0].Weight));
           
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());  
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerMultiSkill(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(MultiAttackCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima  = PresentAllyTurn();

        yield return runningCoroutine = StartCoroutine(multipleAttack.MultiAllySkill(tmpAnima, skillNum, matchedSkill[0].Weight));
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
        
    }
    IEnumerator PlayerSingleHeal(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(BuffCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(singleAttack.SingleAllyHeal(tmpAnima, selectEnemy, skillNum, matchedSkill[0].Weight));
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerMultiHeal(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(MultiBuffCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(multipleAttack.MultiAllyHeal(tmpAnima, skillNum, matchedSkill[0].Weight));
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerSingleShield(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(BuffCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(singleAttack.SingleAllyShield(tmpAnima, selectEnemy, skillNum, matchedSkill[0].Weight));
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerMultiShield(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(MultiBuffCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(multipleAttack.MultiAllyShield(tmpAnima, skillNum, matchedSkill[0].Weight));
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerSingleBuff(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(BuffCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(singleAttack.SingleAllyBuff(tmpAnima, selectEnemy, skillNum, matchedSkill[0].Weight));
        Buff buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[selectEnemy].animaData, 0);
        buffManager.AddOrRenuwBuff(buff);
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerMultiBuff(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(MultiBuffCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(multipleAttack.MultiAllyBuff(tmpAnima, skillNum, matchedSkill[0].Weight));
        Buff buff;
        for (int i = 0; i < allyActions.Count; i++)
        {
            if (allyActions[i].animaData.Animadie) continue;

            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[i].animaData, 0);
            buffManager.AddOrRenuwBuff(buff);
        }
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    
    IEnumerator PlayerSingleDebuff(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(AttackCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(singleAttack.SingleAllyDebuff(tmpAnima, selectEnemy, skillNum, matchedSkill[0].Weight));
        Buff buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
        buffManager.AddOrRenuwBuff(buff);
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator PlayerMultiDebuff(int skillNum)
    {
        yield return null;
        yield return runningCoroutine = StartCoroutine(MultiAttackCursorInit());
        if (runningCoroutine == null) yield break;
        tmpAnima = PresentAllyTurn();
        yield return runningCoroutine = StartCoroutine(multipleAttack.MultiAllyDebuff(tmpAnima, skillNum, matchedSkill[0].Weight));
        Buff buff;
        for (int i = 0; i < enemyActions.Count; i++)
        {
            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[i].animaData, 1);
            buffManager.AddOrRenuwBuff(buff);
        }
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = StartCoroutine(BattleStart());
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }
    }
    IEnumerator EnemyTurn()
    {
        int selectAlly = selectNoDieAnima();
        selectEnemy = Random.Range(0, enemyActions.Count);
        foreach (EnemyActions enemy in enemyActions)
        {
            if (turnList.Count == 0)
            {
                break;
            }
            if (ReferenceEquals(turnList[0], enemy.animaData))
            {
                enemy.DecideAction();
                if (enemy.performance.Equals("Attack"))
                {
                    yield return runningCoroutine = StartCoroutine(singleAttack.SingleEnemyAttack(enemy, selectAlly));
                }
                else if (enemy.performance.Equals("Skill"))
                {
                    matchedSkill = skills.Where(s => s.name == enemy.animaData.skillName[0]).ToList();
                    Buff buff;
                    switch (matchedSkill[0].Type)
                    {
                        case "SingleAttack":
                            yield return runningCoroutine = StartCoroutine(singleAttack.SingleEnemySkill(enemy, selectAlly, matchedSkill[0].Weight));
                            break;
                        case "SingleHeal":
                            yield return runningCoroutine = StartCoroutine(singleAttack.SingleEnemyHeal(enemy, selectEnemy, matchedSkill[0].Weight));
                            break;
                        case "SingleBuff":
                            yield return runningCoroutine = StartCoroutine(singleAttack.SingleEnemyBuff(enemy, selectEnemy, matchedSkill[0].Weight));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 0);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "SingleDebuff":
                            yield return runningCoroutine = StartCoroutine(singleAttack.SingleEnemyDebuff(enemy, selectAlly, matchedSkill[0].Weight));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[selectAlly].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "MultiAttack":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemySkill(enemy, matchedSkill[0].Weight));
                            break;
                        case "MultiHeal":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemyHeal(enemy, matchedSkill[0].Weight));
                            break;
                        case "MultiBuff":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemyBuff(enemy, matchedSkill[0].Weight));
                            for (int i = 0; i < enemyActions.Count; i++)
                            {
                                buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[i].animaData, 0);
                                buffManager.AddOrRenuwBuff(buff);
                            }
                            break;
                        case "MultiDebuff":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.MultiEnemyDebuff(enemy, matchedSkill[0].Weight));
                            for (int i = 0; i < allyActions.Count; i++)
                            {
                                if (allyActions[i].animaData.Animadie) continue;
                                buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[i].animaData, 1);
                                buffManager.AddOrRenuwBuff(buff);
                            }
                            break;
                    }
                    break;
                }
                else if (enemy.performance.Equals("BossAttack"))
                {
                    matchedSkill = skills.Where(s => s.name == enemy.animaData.skillName[0]).ToList();
                    Buff buff;
                    switch (matchedSkill[0].Type)
                    {
                        case "FelixSingle":
                            yield return runningCoroutine = StartCoroutine(singleAttack.FelixSingleAttack(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "PhobiaSingle":
                            yield return runningCoroutine = StartCoroutine(singleAttack.PhobiaSingleAttack(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "LacrimaSingle":
                            yield return runningCoroutine = StartCoroutine(singleAttack.LacrimaSingleAttack(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "AmareSingle":
                            yield return runningCoroutine = StartCoroutine(singleAttack.AmareSingleAttack(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "IrascorSingle":
                            yield return runningCoroutine = StartCoroutine(singleAttack.IrascorSingleAttack(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "HavetSingle":
                            yield return runningCoroutine = StartCoroutine(singleAttack.HavetSingleAttack(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData, 1);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                    }
                }
                else if (enemy.performance.Equals("BossSkill"))
                {
                    matchedSkill = skills.Where(s => s.name == enemy.animaData.skillName[1]).ToList();
                    //Buff buff;
                    switch (matchedSkill[1].Type)
                    {
                        case "FelixMulti":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.FelixMultiSkill(enemy, matchedSkill[1].Weight));
                            //buff = new Buff(matchedSkill[1].Affect, matchedSkill[1].Weight, matchedSkill[1].Turn, enemyActions[selectEnemy].animaData, 1);
                            //buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "PhobiaMulti":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.PhobiaMultiSkill(enemy, matchedSkill[1].Weight));
                            //buff = new Buff(matchedSkill[1].Affect, matchedSkill[1].Weight, matchedSkill[1].Turn, enemyActions[selectEnemy].animaData, 1);
                            //buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "LacrimaMulti":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.LacrimaMultiSkill(enemy, matchedSkill[1].Weight));
                            //buff = new Buff(matchedSkill[1].Affect, matchedSkill[1].Weight, matchedSkill[1].Turn, enemyActions[selectEnemy].animaData, 1);
                            //buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "AmareMulti":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.AmareMultiSkill(enemy, matchedSkill[1].Weight));
                            //buff = new Buff(matchedSkill[1].Affect, matchedSkill[1].Weight, matchedSkill[1].Turn, enemyActions[selectEnemy].animaData, 1);
                            //buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "IrascorMulti":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.IrascorMultiSkill(enemy, matchedSkill[1].Weight));
                            //buff = new Buff(matchedSkill[1].Affect, matchedSkill[1].Weight, matchedSkill[1].Turn, enemyActions[selectEnemy].animaData, 1);
                            //buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "HavetMulti":
                            yield return runningCoroutine = StartCoroutine(multipleAttack.HavetMultiSkill(enemy, matchedSkill[1].Weight));
                            //buff = new Buff(matchedSkill[1].Affect, matchedSkill[1].Weight, matchedSkill[1].Turn, enemyActions[selectEnemy].animaData, 1);
                            //buffManager.AddOrRenuwBuff(buff);
                            break;
                    }
                }

            }
        }
            runningCoroutine = null;
        if (isDefeat)
        {
            if(GameObject.Find("Game Over UI(Clone)") == null)
            {
                LoseBattle();
            }
            yield break;
        }
            if (turnList.Count == 0)
            {
                runningCoroutine = StartCoroutine(BattleStart());
            }
            else
            {
                SetState(turnList);
            }
        
    }
    public int selectNoDieAnima()
    {
        int randomNumber;
        do
        {
            randomNumber = UnityEngine.Random.Range(0, allyActions.Count);
        } while (dieAllyAnima.Contains(randomNumber));
        return randomNumber;
    }
    
    public List<AnimaActions> GetAllyActions(){ return allyActions; }
    public List<EnemyActions> GetEnemyActions(){ return enemyActions; }
    
    
    public void WinBattle()
    {
        cleared = true;
        if (!isBoss)
        {
            AudioManager.Instance.PlaySFX(winClip);
            Instantiate(Resources.Load<GameObject>("Minwoo/Battle Win UI"), canvas.transform);
            BuffDelete();
            for (int i = 0; i < allyActions.Count; i++)
            {
                GameObject animaImage = GameObject.Find("Entry Anima List").transform.Find($"Anima {i}").gameObject;
                animaImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + allyActions[i].animaData.Objectfile);
                animaImage.SetActive(true);
            }
            for (int i = 0; i < dropAnima.Count; i++)
            {
                GameObject dropAnimaImage = GameObject.Find("Drop Anima List").transform.Find($"Anima {i}").gameObject;
                dropAnimaImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Anima_Sprites/" + dropAnima[i].Objectfile);
                dropAnimaImage.SetActive(true);
            }
        }
        else if (isBoss && bossStage != 6)
        {
            Instantiate(Resources.Load<GameObject>("Minwoo/Boss Battle Win UI"), canvas.transform);
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("Minwoo/Game Clear UI"), canvas.transform);
        }


    }
    public void LoseBattle()
    {
        AudioManager.Instance.PlaySFX(loseClip);
        Instantiate(Resources.Load<GameObject>("Minwoo/Game Over UI"), canvas.transform);
    }
    AnimaActions PresentAllyTurn()
    {
        foreach (AnimaActions anima in allyActions)
        {
            if (TurnList.Count == 0)
            {
                runningCoroutine = StartCoroutine(BattleStart());
            }
            if (ReferenceEquals(TurnList[0], anima.animaData))
            {
                return anima;
            }
        }
        return null;
    }
    IEnumerator AttackCursorInit()
    {
        yield return null;
        AudioManager.Instance.PlaySFX(btnClip);
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        index = 0;
        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(enemyBattleSetting.EnemyInstance[index].transform.position.x, enemyBattleSetting.EnemyInstance[index].transform.position.y + 1.2f), Quaternion.identity);
        arrow = GameObject.Find("Arrow_down(Clone)");
        while (true)
        {
            if (index != 2 && index < (enemyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                index++;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.EnemyInstance[index].transform.position.x, enemyBattleSetting.EnemyInstance[index].transform.position.y + 1.2f);
            }
            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                index--;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.EnemyInstance[index].transform.position.x, enemyBattleSetting.EnemyInstance[index].transform.position.y + 1.2f);
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                selectEnemy = index;
                DestroyImmediate(arrow);
                yield return new WaitForSeconds(1f);
                break;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                DestroyImmediate(arrow);
                SetState(turnList);
                if(runningCoroutine != null )StopCoroutine(runningCoroutine);
                runningCoroutine = null;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator MultiAttackCursorInit()
    {
        yield return null;
        AudioManager.Instance.PlaySFX(btnClip);
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        List<GameObject> tmp = new List<GameObject>();
        for(int i = 0; i < enemyActions.Count; i++)
        {
            tmp.Add(Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(enemyBattleSetting.EnemyInstance[i].transform.position.x, enemyBattleSetting.EnemyInstance[i].transform.position.y + 1.2f), Quaternion.identity));
        }
        
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Z) && !attackButton.interactable)
            {
                AudioManager.Instance.PlaySFX(btnClip);
                selectEnemy = index;
                for(int i = 0; i < tmp.Count;)
                {
                    DestroyImmediate(tmp[i]);
                    tmp.RemoveAt(i);
                }
                yield return new WaitForSeconds(1f);
                break;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                foreach (var aa in tmp)
                {
                    DestroyImmediate(aa);
                }

                SetState(turnList);
                if (runningCoroutine != null) StopCoroutine(runningCoroutine);
                runningCoroutine = null;
                break;
            }
            yield return null;
        }
    }
    IEnumerator BuffCursorInit()
    {
        yield return null;
        AudioManager.Instance.PlaySFX(btnClip);
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        index = 0;
        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(allyBattleSetting.AllyInstance[index].transform.position.x, allyBattleSetting.AllyInstance[index].transform.position.y + 1.2f), Quaternion.identity);
        arrow = GameObject.Find("Arrow_down(Clone)");
        while (true)
        {
            if (index != 2 && index < (allyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                index++;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(allyBattleSetting.AllyInstance[index].transform.position.x, allyBattleSetting.AllyInstance[index].transform.position.y + 1.2f);
            }
            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                index--;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(allyBattleSetting.AllyInstance[index].transform.position.x, allyBattleSetting.AllyInstance[index].transform.position.y + 1.2f);
            }
            else if (Input.GetKeyUp(KeyCode.Z) && !attackButton.interactable)
            {
                AudioManager.Instance.PlaySFX(btnClip);
                selectEnemy = index;
                DestroyImmediate(arrow);
                yield return new WaitForSeconds(1f);
                break;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                DestroyImmediate(arrow);
                
                SetState(turnList);
                if (runningCoroutine != null) StopCoroutine(runningCoroutine);
                runningCoroutine = null;
                break;
            }
            yield return null;
        }
    }
    IEnumerator MultiBuffCursorInit()
    {
        yield return null;
        AudioManager.Instance.PlaySFX(btnClip);
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        List<GameObject> tmp = new List<GameObject>();
        for(int i = 0; i < allyActions.Count; i++)
        {
            if (allyActions[i].animaData.Animadie) continue;
            tmp.Add(Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(allyBattleSetting.AllyInstance[i].transform.position.x, allyBattleSetting.AllyInstance[i].transform.position.y + 1.2f), Quaternion.identity));
        }
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Z) && !attackButton.interactable)
            {
                for (int i = 0; i < tmp.Count;)
                {
                    AudioManager.Instance.PlaySFX(btnClip);
                    DestroyImmediate(tmp[i]);
                    tmp.RemoveAt(i);
                }
                yield return new WaitForSeconds(1f);
                break;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                AudioManager.Instance.PlaySFX(btnClip);
                DestroyImmediate(arrow);
                SetState(turnList);
                if (runningCoroutine != null) StopCoroutine(runningCoroutine);
                runningCoroutine = null;
                break;
            }
            yield return null;
        }
    }
    public bool TombAlive()
    {
        for (int i = 0; i < enemyActions.Count; i++)
        {
            if (enemyActions[i].animaData.isTomb) return true;
        }
        return false;
    }
    private void BuffDelete()
    {
        List<string > buffList = new List<string>();
        foreach(var ally in AllyActions)
        {
            buffList = buffManager.TickClear(ally.animaData);
            while (buffList.Count > 0)
            {
                switch (buffList[0])
                {
                    case "strength":
                        ally.animaData.Damage = ally.animaData.tmpAbility["strength"];
                        break;
                    case "speed":
                        ally.animaData.Speed = ally.animaData.tmpAbility["speed"];
                        break;
                    case "defense":
                        ally.animaData.Defense = ally.animaData.tmpAbility["defense"];
                        break;
                    }
                    buffList.RemoveAt(0);
                }
            }
    }
}



