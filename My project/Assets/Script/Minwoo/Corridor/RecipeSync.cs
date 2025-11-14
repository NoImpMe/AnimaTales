using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RecipeSync : MonoBehaviour
{
    public Image obj;
    public TextMeshProUGUI desc;
    void Update()
    {
        desc.text = obj.sprite.name;
    }
}
