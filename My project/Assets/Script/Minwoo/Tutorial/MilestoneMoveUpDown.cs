using System.Collections;
using UnityEngine;

public class MilestoneMoveUpDown : MonoBehaviour
{
    public float moveDistance = 0.06f;
    private float duration = 1f;
    Vector3 originPos;
    Coroutine runningCoroutine;
    private void Start()
    {
        originPos = transform.localPosition;
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
            pos.y += yOffset;
            transform.localPosition = pos;

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
