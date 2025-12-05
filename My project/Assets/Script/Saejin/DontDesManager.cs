using BansheeGz.BGDatabase;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDesManager : MonoBehaviour
{
    public static DontDesManager Instance { get; private set; }
    [SerializeField] GameObject regionManager;
    public GameObject manager;
    public GameObject mapScreen;
    public GameObject grid;
    private GameObject tileManager;
    private string lastUnloaded;
    private Camera tileCam;
    public bool tutoCleared = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(manager);
            var database = BGRepo.I;
            var meta = database.GetMeta("GoldData");
            meta.ForEachEntity(e => { tutoCleared = e.Get<bool>("TutoCleared");});
            if (!tutoCleared)
            {
                regionManager.GetComponent<RegionManager>().StageInit(999);
            }
            else
            {
                regionManager.GetComponent<RegionManager>().StageInit(0);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScene")
        {
            Destroy(manager);
            Destroy(this);
            Destroy(regionManager);
            Destroy(gameObject);
            Destroy(grid);
            Destroy(tileManager);
            return;
        }

        if (lastUnloaded != null && (lastUnloaded.EndsWith("BattleScene") || lastUnloaded.Equals("VillageScene"))) 
        {
            if (grid == null)
            {
                var tilePrefab = GameObject.Find("Tiles");
                if (tilePrefab != null)
                {
                    grid = tilePrefab;
                    DontDestroyOnLoad(grid);
                }
            }
            else
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    if (root.name == "Tiles" && root != grid)
                    {
                        Destroy(root);
                        break;
                    }
                }
            }

            if (tileManager == null)
            {
                var tmPrefab = GameObject.Find("RegionManager");
                if (tmPrefab != null)
                {
                    tileManager = tmPrefab;
                    DontDestroyOnLoad(tileManager);
                }
            }
            else
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    if (root.name == "RegionManager" && root != tileManager)
                    {
                        Destroy(root);
                        break;
                    }
                }
            }
            var cam = GameObject.Find("Main Camera");
            tileCam = cam.GetComponent<Camera>();
            tileCam.transform.position = new Vector3(RegionManager.Instance.wp.x, RegionManager.Instance.wp.y, -10f);
            grid.SetActive(true);
        }
        if (scene.name.EndsWith("BattleScene"))
        {
            grid.SetActive(false);
        }
        if (scene.name.StartsWith("Village"))
        {
            grid.SetActive(false);
        }
    }
    public void OnSceneUnloaded(Scene scene)
    {
        lastUnloaded = scene.name;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void setDesGrid()
    {
        grid = GameObject.Find("Tiles");
        DontDestroyOnLoad(grid);
    }
    public void TutorialClear()
    {
        var database = BGRepo.I;
        var meta = database.GetMeta("GoldData");
        meta.ForEachEntity(e => { 
            e.Set<bool>("TutoCleared", true);
            DBUpdater.Save();
        });
        

    }
}
