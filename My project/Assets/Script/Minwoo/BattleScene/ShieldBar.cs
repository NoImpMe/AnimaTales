using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public ShieldBarController shieldBarController;  
    public float maxshield; 
    public float currentshield;

    public void Initialize(float maxShield, float currentShield)
    {
        this.maxshield = maxShield;
        this.currentshield = currentShield;
        shieldBarController.Updateshield(currentshield / maxshield);
    }
    public IEnumerator UpdateshieldBar()
    {
        yield return StartCoroutine(shieldBarController.SmoothshieldChange(shieldBarController.shieldBarFill.fillAmount, currentshield/maxshield, 1.0f));
    }

    public IEnumerator TakeDamage(float damage)
    {
        currentshield -= damage;
        if (currentshield < 0)
        {
            currentshield = 0; 
        }
        yield return StartCoroutine(shieldBarController.SmoothshieldChange(shieldBarController.shieldBarFill.fillAmount, currentshield / maxshield, 1.0f));
    }
    public IEnumerator TakeShield(float damage)
    {
        currentshield += damage;
        if( currentshield > maxshield)
        {
            maxshield += currentshield;
        }
        yield return StartCoroutine(shieldBarController.SmoothshieldChange(shieldBarController.shieldBarFill.fillAmount, currentshield / maxshield, 1.0f));
    }
}
