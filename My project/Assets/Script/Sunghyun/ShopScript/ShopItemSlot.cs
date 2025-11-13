using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemRemainingText;
    [SerializeField] private Button purchaseButton;

    private ShopItemData itemData;
    private int currentRemaining;
    private Action<ShopItemData> onPurchase;

    public void Setup(ShopItemData data, int remainingCount, Action<ShopItemData> onPurchaseCallback)
    {
        itemData = data;
        currentRemaining = remainingCount;
        onPurchase = onPurchaseCallback;

        itemImage.sprite = Resources.Load<Sprite>($"Minwoo/ShopImage/{data.itemID}");
        itemNameText.text = data.itemName;
        itemPriceText.text = $"{data.price:N0}";
        itemDescriptionText.text = data.itemDescription;
        itemRemainingText.text = $"{remainingCount} / {data.maxPurchaseCount}";

        purchaseButton.interactable = remainingCount > 0;
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
    }

    private void OnPurchaseButtonClick()
    {
        onPurchase?.Invoke(itemData);
    }

    public void UpdateRemainingCount(int newCount)
    {
        currentRemaining = newCount;
        itemRemainingText.text = $"{newCount} / {itemData.maxPurchaseCount}";
        purchaseButton.interactable = newCount > 0;
    }
}