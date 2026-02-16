using UnityEngine;

public class CameraFollowFight : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private BoxCollider2D cameraBounds;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Camera cam;

    private float minX, maxX, minY, maxY;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        CalculateBounds();
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, offset.z);
    }

    void CalculateBounds()
    {
        Bounds bounds = cameraBounds.bounds;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        minX = bounds.min.x + camWidth;
        maxX = bounds.max.x - camWidth;
        minY = bounds.min.y + camHeight;
        maxY = bounds.max.y - camHeight;
    }
}
