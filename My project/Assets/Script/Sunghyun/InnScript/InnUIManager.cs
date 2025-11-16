using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InnUIManager : MonoBehaviour
{
    [Header("UI 요소")]
    [SerializeField] private GameObject innPanel;
    [SerializeField] private TextMeshProUGUI currentPriceText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private Button useInnButton;
    [SerializeField] private Button closeButton;
    
    [Header("피드백 UI")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDuration = 2f;
    
    private InnManager innManager;
    private Coroutine feedbackCoroutine;
    
    private void Awake()
    {
        if (innPanel != null)
            innPanel.SetActive(false);
            
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
            
        if (useInnButton != null)
            useInnButton.onClick.AddListener(OnUseButtonClick);
            
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseInnPanel);
    }
    
    private void Start()
    {
        innManager = InnManager.Instance;
        
        if (innManager != null)
        {
            innManager.OnInnUsed += OnInnUsedSuccessfully;
        }
    }
    
    private void OnDestroy()
    {
        if (innManager != null)
        {
            innManager.OnInnUsed -= OnInnUsedSuccessfully;
        }
        
        if (useInnButton != null)
            useInnButton.onClick.RemoveListener(OnUseButtonClick);
            
        if (closeButton != null)
            closeButton.onClick.RemoveListener(CloseInnPanel);
    }
    
    public void OpenInnPanel()
    {
        if (innPanel != null)
        {
            innPanel.SetActive(true);
            UpdatePriceUI();
            UpdateGoldUI();
            
        }
    }
    
    public void CloseInnPanel()
    {
        if (innPanel != null)
        {
            innPanel.SetActive(false);
           
        }
    }
    
    
    private void UpdatePriceUI()
    {
        if (currentPriceText != null && innManager != null)
        {
            int price = innManager.GetCurrentPrice();
            currentPriceText.text = $"{price:N0}";
        }
    }
    
    private void UpdateGoldUI()
    {
        if (playerGoldText != null && GoldManager.Instance != null)
        {
            int gold = GoldManager.Instance.GetCurrentGold();
            playerGoldText.text = $"{gold:N0}";
            
            if (useInnButton != null && innManager != null)
            {
                useInnButton.interactable = gold >= innManager.GetCurrentPrice();
            }
        }
    }
    
    private void OnUseButtonClick()
    {
        if (innManager == null) return;
        
        bool success = innManager.UseInn();
        
        if (!success)
        {
            ShowFeedback("골드가 부족합니다.");
        }
    }
    
    private void OnInnUsedSuccessfully()
    {
        ShowFeedback("모든 아니마가 회복되었습니다.");
        UpdatePriceUI();
        UpdateGoldUI();
    }
    
    private void ShowFeedback(string message)
    {
        if (feedbackPanel == null || feedbackText == null) return;
        
        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
        }
        
        feedbackText.text = message;
        feedbackPanel.SetActive(true);
        
        feedbackCoroutine = StartCoroutine(HideFeedbackAfterDelay());
    }
    
    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDuration);
        
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }
        
        feedbackCoroutine = null;
    }
}