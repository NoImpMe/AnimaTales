using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [Header("도감 슬롯 관련")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject slotPrefab;

    [Header("상세 정보 패널")]
    [SerializeField] private RecipeDetailUI detailPanel;

    [SerializeField] private AudioClip btnClip;


    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        List<RecipeEntry> recipeList = CorridorManager.Instance.GetAllRecipe();

        foreach (var anima in recipeList)
        {
            
            GameObject obj = Instantiate(slotPrefab, gridParent);
            RecipeSlot slot = obj.GetComponent<RecipeSlot>();
            slot.Setup(anima, anima.sucess);
            slot.onClick.AddListener(ShowDetail);
        }

    }

    private void ShowDetail(RecipeEntry anima)
    {
        AudioManager.Instance.PlaySFX(btnClip);
        detailPanel.gameObject.SetActive(true);
        detailPanel.Display(anima, anima.sucess);
    }
}
