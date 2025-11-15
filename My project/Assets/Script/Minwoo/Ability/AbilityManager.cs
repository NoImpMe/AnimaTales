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
    float shieldSymbol = 0f;
    public float ShieldSymbol => shieldSymbol;
    [SerializeField]
    List<AbilitySO> abilitys = new();
    public List<AbilitySO> Abilitys => abilitys;

    public void GetSymbol(AbilitySO ability)
    {
        abilitys.Add(ability);
        PlayerInfo playerInfo = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
        switch (ability.data.name) 
        {
            case "GoldSymbol":
                goldSymbol += ability.data.value;
                break;
            case "MixSymbol":
                mixSymbol += ability.data.value;
                break;
            case "DropSymbol":
                dropSymbol += ability.data.value;
                break;
            case "StatSymbol":
                for (int i = 0; i < playerInfo.battleAnima.Count; i++)
                {
                    playerInfo.battleAnima[i].weight *= ability.data.value;
                }
                break;
            case "PermanShieldSymbol":
                shieldSymbol += ability.data.value;
                break;
            case "TemporShieldSymbol":
                for(int i =0; i < playerInfo.battleAnima.Count; i++)
                {
                    playerInfo.battleAnima[i].Shield += ability.data.value;
                }
                break;
        }
    }
}
    