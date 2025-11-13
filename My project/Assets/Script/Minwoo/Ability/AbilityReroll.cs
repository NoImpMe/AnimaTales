using UnityEngine;
using UnityEngine.UI;
public class AbilityReroll : MonoBehaviour
{
    [SerializeField] 
    Button rerollButton;
    [HideInInspector] public int rerollCnt = 2;
    public void UseReroll()
    {
        rerollCnt -= 1;
        if (rerollCnt == 0) rerollButton.interactable = false;
    }
}
