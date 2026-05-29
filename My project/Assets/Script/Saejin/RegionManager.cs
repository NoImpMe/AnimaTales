using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RegionManager : MonoBehaviour
{
    public static RegionManager Instance;
    public RegionController startRegion;
    public int stageNum;
    public GameObject tileMap;
    public GameObject cameraRegion;
    public GameObject cameraSet;
    public int currentStageType;
    public float dragThreshold = 10f;
    public GameObject enterTileManager;
    private GameObject managerOB;
    private DontDesManager manager;
    private List<string> tileType = new List<string> {"Amare", "Felix", "Havet","Irascor","Lacrima","Phobia"};
    [SerializeField] RegionManager regionPrefab;
    [SerializeField]
    private FadeEffect fadePanel;
    [SerializeField]
    private List<AudioClip> bgmClips;
    private int count = 0;
    public bool isClicked = false;
    public Vector2 wp;
    public GameObject errorPanel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void StageInit(int stageNum)
    {
        cameraSet = GameObject.Find("Main Cam");
        var camSet = cameraSet.GetComponent<CameraController>();
        tileMap = Resources.Load<GameObject>($"Minwoo/TileMap/Stage{stageNum}");
        this.stageNum = stageNum;
        GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
        map.name = "Tiles";
        startRegion = map.GetComponentInChildren<RegionController>();
        camSet.setMaxMin();

        foreach (var reg in Object.FindObjectsByType<RegionController>(FindObjectsSortMode.None))
        {
            reg.gameObject.SetActive(reg == startRegion);
        }
        var tile = GameObject.Find("StartTile").GetComponent<RegionController>();
        string tmp = tile.transform.parent.name;
        for (int i = 0; i < tileType.Count; i++) 
        {
            if (tmp == tileType[i])
            {
                AudioManager.Instance.PlayBGM(bgmClips[i]);
                currentStageType = i;
                break;
            }
        }
        SetNextTile(tile);
        managerOB = GameObject.Find("DontDesManager");
        manager = managerOB.GetComponent<DontDesManager>();
        manager.setDesGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& SceneManager.GetActiveScene().name == "Stage0Scene")
        {
            if (!isClicked && !EventSystem.current.IsPointerOverGameObject())
            {
                isClicked = true;
                wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(wp, Vector2.zero);
                if (hit.collider == null || !hit.collider.enabled)
                {
                    isClicked = false;
                    return;
                }
                var target = hit.collider.GetComponentInParent<RegionController>();
                if (target == null || !target.gameObject.activeSelf)
                {
                    isClicked = false;
                    return;
                }
                fadePanel = GameObject.Find("FadePanel").GetComponent<FadeEffect>();
                StartCoroutine(EnterBattle(target));
                isClicked = false;
            }
        }
    }

    public IEnumerator EnterBattle(RegionController target)
    {
        
        if(!target.isVillaged && AnimaInventoryManager.Instance.playerInfo.battleAnima.Count <= 0)
        {
            errorPanel.SetActive(true);
        }
        if (target.isVillaged)
        {
            string villageID = target.name;
            VillageDataManager.Instance.SetCurrentVillageID(villageID);
            SetNextTile(target);
            if (target.type == "Tutorial")
            {
                yield return StartCoroutine(fadePanel.LoadSceneWithFade("TutorialVillageScene"));
            }
            else yield return StartCoroutine(fadePanel.LoadSceneWithFade("VillageScene"));
        }
        else if (target.name.StartsWith("EliteBattle"))
        {
            target.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
            
            var targetColor = target.gameObject.GetComponent<Tilemap>().color;
            targetColor.a = 0.37f;
            target.gameObject.GetComponent<Tilemap>().color = targetColor;
            SetNextTile(target);

            switch (target.type)
            {
                case "Felix":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("FelixEliteBattleScene"));
                    break;
                case "Phobia":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("PhobiaEliteBattleScene"));
                    break;
                case "Amare":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("AmareEliteBattleScene"));
                    break;
                case "Irascor":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("IrascorEliteBattleScene"));
                    break;
                case "Lacrima":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("LacrimaEliteBattleScene"));
                    break;
                case "Havet":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("HavetEliteBattleScene"));
                    break;
            }
        }
        else if (target.name.StartsWith("Boss"))
        {
            target.gameObject.GetComponent<TilemapCollider2D>().enabled = false;

            var targetColor = target.gameObject.GetComponent<Tilemap>().color;
            targetColor.a = 0.37f;
            target.gameObject.GetComponent<Tilemap>().color = targetColor;
            switch (target.type)
            {
                case "Felix":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("FelixBossBattleScene"));
                    break;
                case "Phobia":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("PhobiaBossBattleScene"));
                    break;
                case "Amare":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("AmareBossBattleScene"));
                    break;
                case "Irascor":
                    count += 1;
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("IrascorBossBattleScene"));
                    break;
                case "Lacrima":
                    count += 1;
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("LacrimaBossBattleScene"));
                    break;
                case "Havet":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("HavetBossBattleScene"));
                    break;
            }
            target.GetComponentInParent<IsVisitedField>().isVisited = true;
            target.transform.parent.parent.GetComponent<StageController>().ShowNextField();
            if(count == 2)
            {
                target.transform.parent.parent.GetComponent<StageController>().ShowLastField();
            }
        }
        else if (target.name.StartsWith("Start"))
        {
            if(target.type == "Phobia")
            {
                target.otherStartPoint.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
                target.otherStartPoint.GetComponentInParent<IsVisitedField>().isSelected = true;
                target.otherStartPoint.GetComponentInParent<IsVisitedField>().isVisited = true;
                target.otherStartPoint.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
            }
            target.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
            target.GetComponentInParent<IsVisitedField>().isSelected = true;
            target.transform.parent.parent.GetComponent<StageController>().EnterNewField();
            target.GetComponentInParent<IsVisitedField>().isVisited = true;
            string tmp = target.transform.parent.name;
            for (int i = 0; i < tileType.Count; i++)
            {
                if (tmp == tileType[i])
                {
                    AudioManager.Instance.PlayBGM(bgmClips[i]);
                    currentStageType = i;
                    break;
                }
            }
            SetNextTile(target);
        }
        else
        {
            target.gameObject.GetComponent<TilemapCollider2D>().enabled = false;

            var targetColor = target.gameObject.GetComponent<Tilemap>().color;
            targetColor.a = 0.37f;
            target.gameObject.GetComponent<Tilemap>().color = targetColor;
            SetNextTile(target);
            switch (target.type)
            {
                case "Felix":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("FelixBattleScene"));
                    break;
                case "Phobia":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("PhobiaBattleScene"));
                    break;
                case "Amare":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("AmareBattleScene"));
                    break;
                case "Irascor":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("IrascorBattleScene"));
                    break;
                case "Lacrima":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("LacrimaBattleScene"));
                    break;
                case "Havet":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("HavetBattleScene"));
                    break;
                case "Tutorial":
                    if (GameObject.Find("StageTutorialCanvas") != null) GameObject.Find("StageTutorialCanvas").SetActive(false);
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("TutorialBattleScene"));
                    break;
            }
        }

    }

    public void SetNextTile(RegionController target)
    {
        foreach (var nb in target.neighbors)
        {
            nb.gameObject.SetActive(true);
        }
    }
}
