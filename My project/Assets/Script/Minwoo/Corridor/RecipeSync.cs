using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RecipeSync : MonoBehaviour
{
    public Image obj;
    public TextMeshProUGUI desc;
    void Update()
    {
        if(obj.sprite.texture.name == "Unknown")
        {
            desc.text = "";
        }
        else
        {
            desc.text = obj.sprite.texture.name;
        }
        
    }
}
