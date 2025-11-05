using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimaInventoryUIController : MonoBehaviour
{
    [Header("슬롯 프리팹 및 부모 오브젝트")]
    [SerializeField] private GameObject animaSlotPrefab;
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform PartySlotParent;
    [SerializeField] private Transform mainSlotParent;
    [SerializeField] private Transform subSlotParent;
    [Header("디테일 패널")]
    [SerializeField] private AnimaInventoryDetailUI detailUI;
    private List<AnimaSlotUI> inventorySlots = new List<AnimaSlotUI>();
    private List<AnimaSlotUI> PartySlots = new List<AnimaSlotUI>();
    private MixManager mixManager;
    private void Start()
    {
        if (AnimaInventoryManager.Instance != null)
        {
            AnimaInventoryManager.Instance.OnAnimaInventoryChanged += RefreshUI;
            InitializeUI();
        }
    }

    private void OnDestroy()
    {
        if (AnimaInventoryManager.Instance != null)
        {
            AnimaInventoryManager.Instance.OnAnimaInventoryChanged -= RefreshUI;
        }
    }

    private void InitializeUI()
    {
        CreatePartySlots();
        CreateInventorySlots();
    }
    private void CreatePartySlots()
    {
        foreach (Transform child in PartySlotParent)
            Destroy(child.gameObject);
        PartySlots.Clear();

        var partyAnimas = AnimaInventoryManager.Instance.GetActiveAnima();
        for (int i = 0; i < 3; i++)
        {
            AnimaDataSO anima = i < partyAnimas.Count ? partyAnimas[i] : null;
            var slot = Instantiate(animaSlotPrefab, PartySlotParent);
            var slotUI = slot.GetComponent<AnimaSlotUI>();
            slotUI.SetData(anima, InventorySlotType.Party, OnSlotClicked);
            PartySlots.Add(slotUI);
        }
    }

    private void CreateInventorySlots()
    {
        foreach (Transform child in inventorySlotParent)
            Destroy(child.gameObject);
        inventorySlots.Clear();

        var allAnimas = AnimaInventoryManager.Instance.GetHaveAnima();
        int animaCount = allAnimas.Count;

        int requiredSlotCount = Mathf.Max(12, Mathf.CeilToInt(animaCount / 4f) * 4);

        for (int i = 0; i < requiredSlotCount; i++)
        {
            var slot = Instantiate(animaSlotPrefab, inventorySlotParent);
            var slotUI = slot.GetComponent<AnimaSlotUI>();
            AnimaDataSO data = i < animaCount ? allAnimas[i] : null;
            slotUI.SetData(data, InventorySlotType.Inventory, OnSlotClicked);
            inventorySlots.Add(slotUI);
        }
    }
    private void RefreshUI()
    {
        UpdatePartySlots();
        UpdateInventorySlots();
    }

    private void UpdatePartySlots()
    {
        var partyAnimas = AnimaInventoryManager.Instance.GetActiveAnima();
        
        if (PartySlots.Count == 0)
        {
            CreatePartySlots();
            return;
        }
        
        for (int i = 0; i < PartySlots.Count; i++)
        {
            AnimaDataSO anima = i < partyAnimas.Count ? partyAnimas[i] : null;
            if (PartySlots[i] != null)
            {
                PartySlots[i].SetData(anima, InventorySlotType.Party, OnSlotClicked);
            }
        }
    }

    private void UpdateInventorySlots()
    {
        var allAnimas = AnimaInventoryManager.Instance.GetHaveAnima();
        int animaCount = allAnimas.Count;
        
        if (inventorySlots.Count == 0)
        {
            CreateInventorySlots();
            return;
        }
        
        int requiredSlotCount = Mathf.Max(12, Mathf.CeilToInt(animaCount / 4f) * 4);
        
        if (inventorySlots.Count != requiredSlotCount)
        {
            CreateInventorySlots();
            return;
        }
        
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            AnimaDataSO data = i < animaCount ? allAnimas[i] : null;
            if (inventorySlots[i] != null)
            {
                inventorySlots[i].SetData(data, InventorySlotType.Inventory, OnSlotClicked);
            }
        }
    }

    private void OnSlotClicked(AnimaDataSO anima)
    {
        if (detailUI != null)
        {
            detailUI.Show(anima);
        }
    }
}