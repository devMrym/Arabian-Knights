using UnityEngine;

public class CameraFollowFight : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
