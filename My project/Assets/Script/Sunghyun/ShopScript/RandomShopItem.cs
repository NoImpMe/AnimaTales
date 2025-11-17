using System.Collections.Generic;

public static class RandomShopItems
{
    public static List<ShopItemData> GetRandomItemPool()
    {
        return new List<ShopItemData>
        {
            new ShopItemData {
                itemID = "Recipe_Common",
                itemName = "교감의 두루마리",
                itemDescription = "교감의 두루마리를 하나 획득",
                itemType = ItemType.Recipe,
                targetType = TargetType.None,
                price = 500,
                maxPurchaseCount = 3
            },
            new ShopItemData {
                itemID = "growth_Max_boost",
                itemName = "이상한 알약",
                itemDescription = "아니마 한 마리의 레벨 상한을 1증가",
                itemType = ItemType.Growth,
                targetType = TargetType.Single,
                price = 500,
                maxPurchaseCount = 2
            },
            new ShopItemData {
                itemID = "enhance_AP_buff",
                itemName = "힘의 뿌리",
                itemDescription = "아니마 한 마리의 공격력 스테이터스를 증가",
                itemType = ItemType.Enhance,
                targetType = TargetType.Single,
                price = 400,
                maxPurchaseCount = 5
            },
        };
    }
}