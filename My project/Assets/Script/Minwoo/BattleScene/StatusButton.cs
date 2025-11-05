using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusButton : MonoBehaviour
{
    public GameObject image;
    
    public void onClicked() 
    {
        if (!image.activeSelf)
        {
            image.SetActive(true);
            
        }
        else
        {
            image.SetActive(false);
        }
    }

}
