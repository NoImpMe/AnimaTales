using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    float goldSymbol = 0f;
    public float GoldSymbol => goldSymbol;
    float mixSymbol = 0f;
    public float MixSymbol => mixSymbol;
    float dropSymbol = 0f;
    public float DropSymbol => dropSymbol;
    float statSymbol = 0f;
    public float StatSymbol => statSymbol;
    float shieldSymbol = 0f;
    public float ShieldSymbol => shieldSymbol;
    [SerializeField]
    List<AbilitySO> abilitys;
    public List<AbilitySO> Abilitys => abilitys;

    public void GetSymbol(AbilitySO ability)
    {
        abilitys.Add(ability);
        switch (ability.data.id) 
        {
            case "goldSymbol":
                goldSymbol += ability.data.value;
                break;
            case "mixSymbol":
                mixSymbol += ability.data.value;
                break;
            case "dropSymbol":
                dropSymbol += ability.data.value;
                break;
            case "statSymbol":
                statSymbol += ability.data.value;
                break;
            case "permanShieldSymbol":
                shieldSymbol += ability.data.value;
                break;
            case "temporShieldSymbol":
                PlayerInfo playerInfo = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
                for(int i =0; i < playerInfo.battleAnima.Count; i++)
                {
                    playerInfo.battleAnima[i].Shield += ability.data.value;
                }
                break;
        }
    }
}
    