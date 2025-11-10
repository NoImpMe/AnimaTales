using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

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
    private List<string> tileType = new List<string> {"Amare", "Felix", "Havet","Irascor","Lacrima","Phobia"};
    [SerializeField] RegionManager regionPrefab;
    [SerializeField]
    private FadeEffect fadePanel;

    public void StageInit(int stageNum)
    {
        int randomSelectTile = Random.Range(0, 1);
        cameraSet = GameObject.Find("Main Camera");
        var camSet = cameraSet.GetComponent<CameraController>();
        if (randomSelectTile == 0)
        {
            tileMap = Resources.Load<GameObject>($"Minwoo/TileMap/Stage{stageNum}");
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
        fadePanel = GameObject.Find("FadePanel").GetComponent<FadeEffect>();
        StartCoroutine(EnterBattle(target));
    }

    IEnumerator EnterBattle(RegionController target)
    {
        if (target.isCleared && !target.isVillaged)
        {
            yield return null;
        }
        else if (target.isVillaged)
        {
            manager.SetTile(target);
            currentRegion = target;
            string villageID = target.name;
            VillageDataManager.Instance.SetCurrentVillageID(villageID);
            yield return StartCoroutine(fadePanel.LoadSceneWithFade("VillageScene"));
            
        }
        else if (target.name.StartsWith("EliteBattle"))
        {
            manager.SetTile(target);
            target.isCleared = true;
            currentRegion = target;
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
            manager.SetTile(target);
            target.isCleared = true;
            currentRegion = target;
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
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("IrascorBossBattleScene"));
                    break;
                case "Lacrima":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("LacrimaBossBattleScene"));
                    break;
                case "Havet":
                    yield return StartCoroutine(fadePanel.LoadSceneWithFade("HavetBossBattleScene"));
                    break;
            }
            target.GetComponentInParent<IsVisitedField>().isVisited = true;
            target.transform.parent.parent.GetComponent<StageController>().ShowNextField();
        }
        else if (target.name.StartsWith("Start"))
        {
            manager.SetTile(target);
            target.isCleared = true;
            currentRegion = target;
            target.GetComponentInParent<IsVisitedField>().isSelected = true;
            target.transform.parent.parent.GetComponent<StageController>().EnterNewField();
            target.GetComponentInParent<IsVisitedField>().isVisited = true;
            SetNextTile(target);
        }
        else
        {
            manager.SetTile(target);
            target.isCleared = true;
            currentRegion = target;
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
