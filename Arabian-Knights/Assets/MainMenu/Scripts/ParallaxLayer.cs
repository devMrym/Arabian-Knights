using UnityEngine;

public class ParallaxLayer1 : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    private RectTransform rectTransform;
    private float startX;
    private float width;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startX = rectTransform.anchoredPosition.x;
        width = rectTransform.rect.width;
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.right * parallaxSpeed * Time.deltaTime;

        // Loop back when it moves too far
        if (rectTransform.anchoredPosition.x > startX + width)
            rectTransform.anchoredPosition = new Vector2(startX, rectTransform.anchoredPosition.y);
    }
}