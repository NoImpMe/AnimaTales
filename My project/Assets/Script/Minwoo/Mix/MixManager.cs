using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixManager : MonoBehaviour
{
    public TextMeshProUGUI skillText1;
    public TextMeshProUGUI skillText2;
    public AnimaDataSO mainAnima;
    public AnimaDataSO subAnima;
    public Image mainImage;
    public Image subImage;
    public Button skill1;
    public Button skill2;
    [SerializeField]
    GameObject resultCanvas;
    [SerializeField]
    TextMeshProUGUI resultText;
    [SerializeField]
    Image resultImage;
    [SerializeField]
    private AnimaSlotUI mainSlot;
    [SerializeField]
    private AnimaSlotUI subSlot;
    [SerializeField]
    TextAsset mixDataSet;

    List<MixData> mixDatas;
    List<MixData> matchedMixData;
    private void Start()
    {
        mixDatas = JsonConvert.DeserializeObject<List<MixData>>(mixDataSet.text);
        matchedMixData = new List<MixData>();
    }
    public void Init()
    {
        mainSlot.SetData(null, InventorySlotType.Main);
        subSlot.SetData(null, InventorySlotType.Sub);
    }
    public void Update()
    {
        if (mainAnima == null && mainImage != null) 
        {
            skillText1.text = "";
            skillText2.text = "";
            mainImage.sprite = null;
            mainImage.gameObject.GetComponent<CanvasGroup>().alpha = 0;  
        }
        if (subAnima == null && subImage != null )
        {
            subImage.sprite = null;
            subImage.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        if (mainAnima != null)
        {
            mainImage.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            mainImage.sprite = Resources.Load<Sprite>($"Minwoo/Portrait/{mainAnima.Objectfile}");
            skillText1.text = mainAnima.skillName[0];
            if (mainAnima.skillName.Count > 1) 
            {
                skillText2.text = mainAnima.skillName[1];
            }
        }
        if(subAnima != null)
        {
            subImage.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            subImage.sprite = Resources.Load<Sprite>($"Minwoo/Portrait/{subAnima.Objectfile}");
        }
    }
    public void Mix() 
    {
        resultCanvas.SetActive(true);
        matchedMixData = mixDatas.Where(x => x.Main == mainAnima.Name && x.Sub == subAnima.Name).ToList();
        float odds = Random.Range(0f, 1f);
        var inven = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>();
        if( odds < matchedMixData[0].Odds )
        {
            resultText.text = "교감 성공!!";
            resultImage.sprite = Resources.Load<Sprite>($"Minwoo/Portrait/{matchedMixData[0].Result}");
            int level = mainAnima.level;
            AnimaDataSO resultAnima = ScriptableObject.CreateInstance<AnimaDataSO>();
            resultAnima.Initialize(matchedMixData[0].Result, level);
            inven.playerInfo.haveAnima.Add(resultAnima);
            mainAnima = null;
            subAnima = null;
            mainImage.sprite = null;
            mainImage.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            subImage.sprite = null;
            subImage.gameObject.GetComponent<CanvasGroup>().alpha = 0;

        }
        else
        {
            resultText.text = "교감 실패..";
            resultImage.sprite = mainImage.sprite;
            inven.playerInfo.haveAnima.Add(mainAnima);
            mainAnima = null;
            subAnima = null;
            mainImage.sprite = null;
            mainImage.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            subImage.sprite = null;
            subImage.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        inven.InvenChanged();
    }

    public void Revert()
    {
        if (mainAnima == null && subAnima == null) return;
        var inven = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>();
        if (mainAnima != null)
        {
            inven.playerInfo.haveAnima.Add(mainAnima);
            mainAnima = null;
            mainSlot.AnimaData = null;
        }
        if (subAnima != null)
        {
            inven.playerInfo.haveAnima.Add(subAnima);
            subAnima = null;
            subSlot.AnimaData = null;
        }
        inven.InvenChanged();
    }

}
