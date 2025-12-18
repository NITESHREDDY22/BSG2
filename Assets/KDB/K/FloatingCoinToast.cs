using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FloatingCoinToast : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI scoreText;
    public CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    public float moveDistance = 50f;
    public float duration = 0.7f;
    public float startScale = 0.6f;
    public float endScale = 1.2f;

    private Vector3 startPos;

    public void Play(string text)
    {
        scoreText.text = text;
        startPos = transform.localPosition;
        canvasGroup.alpha = 1f;
        transform.localScale = Vector3.one * startScale;

        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            // Move upward
            transform.localPosition = startPos + Vector3.up * (moveDistance * normalized);

            // Scale
            float scale = Mathf.Lerp(startScale, endScale, normalized);
            transform.localScale = Vector3.one * scale;

            // Fade
            canvasGroup.alpha = 1f - normalized;

            yield return null;
        }

        Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (amount));
    }


    #if UNITY_EDITOR
    /// <summary>
    /// Editor-only preview, allows manual animation testing.
    /// </summary>
    public void EditorPreview()
    {
        if (!Application.isPlaying)
        {
            // Reset visually in editor
            UnityEditor.EditorApplication.delayCall += () =>
            {
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one * startScale;
                canvasGroup.alpha = 1f;
            };
        }

        Play("+20");
    }
#endif
}
