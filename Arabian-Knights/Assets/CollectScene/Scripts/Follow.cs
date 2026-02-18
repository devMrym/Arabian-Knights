using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public float speed = 5f;
    public bool isFollowing = false;
    public float followDistance = 3f;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isFollowing || Target == null)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        float dist = Vector2.Distance(transform.position, Target.position);

        if (dist > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                Target.position,
                speed * Time.deltaTime
            );

            anim.SetFloat("Speed", speed);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
        {
            GameManager.instance.AddSoldier();

            isFollowing = true;

            Target = SoldierManager.instance.GetFollowTarget();
            SoldierManager.instance.RegisterSoldier(transform);
        }
    }
}
