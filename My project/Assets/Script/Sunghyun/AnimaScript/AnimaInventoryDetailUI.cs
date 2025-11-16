using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AnimaInventoryDetailUI : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text apText;
    [SerializeField] private TMP_Text dpText;
    [SerializeField] private TMP_Text spText;

    [SerializeField] private TMP_Text skill1NameText;
    [SerializeField] private TMP_Text skill1DescriptionText;
    [SerializeField] private TMP_Text skill2NameText;
    [SerializeField] private TMP_Text skill2DescriptionText;
    
    [SerializeField] private Material grayscaleMaterial;
    [SerializeField] private Color normalHpColor = Color.white;
    [SerializeField] private Color defeatedHpColor = Color.red;

    private static Dictionary<string, Sprite> portraitCache = new Dictionary<string, Sprite>();

    public void Show(AnimaDataSO anima)
    {
        if (anima == null)
        {
            Clear();
            return;
        }

        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    
        string imagePath = "Anima_Sprites/" + anima.Objectfile;
        if (portraitImage != null)
        {
            if (!portraitCache.TryGetValue(imagePath, out Sprite sprite))
            {
                sprite = Resources.Load<Sprite>(imagePath);
                if (sprite != null)
                {
                    portraitCache[imagePath] = sprite;
                }
            }
            portraitImage.sprite = sprite;
            
            bool isDefeated = anima.Animadie || anima.Stamina <= 0;
            if (isDefeated)
            {
                portraitImage.material = grayscaleMaterial;
            }
            else
            {
                portraitImage.material = null;
            }
        }
    
        if (nameText != null) nameText.text = anima.Name;
        if (levelText != null) levelText.text = anima.level.ToString();
        if (typeText != null) typeText.text = anima.type;

        if (hpText != null) 
        {
            hpText.text = $"{Mathf.FloorToInt(anima.Stamina)} / {Mathf.FloorToInt(anima.Maxstamina)}";
            
            bool isDefeated = anima.Animadie || anima.Stamina <= 0;
            hpText.color = isDefeated ? defeatedHpColor : normalHpColor;
        }

        if (apText != null) apText.text = Mathf.FloorToInt(anima.Damage).ToString();
        if (dpText != null) dpText.text = Mathf.FloorToInt(anima.Defense).ToString();
        if (spText != null) spText.text = Mathf.FloorToInt(anima.Speed).ToString();
        if(anima.skillName.Count == 0)
        {
            skill1NameText.text = "";
            skill2NameText.text = "";
        }
        if (anima.skillName.Count == 1)
        {
            skill1NameText.text = anima.skillName[0];
            skill2NameText.text = "";
        }

        if (anima.skillName.Count == 2)
        {
            skill1NameText.text = anima.skillName[0];
            skill2NameText.text = anima.skillName[1];
        }
    
        if (skill1DescriptionText != null) skill1DescriptionText.text = "";
        if (skill2DescriptionText != null) skill2DescriptionText.text = "";
    }

    public void Clear()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
    
    public static void ClearPortraitCache()
    {
        portraitCache.Clear();
    }
}