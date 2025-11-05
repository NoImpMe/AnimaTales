using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public Image healthBarFill; 

    public void UpdateHealth(float newHealthPercentage)
    {
        StartCoroutine(SmoothHealthChange(healthBarFill.fillAmount, newHealthPercentage, 1.3f));
    }

    public IEnumerator SmoothHealthChange(float startFillAmount, float targetFillAmount, float duration)
    {
        float elapsedTime = 0f;

        Color startColor = healthBarFill.color;
        Color targetColor;

        if (targetFillAmount > 0.5f)
        {
            targetColor = Color.green;
        }
        else if (targetFillAmount > 0.3f)
        {
            targetColor = new Color(1f, 0.64f, 0f);
        }
        else
        {
            targetColor = Color.red; 
        }

        while (elapsedTime < duration)
        {
            healthBarFill.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);

            healthBarFill.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        healthBarFill.fillAmount = targetFillAmount;
        healthBarFill.color = targetColor;
    }
}
