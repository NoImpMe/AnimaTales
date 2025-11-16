using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopAnimaSelectUI : MonoBehaviour
{
    public static ShopAnimaSelectUI Instance { get; private set; }
    
    [Header("UI 요소")]
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private Transform animaListContent;
    [SerializeField] private GameObject animaSlotPrefab;
    [SerializeField] private TextMeshProUGUI emptyListText;
    [SerializeField] private Button closeButton;
    
    private ShopItemData currentItem;
    private Action<ShopItemData, AnimaDataSO> onSelectCallback;
    
    private void Awake()
    {
        Instance = this;
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
        
        if (selectPanel != null)
        {
            selectPanel.SetActive(false);
        }
    }
    
    public void ShowUI(ShopItemData item, Action<ShopItemData, AnimaDataSO> callback)
    {
        currentItem = item;
        onSelectCallback = callback;
        
        ClearAnimaSlots();
        
        List<AnimaDataSO> allAnimas = new List<AnimaDataSO>();
        
        if (AnimaInventoryManager.Instance != null)
        {
            allAnimas.AddRange(AnimaInventoryManager.Instance.GetAllAnima());
        }
        
        List<AnimaDataSO> filteredAnimas = FilterAnimasByItem(allAnimas, item);
        
        if (filteredAnimas.Count == 0)
        {
            ShowEmptyMessage("대상 아니마 없음");
            return;
        }
        
        CreateAnimaSlots(filteredAnimas);
        
        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }
        
        if (emptyListText != null)
        {
            emptyListText.gameObject.SetActive(false);
        }
    }
    
    public void CloseUI()
    {
        if (selectPanel != null)
        {
            selectPanel.SetActive(false);
        }
    }
    
    private List<AnimaDataSO> FilterAnimasByItem(List<AnimaDataSO> animas, ShopItemData item)
    {
        List<AnimaDataSO> filtered = new List<AnimaDataSO>();
        
        foreach (AnimaDataSO anima in animas)
        {
            bool isEligible = false;
            
            switch (item.itemType)
            {
                case ItemType.FullHeal:
                    isEligible = !anima.Animadie && anima.Stamina < anima.Maxstamina;
                    break;
                case ItemType.AllHeal:
                    isEligible = !anima.Animadie && anima.Stamina < anima.Maxstamina;
                    break;
                case ItemType.Revive:
                    isEligible = anima.Animadie;
                    break;
                case ItemType.Growth:
                    isEligible = true;
                    break;
                case ItemType.Enhance:
                    isEligible = true;
                    break;
            }
            
            if (isEligible)
            {
                filtered.Add(anima);
            }
        }
        
        return filtered;
    }
    
    private void ClearAnimaSlots()
    {
        if (animaListContent != null)
        {
            foreach (Transform child in animaListContent)
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    private void CreateAnimaSlots(List<AnimaDataSO> animas)
    {
        foreach (AnimaDataSO anima in animas)
        {
            GameObject slotObject = Instantiate(animaSlotPrefab, animaListContent);
            AnimaSlotUI slot = slotObject.GetComponent<AnimaSlotUI>();
            
            slot.SetData(anima, InventorySlotType.Shop, OnAnimaSelected);
        }
    }
    
    private void ShowEmptyMessage(string message)
    {
        if (emptyListText != null)
        {
            emptyListText.text = message;
            emptyListText.gameObject.SetActive(true);
        }
        
        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }
    }
    
    private void OnAnimaSelected(AnimaDataSO selectedAnima)
    {
        CloseUI();
        
        onSelectCallback?.Invoke(currentItem, selectedAnima);
    }
}