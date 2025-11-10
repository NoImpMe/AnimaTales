using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityCreator : MonoBehaviour
{
    [SerializeField]
    Button[] abilitys = new Button[3];
    [SerializeField]
    TextMeshProUGUI[] abilityTxt = new TextMeshProUGUI[3];
    [SerializeField]
    Button[] rerolls = new Button[3];
    [SerializeField]
    TextMeshProUGUI[] rerollTxt = new TextMeshProUGUI[3];
    int[] rerollCnts = new int[] { 1, 1, 1};
    int[] ranNums = new int[3];
    void Start()
    {
        ranNums[0] = Random.Range(0, 3);
        ranNums[1] = Random.Range(0, 3);
        ranNums[2] = Random.Range(0, 3);
        while (ranNums[0] == ranNums[1])
        {
            ranNums[1] = Random.Range(0, 3);
        }
        while (ranNums[0] == ranNums[2] || ranNums[1] == ranNums[2])
        {
            ranNums[2] = Random.Range(0, 3);
        }

        for (int i = 0; i < 3; i++)
        {
            abilitys[i].image.sprite = Resources.Load<AbilitySO>($"Minwoo/Ability/Ability{ranNums[i]}").data.icon;
            abilityTxt[i].text = Resources.Load<AbilitySO>($"Minwoo/Ability/Ability{ranNums[i]}").data.description;
            rerollTxt[i].text = rerollCnts[i].ToString();
        }
    }
    public void Reroll()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        int selectNum = int.Parse(selectedButton.name.Substring(7, 8));
        //GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo
    }

    public void SelectAbility()
    {
        
    }
}
