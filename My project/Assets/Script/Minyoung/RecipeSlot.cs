using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    [Header("UI ¿ä¼Ò")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite openImage;
    [SerializeField] private Button slotButton;
    private RecipeEntry recipeData;

    public UnityEvent<RecipeEntry> onClick = new();

    public void Setup(RecipeEntry entry, int sucessed)
    {
        recipeData = entry;
        if(sucessed == 1)
        {
            iconImage.sprite = openImage;
        }
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => onClick.Invoke(recipeData));
    }
}
