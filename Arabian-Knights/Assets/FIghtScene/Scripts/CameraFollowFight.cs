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

    private Camera cam;
    private Vector3 offset = new Vector3(0f, 0f, -10f);

    private BoxCollider2D activeBound;
    private bool boundBLocked = false;

    private float minX, maxX, minY, maxY;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        // Force initial bound check
        UpdateActiveBound();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        UpdateActiveBound();

        Vector3 desiredPos = target.position + offset;

        // If we have a bound, clamp
        if (activeBound != null)
        {
            CalculateBounds(activeBound);

            desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
            desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);
        }

        transform.position = new Vector3(desiredPos.x, desiredPos.y, offset.z);
    }

    void UpdateActiveBound()
    {
        if (boundA != null && boundA.OverlapPoint(target.position))
        {
            activeBound = boundA;
            return;
        }

        if (!boundBLocked && boundB != null && boundB.OverlapPoint(target.position))
        {
            activeBound = boundB;
            boundBLocked = true;
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
