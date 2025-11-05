using UnityEngine;

public static class ShopEffectHandler
{
    public static void ApplyEffect(ShopItemData itemData, AnimaDataSO target = null)
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
            case ItemType.Heal:
                ApplyHeal(itemData, target, playerInfo);
                break;

            case ItemType.Revive:
                ApplyRevive(itemData, target, playerInfo);
                break;

            case ItemType.Growth:
                ApplyGrowth(itemData, target);
                break;

            case ItemType.Recipe:
                ApplyRecipe(itemData, target);
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
            if (itemData.itemID.Contains("half"))
            {
                healAmount = target.Maxstamina * 0.5f;
            }
            else if (itemData.itemID.Contains("Full"))
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
            Debug.Log($"[성장] {target.Name}의 레벨 상한이 증가");
        }
    }

    private static void ApplyRecipe(ShopItemData itemData, AnimaDataSO target)
    {
        Debug.Log($"[레시피] {itemData.itemName}을(를) 획득");
    }

    private static void ApplyEnhance(ShopItemData itemData, AnimaDataSO target)
    {
        if (target == null)
        {
            return;
        }

        if (itemData.itemID.Contains("AP_buff"))
        {
            target.Damage += 5; 
        }
    }
}
