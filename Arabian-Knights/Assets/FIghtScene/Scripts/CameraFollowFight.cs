using UnityEngine;

public class CameraFollowFight : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    private Transform target;

    [Header("Camera Bounds")]
    [SerializeField]
    private BoxCollider2D boundA;
    [SerializeField]
    private BoxCollider2D boundB;

    [Header("Smooth Settings")]
    [SerializeField]
    private float smoothTime = 0.25f;

    private Camera cam;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity;

    private BoxCollider2D activeBound;
    private bool boundBLocked = false;

    private float minX, maxX, minY, maxY;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        UpdateActiveBound();
        if (activeBound == null)
            return;

        CalculateBounds(activeBound);

        Vector3 desiredPos = target.position + offset;

        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);

        Vector3 smoothPos = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            smoothTime
        );

        transform.position = new Vector3(smoothPos.x, smoothPos.y, offset.z);
    }

    void UpdateActiveBound()
    {
        if (activeBound != null)
            return;

        if (boundA != null && boundA.OverlapPoint(target.position))
        {
            activeBound = boundA;
            return;
        }

        if (!boundBLocked && boundB != null && boundB.OverlapPoint(target.position))
        {
            activeBound = boundB;
            boundBLocked = true; // 🔒 once entered, it becomes one-way
        }
    }

    void CalculateBounds(BoxCollider2D cameraBounds)
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
