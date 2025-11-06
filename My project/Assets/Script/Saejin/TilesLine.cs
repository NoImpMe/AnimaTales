using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(CompositeCollider2D))]
public class TilesLine : MonoBehaviour
{
    [Header("LineRenderer Settings")]
    public Material lineMat;
    public float lineWidth = 0.1f;
    [Tooltip("깜빡임 속도")]
    public float blinkSpeed = 2f;
    public RegionController regionController;
    private List<LineRenderer> lines;

    void Start()
    {
        lines = new List<LineRenderer>();
        var composite = GetComponent<CompositeCollider2D>();
        composite.GenerateGeometry();  // 콤포지트 콜라이더 갱신

        for (int pathIdx = 0; pathIdx < composite.pathCount; pathIdx++)
        {
            Vector2[] path = new Vector2[composite.GetPathPointCount(pathIdx)];
            composite.GetPath(pathIdx, path);

            var go = new GameObject($"Outline_{pathIdx}");
            go.transform.SetParent(transform);
            var lr = go.AddComponent<LineRenderer>();
            lr.material = lineMat;
            lr.positionCount = path.Length + 1;
            lr.loop = true;
            lr.startWidth = lr.endWidth = lineWidth;

            for (int i = 0; i < path.Length; i++)
                lr.SetPosition(i, (Vector3)path[i]);
            lr.SetPosition(path.Length, (Vector3)path[0]);

            lines.Add(lr);
        }
    }

    void Update()
    {
        if (regionController == null) return;

        if (!regionController.isCleared && lines != null)
        {
            float t = (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;
            Color blinkCol = Color.Lerp(Color.white, Color.yellow, t);
            foreach (var lr in lines)
            {
                lr.startColor = lr.endColor = blinkCol;
            }
        }
        else if(!regionController.isVillaged)
        {
            lines = null;
        }
    }
}
