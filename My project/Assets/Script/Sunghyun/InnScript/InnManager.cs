using UnityEngine;
using System;

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
    
    [Header("여관 설정")]
    [SerializeField] private int initialPrice = 300;
    [SerializeField] private int priceIncreaseAmount = 50;
    
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
    
    public bool UseInn()
    {
        if (!isInitialized)
        {
            InitializePrice();
        }
        
        if (GoldManager.Instance.GetCurrentGold() >= currentPrice)
        {
            GoldManager.Instance.SpendGold(currentPrice);
            InnEffectHandler.ApplyInnEffect();
            AudioManager.Instance.PlaySFX(innClip);
            IncreasePrice();
            
            OnInnUsed?.Invoke();
            
            return true;
        }
        
        return false;
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