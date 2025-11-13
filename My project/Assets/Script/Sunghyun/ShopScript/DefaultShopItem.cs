using System.Collections.Generic;

public static class DefaultShopItems
{
    public static List<ShopItemData> GetDefaultItems()
    {
        return new List<ShopItemData>
        {
            new ShopItemData {
                itemID = "heal_single_Full",
                itemName = "회복약",
                itemDescription = "아니마 한 마리의 체력을 최대로 회복",
                itemType = ItemType.FullHeal,
                targetType = TargetType.Single,
                price = 30,
                maxPurchaseCount = 10
            },
            new ShopItemData {
                itemID = "heal_all",
                itemName = "만병통치약",
                itemDescription = "모든 아니마의 체력을 최대로 회복",
                itemType = ItemType.AllHeal,
                targetType = TargetType.All,
                price = 150,
                maxPurchaseCount = 5
            },
            new ShopItemData {
                itemID = "revive_single",
                itemName = "부활의 영약",
                itemDescription = "아니마 한 마리의 기절 상태를 회복",
                itemType = ItemType.Revive,
                targetType = TargetType.Single,
                price = 100,
                maxPurchaseCount = 3
            }
        };
    }
}