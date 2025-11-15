using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> field;
    Color tmpColor;
    IsVisitedField checkField;
    public void EnterNewField()
    {
        for (int i = 0; i < field.Count; i++)
        {
            
            if (field[i].gameObject.activeSelf)
            {
                checkField = field[i].gameObject.GetComponent<IsVisitedField>();
                if(checkField.isVisited || !checkField.isSelected)
                {
                    foreach (Transform tile in field[i].transform)
                    {
                        if (tile.gameObject.GetComponent<Rigidbody2D>() != null)
                        {
                            tile.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                        }
                        tmpColor = tile.gameObject.GetComponent<Tilemap>().color;
                        tmpColor.a = 0.37f;
                        tile.gameObject.GetComponent<Tilemap>().color = tmpColor;
                    }
                }
            }
        }
    }
    public void ShowNextField()
    {
        for (int i = 0; i < field.Count; i++) 
        {
            if (field[i].gameObject.activeSelf)
            {
                checkField = field[i].gameObject.GetComponent<IsVisitedField>();
                if (checkField.isVisited)
                {
                    foreach(GameObject tile in checkField.nearFields)
                    {
                        tile.SetActive(true);
                    }
                }
            }
        }
    }
}
