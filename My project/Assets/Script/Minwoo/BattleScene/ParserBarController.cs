using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParserBarController : MonoBehaviour
{
    public Image parserBarFill;

    public void UpdatePoint(float newHealthPercentage)
    {
        StartCoroutine(SmoothPointChange(parserBarFill.fillAmount, newHealthPercentage, 1.3f));
    }

    public IEnumerator SmoothPointChange(float startFillAmount, float targetFillAmount, float duration)
    {
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            parserBarFill.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        parserBarFill.fillAmount = targetFillAmount;
    }
}
