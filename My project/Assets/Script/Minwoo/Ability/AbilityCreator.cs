using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AbilityCreator : MonoBehaviour
{
    [SerializeField]
    GameObject[] abilitys;
    [SerializeField]
    TextMeshProUGUI[] abilityTxt = new TextMeshProUGUI[3];
    [SerializeField]
    TextMeshProUGUI[] rerollTxt = new TextMeshProUGUI[3];
    [SerializeField]
    AbilityReroll[] abilityReroll;
    int[] ranNums = new int[3];
    List<AbilitySO> exceptAbility;
    [SerializeField]
    List<AbilitySO> bronzeList;
    [SerializeField]
    List<AbilitySO> silverList;
    [SerializeField]
    List<AbilitySO> goldList;
    [SerializeField]
    List<AbilitySO> platinumList;
    [SerializeField]
    private AudioClip rerollClip;
    [SerializeField]
    private AudioClip selectClip;

    float duration = 1f;
    void Start()
    {
        InitAbility();
        for (int i = 0; i < 3; i++)
        {
            GameObject oldObj = abilitys[i];
            abilitys[i] = Instantiate(Resources.Load<GameObject>($"Minwoo/Ability/{exceptAbility[i].data.id}Ability"),oldObj.transform.position, Quaternion.identity,this.transform);
            abilitys[i].GetComponent<AbilityHolder>().abilitySO = exceptAbility[i];
            abilitys[i].transform.Find("AbilityImage").GetComponent<Image>().sprite = exceptAbility[i].data.icon;
            abilitys[i].transform.Find("AbilityTxt").GetComponent<TextMeshProUGUI>().text = exceptAbility[i].data.description;
            rerollTxt[i].text = abilityReroll[i].rerollCnt.ToString();
            abilitys[i].GetComponent<Button>().onClick.AddListener(SelectAbility);
            Destroy(oldObj);
        }
    }
    public void Reroll()
    {
        AudioManager.Instance.PlaySFX(rerollClip);
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        int selectNum = int.Parse(selectedButton.name.Substring(6, 1));
        if (abilityReroll[selectNum].rerollCnt > 0)
        {
            AbilitySO newAbility = AbilityChoice();
            exceptAbility[selectNum] = newAbility;
            GameObject oldObj = abilitys[selectNum];
            abilitys[selectNum] = Instantiate(Resources.Load<GameObject>($"Minwoo/Ability/{newAbility.data.id}Ability"), oldObj.transform.position, Quaternion.identity, this.transform);
            abilitys[selectNum].GetComponent<AbilityHolder>().abilitySO = newAbility;
            abilitys[selectNum].transform.Find("AbilityImage").GetComponent<Image>().sprite = newAbility.data.icon;
            abilitys[selectNum].transform.Find("AbilityTxt").GetComponent<TextMeshProUGUI>().text = newAbility.data.description;
            abilityReroll[selectNum].UseReroll();
            rerollTxt[selectNum].text = abilityReroll[selectNum].rerollCnt.ToString();
            abilitys[selectNum].GetComponent<Button>().onClick.AddListener(SelectAbility);
            Destroy(oldObj);
        }
    }
    private void InitAbility()
    {
        exceptAbility = new List<AbilitySO>();
        for(int i = 0; i < 3; i++)
        {
            exceptAbility.Add(AbilityChoice());
        }
    }
    private AbilitySO AbilityChoice()
    {
        float odds = Random.Range(0f, 1f);
        int range;
        if (odds < 0.65)
        {
            range = Random.Range(0, bronzeList.Count);
            while (exceptAbility.Contains(bronzeList[range]))
            {
                range = Random.Range(0, bronzeList.Count);
            }
            return bronzeList[range];
        }
        else if (odds < 0.85)
        {
            range = Random.Range(0, silverList.Count);
            while (exceptAbility.Contains(silverList[range]))
            {
                range = Random.Range(0, silverList.Count);
            }
            return silverList[range];
        }
        else if (odds < 0.95)
        {
            range = Random.Range(0, goldList.Count);
            while (exceptAbility.Contains(goldList[range]))
            {
                range = Random.Range(0, goldList.Count);
            }
            return goldList[range];
        }
        else
        {
            range = Random.Range(0, platinumList.Count);
            while (exceptAbility.Contains(platinumList[range]))
            {
                range = Random.Range(0, platinumList.Count);
            }
            return platinumList[range];
        }
    }
    public void SelectAbility()
    {
        AudioManager.Instance.PlayBGM(selectClip);
        GameObject tmp = EventSystem.current.currentSelectedGameObject;
        Transform focus = GameObject.Find("FocusTarget").transform;
        GameObject.Find("Game Manager").GetComponent<AbilityManager>().GetSymbol(tmp.GetComponent<AbilityHolder>().abilitySO);
        tmp.GetComponent<AbilityVibrator>().StopVib();
        foreach (GameObject obj in abilitys)
        {
            if(obj == tmp)
            {
                continue;
            }
            obj.SetActive(false);
        }
        foreach(var obj in abilityReroll)
        {
            obj.gameObject.SetActive(false);
        }
        StartCoroutine(AbilityMove(tmp.GetComponent<RectTransform>(), tmp.transform.localPosition, focus.localPosition, tmp.transform.localScale));
        GameObject.Find("Next Battle Button").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("Next Battle Button").GetComponent<CanvasGroup>().alpha = 1f;
    }
    IEnumerator AbilityMove(RectTransform rt, Vector2 fromPos, Vector2 toPos, Vector3 fromScale)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; 
            float lerp = t / duration;

            rt.anchoredPosition = Vector2.Lerp(fromPos, toPos, lerp);
            rt.localScale = Vector3.Lerp(fromScale, new Vector3(1.2f,1.2f,1.2f), lerp);

            yield return null;
        }
        rt.anchoredPosition = toPos;
        rt.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
}
