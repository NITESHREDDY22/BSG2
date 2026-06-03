using UnityEngine;

public class JellyUIAnimation : MonoBehaviour
{
    [Header("Jelly Settings")]
    [SerializeField] private float intensity = 0.1f; // How much it stretches
    [SerializeField] private float speed = 4f;     // How fast it moves
    
    private Vector3 originalScale;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        // Reset scale when enabled to prevent weird jumps
        rectTransform.localScale = originalScale;
    }

    private void Update()
    {
        // Calculate the Jelly/Squash and Stretch effect
        // When X gets wider, Y gets shorter (and vice versa)
        float sine = Mathf.Sin(Time.time * speed);
        
        float scaleX = originalScale.x + (sine * intensity);
        float scaleY = originalScale.y - (sine * intensity); 

        rectTransform.localScale = new Vector3(scaleX, scaleY, originalScale.z);
    }

    private void OnDisable()
    {
        // Reset scale when the popup closes
        rectTransform.localScale = originalScale;
    }
}