using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager instance;

    public List<Follow> soldiers = new List<Follow>();
    public GameObject player;

    void Awake()
    {
        instance = this;
    }

    public void AddSoldier(Follow newSoldier)
    {
        if (soldiers.Count == 0)
        {
            newSoldier.Target = player;
        }
        else
        {
            newSoldier.Target = soldiers[soldiers.Count - 1].gameObject;
        }

        newSoldier.isFollowing = true;
        soldiers.Add(newSoldier);
    }
}
