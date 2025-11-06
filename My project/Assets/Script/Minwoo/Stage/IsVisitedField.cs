using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IsVisitedField : MonoBehaviour
{
    public bool isVisited = false;
    public bool isSelected = false;
    public List<GameObject> nearFields;
}
