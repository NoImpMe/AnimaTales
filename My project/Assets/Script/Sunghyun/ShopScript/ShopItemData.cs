using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItemData
{
    public Button itemButton;
    public string itemID;
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public TargetType targetType;
    public int price;
    public int maxPurchaseCount = 99;
}

public enum ItemType
{
    FullHeal,
    AllHeal,
    Revive,
    Growth,
    Recipe,
    Enhance
}

public enum TargetType
{
    Single,
    All,
    None
}