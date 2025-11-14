using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimaActionUIController : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI animaName;
    public GameObject selectAction;
    public GameObject selectSkill;
    public GameObject skill1Frame;
    public GameObject skill2Frame;
    [Header("Button")]
    public Button attackButton;
    public Button skillButton;
    public Button skill1;
    public Button skill2;
    public Button cancleButton;
    [Header ("Skill")]
    public Image skill1Image;
    public TextMeshProUGUI skill1Des;
    public Image skill2Image;
    public TextMeshProUGUI skill2Des;
    
    public void CancelButton()
    {
        selectSkill.SetActive (false);
        selectAction.SetActive (true);
    }
}
