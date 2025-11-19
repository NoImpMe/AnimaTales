
using System.Collections.Generic;
using UnityEngine;

public class VillageDataManager : MonoBehaviour
{
    public static VillageDataManager Instance { get; private set; }
    
    private string currentVillageID;
    
    private Dictionary<string, VillageShopState> villageShops = new Dictionary<string, VillageShopState>(10);
    private int innCurrentPrice = 1000;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public int GetInnPrice()
    {
        return innCurrentPrice;
    }
    
    public void UpdateInnPrice(int newPrice)
    {
        innCurrentPrice = newPrice;
    }
    
    public VillageShopState GetVillageShopState(string villageID, List<ShopItemData> shopItems)
    {
        if (villageShops.ContainsKey(villageID))
        {
            return villageShops[villageID];
        }
        
        VillageShopState newState = new VillageShopState(villageID, shopItems);
        villageShops[villageID] = newState;
        
        return newState;
    }
    
    public bool PurchaseItem(string villageID, string itemID)
    {
        if (!villageShops.ContainsKey(villageID))
        {
            return false;
        }
        
        return villageShops[villageID].PurchaseItem(itemID);
    }
    
    public void ClearAllVillages()
    {
        villageShops.Clear();
    }
    
    public void SetCurrentVillageID(string villageID)
    {
        currentVillageID = villageID;
    }

    public string GetCurrentVillageID()
    {
        return currentVillageID;
    }
}