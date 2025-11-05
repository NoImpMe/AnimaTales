using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HealthBarController healthBarController;  
    public float maxHealth; 
    public float currentHealth;

    public void Initialize(float maxStamina, float curStamina)
    {
        maxHealth = maxStamina;
        currentHealth = curStamina;
        healthBarController.UpdateHealth(currentHealth / maxHealth);
    }
    public IEnumerator UpdateHealthBar()
    {
        yield return StartCoroutine(healthBarController.SmoothHealthChange(healthBarController.healthBarFill.fillAmount, currentHealth/maxHealth, 1.3f));
    }

    public IEnumerator TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0; 
        }
        yield return StartCoroutine(healthBarController.SmoothHealthChange(healthBarController.healthBarFill.fillAmount, currentHealth / maxHealth, 1.3f));
        
    }
    public IEnumerator TakeHeal(float damage)
    {
        currentHealth += damage;
        if( currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        yield return StartCoroutine(healthBarController.SmoothHealthChange(healthBarController.healthBarFill.fillAmount, currentHealth / maxHealth, 1.3f));
    }
}
