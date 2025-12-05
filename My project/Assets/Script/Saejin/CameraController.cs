using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX , mapMinY, mapMaxY;
    private GameObject camDesOB;
    private DontDesManager camDes;
    Vector3 dragStartWorldPos;
    bool isDragging;

    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;

        camDesOB = GameObject.Find("DontDesManager");
        camDes = camDesOB.GetComponent<DontDesManager>();
    }

    private void Update()
    {
        HandleCameraDrag();
    }

    private void HandleCameraDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(1))
            BeginDrag();

        if (Input.GetMouseButton(1) && isDragging)
            DragCamera();

        if (Input.GetMouseButtonUp(1))
            EndDrag();
    }
    private void BeginDrag()
    {
        isDragging = true;
        dragStartWorldPos = GetMouseWorldPos();
    }

    private void DragCamera()
    {
        Vector3 currentWorldPos = GetMouseWorldPos();
        Vector3 delta = dragStartWorldPos - currentWorldPos;

        cam.transform.position = ClampCamera(cam.transform.position + delta);
    }

    private void EndDrag()
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        return cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane)
        );
    }
    private Vector3 ClampCamera(Vector3 targetPos)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;

        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        // Z는 원래 값 유지
        return new Vector3(clampedX, clampedY, targetPos.z);
    }
    public void setMaxMin()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }
}
