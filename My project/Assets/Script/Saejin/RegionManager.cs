using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RegionManager : MonoBehaviour
{
    public RegionController startRegion;
    public int stageType;
    public GameObject tileMap;
    public GameObject cameraRegion;
    public GameObject cameraSet;
    private RegionController currentRegion;
    private Vector3 pointerDownPos;
    private bool isDragging = false;
    public float dragThreshold = 10f;
    public GameObject enterTileManager;
    public EnterTiles enterTiles;
    private GameObject managerOB;
    private DontDesManager manager;

    [SerializeField] RegionManager regionPrefab;

    void Start()
    {
        int randomSelectTile = Random.Range(0, 1);
        cameraSet = GameObject.Find("Main Camera");
        var camSet = cameraSet.GetComponent<CameraController>();
        if (randomSelectTile == 0)
        {
            tileMap = Resources.Load<GameObject>("Minwoo/TileMap/Stage0");
            stageType = 0;
            GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
            map.name = "Tiles";
            startRegion = map.GetComponentInChildren<RegionController>();
            camSet.setMaxMin();
        }
        foreach (var reg in Object.FindObjectsByType<RegionController>(FindObjectsSortMode.None))
        {
            reg.gameObject.SetActive(reg == startRegion);
        }
        currentRegion = startRegion;
        var tile = GameObject.Find("StartTile").GetComponent<RegionController>();
        SetNextTile(tile);
        managerOB = GameObject.Find("DontDesManager");
        manager = managerOB.GetComponent<DontDesManager>();
        manager.setDesGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerDownPos = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!isDragging &&
                Vector3.Distance(pointerDownPos, Input.mousePosition) > dragThreshold)
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleClick();
            }
        }
    }

    void HandleClick()
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(wp, Vector2.zero);
        if (hit.collider == null) return;

        var target = hit.collider.GetComponentInParent<RegionController>();
        if (target == null || !target.gameObject.activeSelf) return;

        EnterBattle(target);
    }

    void EnterBattle(RegionController target)
    {
        if (target.isCleared && !target.isVillaged)
        {
            return;
        }
        else if (target.isVillaged)
        {
            string villageID = target.name;
            VillageDataManager.Instance.SetCurrentVillageID(villageID);
            SceneManager.LoadScene("VillageScene");
        }
        else if (target.name.StartsWith("EliteBattle"))
        {
            switch (target.type)
            {
                case "Felix":
                    SceneManager.LoadScene("FelixEliteBattleScene");
                    break;
                case "Phobia":
                    SceneManager.LoadScene("PhobiaEliteBattleScene");
                    break;
                case "Amare":
                    SceneManager.LoadScene("AmareEliteBattleScene");
                    break;
                case "Irascor":
                    SceneManager.LoadScene("IrascorEliteBattleScene");
                    break;
                case "Lacrima":
                    SceneManager.LoadScene("LacrimaEliteBattleScene");
                    break;
                case "Havet":
                    SceneManager.LoadScene("HavetEliteBattleScene");
                    break;
            }
        }
        else if (target.name.StartsWith("Boss"))
        {
            switch (target.type)
            {
                case "Felix":
                    SceneManager.LoadScene("FelixBossBattleScene");
                    break;
                case "Phobia":
                    SceneManager.LoadScene("PhobiaBossBattleScene");
                    break;
                case "Amare":
                    SceneManager.LoadScene("AmareBossBattleScene");
                    break;
                case "Irascor":
                    SceneManager.LoadScene("IrascorBossBattleScene");
                    break;
                case "Lacrima":
                    SceneManager.LoadScene("LacrimaBossBattleScene");
                    break;
                case "Havet":
                    SceneManager.LoadScene("HavetBossBattleScene");
                    break;
            }
        }
        else
        {
            switch (target.type)
            {
                case "Felix":
                    SceneManager.LoadScene("FelixBattleScene");
                    break;
                case "Phobia":
                    SceneManager.LoadScene("PhobiaBattleScene");
                    break;
                case "Amare":
                    SceneManager.LoadScene("AmareBattleScene");
                    break;
                case "Irascor":
                    SceneManager.LoadScene("IrascorBattleScene");
                    break;
                case "Lacrima":
                    SceneManager.LoadScene("LacrimaBattleScene");
                    break;
                case "Havet":
                    SceneManager.LoadScene("HavetBattleScene");
                    break;
            }
        }
        manager.SetTile(target);
        target.isCleared = true;
        currentRegion = target;
    }

    public void SetNextTile(RegionController target)
    {
        foreach (var nb in target.neighbors)
        {
            
            nb.gameObject.SetActive(true);
        }
    }
}
