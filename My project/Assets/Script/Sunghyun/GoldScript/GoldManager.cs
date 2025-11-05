using BansheeGz.BGDatabase;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }
    
    private TextMeshProUGUI goldText;
    public TextMeshProUGUI GoldText
    {
        get => goldText;
        set => goldText = value;
    }

    [SerializeField] private string goldTextObjectName = "GoldText";
    [SerializeField] private string goldFormat = "{0:N0}";

    private BGRepo database;
    private BGMetaEntity goldTable;
    private BGEntity entity;
    private int currentGold;
    
    public event System.Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            database = BGRepo.I;
            goldTable = database.GetMeta("GoldData");
            entity = goldTable.FirstOrDefault(e => e.Get<string>("name").Equals("GoldData"));
            currentGold = entity.Get<int>("Gold");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindGoldTextInScene();
    }
    
    private void FindGoldTextInScene()
    {
        GameObject goldTextObj = GameObject.Find(goldTextObjectName);
        
        if (goldTextObj != null)
        {
            goldText = goldTextObj.GetComponent<TextMeshProUGUI>();
            if (goldText != null)
                UpdateGoldDisplay();
        }
        else
        {
            goldText = null;
        }
    }
    
    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        
        currentGold += amount;
        entity = goldTable.FirstOrDefault(e => e.Get<string>("name").Equals("GoldData"));
        entity.Set<int>("Gold", currentGold);
        OnGoldChanged?.Invoke(currentGold);
    }
    
    public IEnumerator SpendGold(int amount)
    {
        float elapsed = 0f;
        float duration = 1f;
        int resultGold = currentGold - amount;
        float value;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            value = Mathf.Lerp(currentGold, resultGold, t);
            goldText.text = string.Format(goldFormat, value);
            yield return null;
        }
        goldText.text = string.Format(goldFormat, resultGold);
        entity = goldTable.FirstOrDefault(e => e.Get<string>("name").Equals("GoldData"));
        entity.Set<int>("Gold", currentGold);
        OnGoldChanged?.Invoke(currentGold);
        
    }
    public int GetCurrentGold()
    {
        return currentGold;
    }
    
    private void UpdateGoldDisplay()
    {
        if (goldText != null)
            goldText.text = string.Format(goldFormat, currentGold);
    }
}