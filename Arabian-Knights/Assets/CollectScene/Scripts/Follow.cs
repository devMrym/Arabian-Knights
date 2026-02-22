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

    private AudioSource footstepSound;


    void Start()
    {
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
        originalScale = transform.localScale;
        footstepSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isFollowing || Target == null)
        {
            anim.SetFloat("Speed", 0f);
            lastPosition = transform.position;
            if (footstepSound.isPlaying)
                footstepSound.Stop();
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

            // Flip
            if (Target.position.x > transform.position.x)
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            else if (Target.position.x < transform.position.x)
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

            // Sound
            if (!footstepSound.isPlaying)
                footstepSound.Play();
        }
        else
        {
            if (footstepSound.isPlaying)
                footstepSound.Stop();
        }

        actualSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        anim.SetFloat("Speed", actualSpeed);
        lastPosition = transform.position;
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
