using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public Image healthBarFill; 
    public bool isAlly = false;

    private void Start()
    {
        if (this.transform.parent.name.Contains("Ally"))
        {
            isAlly = true;
        }
        if (isAlly)
        {
            Color targetColor;
            targetColor = Color.green;
            healthBarFill.color = targetColor;
        }
        else
        {
            healthBarFill.color = Color.red;
        }

    }
    public void UpdateHealth(float newHealthPercentage)
    {
        StartCoroutine(SmoothHealthChange(healthBarFill.fillAmount, newHealthPercentage, 1.0f));
    }

    public IEnumerator SmoothHealthChange(float startFillAmount, float targetFillAmount, float duration)
    {
        float elapsedTime = 0f;



        while (elapsedTime < duration)
        {
            healthBarFill.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        healthBarFill.fillAmount = targetFillAmount;
    }
}
