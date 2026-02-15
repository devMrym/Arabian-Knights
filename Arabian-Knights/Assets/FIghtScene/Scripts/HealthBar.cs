using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    private Transform target;

    public void Follow(Transform followTarget)
    {
        target = followTarget;
    }

    public void UpdateBar(int current, int max)
    {
        fillImage.fillAmount = (float)current / max;
    }

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
    }
}
