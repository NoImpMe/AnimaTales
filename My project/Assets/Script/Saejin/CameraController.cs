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
    private Vector3 dragOrigin;

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
        PanCamera();
    }

    private void PanCamera()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position = ClampCamera(cam.transform.position + difference);
            camDes.setCamPosition(cam.transform.position);
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWIdth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWIdth;
        float maxX = mapMaxX - camWIdth;

        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
    public void setMaxMin()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }
}
