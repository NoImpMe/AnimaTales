using System.Collections;
using UnityEngine;

public class AbilityVibrator : MonoBehaviour
{
    float vibrateScale = 0.1f;
    float duration = 1f;
    Vector3 originSize;
    Coroutine runningCoroutine;
    private void Start()
    {
        originSize = transform.localScale;
        runningCoroutine = StartCoroutine(PulseAnimation());
    }
    private IEnumerator PulseAnimation()
    {
        while (true)
        {
            float elapsed = 0f;
            while (elapsed < duration / 2)
            {
                float scale = 1f + Mathf.Sin((elapsed / (duration / 2)) * Mathf.PI / 2) * vibrateScale;
                transform.localScale = originSize * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < duration / 2)
            {
                float scale = 1f + vibrateScale - Mathf.Sin((elapsed / (duration / 2)) * Mathf.PI / 2) * vibrateScale;
                transform.localScale = originSize * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
    public void StopVib()
    {
        StopCoroutine(runningCoroutine);
    }

}
