using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using BansheeGz.BGDatabase;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private string villageID = "Village_01";
    
    [Header("피드백 UI")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDisplayTime = 2f;
    
    private ShopList shopList;
    private VillageShopState shopState;
    private Dictionary<string, int> remainingCounts = new Dictionary<string, int>();
    private Coroutine feedbackCoroutine;
    [SerializeField] private AudioClip itemClip;
    Coroutine runningCoroutine;
    
    private void Start()
    {
        villageID = VillageDataManager.Instance.GetCurrentVillageID();

        shopList = ShopList.CreateForVillage(villageID);
        shopState = VillageDataManager.Instance.GetVillageShopState(villageID, shopList.GetAllItems());

        foreach (var item in shopList.GetAllItems())
        {
            remainingCounts[item.itemID] = shopState.GetRemainingCount(item.itemID);
        }

        ShopUIManager.Instance.ShowShopItems(shopList.GetAllItems(), remainingCounts, OnItemPurchase);

        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }
    }
    
    private void OnItemPurchase(ShopItemData item)
    {
        switch (item.targetType)
        {
            case TargetType.Single:
                ShopAnimaSelectUI.Instance.ShowUI(item, OnAnimaSelected);
                break;
            
            case TargetType.None:
                StartCoroutine(ProcessItemPurchase(item, result =>
                {
                    if (result)
                    {
                        int randomNum = UnityEngine.Random.Range(0, 102);
                        ShowFeedback($"{randomNum}번 교감의 두루마리를 얻었습니다.");
                        ShopEffectHandler.ApplyEffect(item);
                        AudioManager.Instance.PlaySFX(itemClip);
                    }
                }));
                break;
            case TargetType.All:
                StartCoroutine(ProcessItemPurchase(item, result =>
                {
                    if (result)
                    {
                        ShopEffectHandler.ApplyEffect(item);
                        AudioManager.Instance.PlaySFX(itemClip);
                        ShowFeedback($"'{item.itemName}'을(를)\n사용했습니다.");
                    }
                }));
                break;
        }
    }
    
    private void OnAnimaSelected(ShopItemData item, AnimaDataSO selectedAnima)
    {
        StartCoroutine(ProcessItemPurchase(item, result =>
        {
            if (result)
            {
                ShopEffectHandler.ApplyEffect(item, selectedAnima);
                ShowFeedback($"'{item.itemName}'을(를)\n'{selectedAnima.Name}'에게\n사용했습니다.");
            }
        }));
    }

    private IEnumerator ProcessItemPurchase(ShopItemData item, System.Action<bool> callback)
    {
        if (!VillageDataManager.Instance.PurchaseItem(villageID, item.itemID))
        {
            ShowFeedback("품절 상태입니다.");
            callback?.Invoke(false);
            yield break;
        }
        if (GoldManager.Instance.GetCurrentGold() < item.price)
        {
            ShowFeedback($"골드가 부족합니다.");
            callback?.Invoke(false);
            yield break;
        }
        yield return StartCoroutine(SucessPurchase(item));
        callback?.Invoke(true);
    }
    IEnumerator SucessPurchase(ShopItemData item)
    {
        yield return StartCoroutine(GoldManager.Instance.SpendGold(item.price));
        remainingCounts[item.itemID] = shopState.GetRemainingCount(item.itemID);
        ShopUIManager.Instance.UpdateItemSlot(item.itemID, remainingCounts[item.itemID]);
    }
    private void ShowFeedback(string message)
    {
        if (feedbackPanel == null || feedbackText == null)
        {
            return;
        }
        
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
        yield return new WaitForSeconds(feedbackDisplayTime);
        feedbackPanel.SetActive(false);
        feedbackCoroutine = null;
    }
    
    public void SetVillageID(string newVillageID)
    {
        villageID = newVillageID;
        
        if (shopList != null)
        {
            shopList = ShopList.CreateForVillage(villageID);
            shopState = VillageDataManager.Instance.GetVillageShopState(villageID, shopList.GetAllItems());
            
            foreach (var item in shopList.GetAllItems())
            {
                remainingCounts[item.itemID] = shopState.GetRemainingCount(item.itemID);
            }
            
            ShopUIManager.Instance.ShowShopItems(shopList.GetAllItems(), remainingCounts, OnItemPurchase);
        }
    }
}