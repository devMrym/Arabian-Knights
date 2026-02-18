using UnityEngine;

public class EnemyKillBinder : MonoBehaviour
{
    [HideInInspector]
    public IntensityOverlayUI intensityOverlay;

    void Start()
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.onDeath.AddListener(OnEnemyKilled);
        }
    }

    void OnEnemyKilled()
    {
        if (intensityOverlay != null)
        {
            intensityOverlay.RegisterKill();
        }
    }
}
