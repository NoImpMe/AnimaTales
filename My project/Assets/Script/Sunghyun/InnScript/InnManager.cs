using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class InnManager : MonoBehaviour
{
    private static InnManager _instance;
    
    public static InnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("여관 매니저 인스턴스 없음");
            }
            return _instance;
        }
    }
    
    private int initialPrice = 1200;
    private int priceIncreaseAmount = 700;
    
    private int currentPrice;
    private VillageDataManager dataManager;
    private bool isInitialized = false;
    
    public event Action OnInnUsed;
    [SerializeField] AudioClip innClip;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }
    
    private void Start()
    {
        InitializePrice();
    }
    
    private void InitializePrice()
    {
        if (isInitialized) return;
        
        dataManager = VillageDataManager.Instance;
        
        if (dataManager != null)
        {
            currentPrice = dataManager.GetInnPrice();
            
            if (currentPrice < initialPrice)
            {
                currentPrice = initialPrice;
                dataManager.UpdateInnPrice(currentPrice);
            }
        }
        else
        {
            currentPrice = initialPrice;
        }
        
        isInitialized = true;
    }
    
    public int GetCurrentPrice()
    {
        if (!isInitialized)
        {
            InitializePrice();
        }
        
        return currentPrice;
    }
    
    public IEnumerator UseInn()
    {
        if (!isInitialized)
        {
            InitializePrice();
        }
        
        if (GoldManager.Instance.GetCurrentGold() >= currentPrice)
        {
            yield return StartCoroutine(GoldManager.Instance.SpendGold(currentPrice));
            InnEffectHandler.ApplyInnEffect();
            AudioManager.Instance.PlaySFX(innClip);
            IncreasePrice();
            OnInnUsed?.Invoke();
        }
    }
    
    private void IncreasePrice()
    {
        currentPrice += priceIncreaseAmount;
        
        if (dataManager != null)
        {
            dataManager.UpdateInnPrice(currentPrice);
        }
    }
    
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}