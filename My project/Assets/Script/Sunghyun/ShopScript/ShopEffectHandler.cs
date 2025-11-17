using BansheeGz.BGDatabase;
using UnityEngine;

public static class ShopEffectHandler
{
    public static void ApplyEffect(ShopItemData itemData, AnimaDataSO target = null, int randomNum = 0)
    {
        if (itemData == null)
        {
            return;
        }

        PlayerInfo playerInfo = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
        if (playerInfo == null)
        {
            return;
        }

        switch (itemData.itemType)
        {
            case ItemType.FullHeal:
                ApplyHeal(itemData, target, playerInfo);
                break;
            case ItemType.AllHeal:
                ApplyHeal(itemData, target, playerInfo);
                break;
            case ItemType.Revive:
                ApplyRevive(itemData, target, playerInfo);
                break;

            case ItemType.Growth:
                ApplyGrowth(itemData, target);
                break;

            case ItemType.Recipe:
                ApplyRecipe(itemData, randomNum);
                break;

            case ItemType.Enhance:
                ApplyEnhance(itemData, target);
                break;

            default:
                Debug.LogWarning($"알 수 없는 아이템 타입");
                break;
        }
    }

    private static void ApplyHeal(ShopItemData itemData, AnimaDataSO target, PlayerInfo playerInfo)
    {
        if (itemData.targetType == TargetType.Single)
        {
            if (target == null)
            {
                return;
            }

            float healAmount = 0;
            if (itemData.itemID.Contains("Full"))
            {
                healAmount = target.Maxstamina;
            }

            target.Stamina = Mathf.Min(target.Stamina + healAmount, target.Maxstamina);
        }
        else if (itemData.targetType == TargetType.All)
        {
            foreach (var anima in playerInfo.battleAnima)
            {
                if (!anima.Animadie)
                {
                    anima.Stamina = anima.Maxstamina;
                }
            }
            foreach (var anima in playerInfo.haveAnima)
            {
                if (!anima.Animadie)
                {
                    anima.Stamina = anima.Maxstamina;
                }
            }
        }
    }

    private static void ApplyRevive(ShopItemData itemData, AnimaDataSO target, PlayerInfo playerInfo)
    {
        if (itemData.targetType == TargetType.Single)
        {
            if (target == null || !target.Animadie)
            {
                return;
            }

            target.Animadie = false;
            target.Stamina = target.Maxstamina * 0.3f;
        }
    }

    private static void ApplyGrowth(ShopItemData itemData, AnimaDataSO target)
    {
        if (target == null)
        {
            return;
        }

        if (itemData.itemID.Contains("Max_boost"))
        {
            target.maxLevel[target.mood] += 1;
        }
    }

    private static void ApplyRecipe(ShopItemData itemData, int randomNum)
    {
        var database = BGRepo.I;
        var recipeTable = database.GetMeta("Recipe");
        recipeTable.GetEntity(randomNum).Set<int>("Sucess", 1);
        DBUpdater.Save();
    }

    private static void ApplyEnhance(ShopItemData itemData, AnimaDataSO target)
    {
        if (target == null)
        {
            return;
        }

        if (itemData.itemID.Contains("AP_buff"))
        {
            target.defAP += 0.01f; 
        }
    }
}
