using UnityEngine;

public class SessionData : MonoBehaviour
{
    public static SessionData instance;
    public int soldiersCollected = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}