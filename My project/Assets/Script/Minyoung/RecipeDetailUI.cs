using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDetailUI : MonoBehaviour
{
    [Header("UI ÂüÁ¶")]
    [SerializeField] private Image main;
    [SerializeField] private Image sub;
    [SerializeField] private Image result;

    public void Display(RecipeEntry recipe, int sucessed)
    {
        if(sucessed == 1)
        {
            main.sprite = recipe.GetMainImage();
            sub.sprite = recipe.GetSubImage();
            result.sprite = recipe.GetResultImage();
        }
        else
        {
            main.sprite = recipe.GetQuestionImage();
            sub.sprite = recipe.GetQuestionImage();
            result.sprite = recipe.GetQuestionImage();
        }
    }
}
