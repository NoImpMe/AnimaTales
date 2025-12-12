using System.Collections;
using UnityEngine;

public class MilestoneMoveLfRtUI : MonoBehaviour
{
    public float moveDistance = 6f;
    private float duration = 1f;
    Vector3 originPos;
    Coroutine runningCoroutine;
    private void Start()
    {
        originPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        runningCoroutine = StartCoroutine(MoveAnimation());
    }
    private IEnumerator MoveAnimation()
    {
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float t = (elapsed / duration) * 2f * Mathf.PI;

            float yOffset = Mathf.Sin(t) * moveDistance;

            Vector3 pos = originPos;
            pos.x += yOffset;
            gameObject.GetComponent<RectTransform>().anchoredPosition = pos;

            if (elapsed > duration)
                elapsed -= duration;

            yield return null;
        }
    }
    public void Stop()
    {
        Destroy(gameObject);
    }
}
