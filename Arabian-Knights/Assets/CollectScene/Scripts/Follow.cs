using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject Target;
    public float speed = 5f;
    public bool isFollowing = false;
    public float followDistance = 1.2f;   // spacing

    void Update()
    {
        if (isFollowing && Target != null)
        {
            float dist = Vector2.Distance(
                transform.position,
                Target.transform.position
            );

            if (dist > followDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    Target.transform.position,
                    speed * Time.deltaTime
                );
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
        {
            SoldierManager.instance.AddSoldier(this);
        }
    }
}

