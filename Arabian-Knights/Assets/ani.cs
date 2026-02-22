using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;

    void Update()
    {
        transform.position += Vector3.right * parallaxSpeed * Time.deltaTime;
    }
}