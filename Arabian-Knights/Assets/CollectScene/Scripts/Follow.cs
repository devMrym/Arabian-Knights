using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public float speed = 5f;
    public bool isFollowing = false;
    public float followDistance = 3f;

    private Animator anim;

    private Vector3 lastPosition;
    private Vector3 originalScale;

    private float actualSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isFollowing || Target == null)
        {
            anim.SetFloat("Speed", 0f);
            lastPosition = transform.position;
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
        }

        // Calculate REAL movement speed
        actualSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        // Send actual speed to Animator
        anim.SetFloat("Speed", actualSpeed);

        // Save position for next frame
        lastPosition = transform.position;

        if (dist > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                Target.position,
                speed * Time.deltaTime
            );

            if (Target.position.x > transform.position.x)
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            else if (Target.position.x < transform.position.x)
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
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
