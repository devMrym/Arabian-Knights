using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager instance;

    public Transform player;
    private Transform lastSoldier = null;

    void Awake()
    {
        instance = this;
    }

    public Transform GetFollowTarget()
    {
        if (lastSoldier == null)
            return player;

        return lastSoldier;
    }

    public void RegisterSoldier(Transform soldier)
    {
        lastSoldier = soldier;
    }
}
