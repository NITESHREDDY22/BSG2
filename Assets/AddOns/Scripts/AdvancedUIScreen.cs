using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AdvancedUIScreen : MonoBehaviour
{
    [System.Serializable]
    public class UIElement
    {
        public RectTransform transform;
        public float delay = 0.1f;
        public float duration = 0.5f;
        public Vector2 slideOffset = new Vector2(0, -50);
        public bool useFade = true;
        public bool useScale = true;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    [Header("Main Panel Settings")]
    public float panelFadeDuration = 0.3f;
    
    [Header("Child Elements (Staggered)")]
    public List<UIElement> elements = new List<UIElement>();

    private CanvasGroup mainCanvasGroup;

    void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
    }

    [ContextMenu("Play Show Animation")]
    public void Show()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(AnimateScreen(true));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScreen(false));
    }

    private IEnumerator AnimateScreen(bool opening)
    {
        // 1. Animate Main Panel Alpha
        float timer = 0;
        float startAlpha = mainCanvasGroup.alpha;
        float endAlpha = opening ? 1 : 0;

        while (timer < panelFadeDuration)
        {
            timer += Time.deltaTime;
            mainCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / panelFadeDuration);
            
            // Start child animations halfway through the panel fade for a "fluid" feel
            if (opening && timer >= panelFadeDuration * 0.5f) break; 
            yield return null;
        }

        // 2. Animate Child Elements
        if (opening)
        {
            foreach (var element in elements)
            {
                StartCoroutine(AnimateSingleElement(element, true));
                yield return new WaitForSeconds(element.delay); // The "Stagger" effect
            }
        }
        else
        {
            // When hiding, animate all children at once or in reverse
            foreach (var element in elements)
            {
                StartCoroutine(AnimateSingleElement(element, false));
            }
            yield return new WaitForSeconds(0.5f); // Wait for children to finish
            gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimateSingleElement(UIElement el, bool opening)
    {
        if (el.transform == null) yield break;

        CanvasGroup cg = el.transform.GetComponent<CanvasGroup>();
        if (cg == null) cg = el.transform.gameObject.AddComponent<CanvasGroup>();

        Vector2 startPos = opening ? el.slideOffset : Vector2.zero;
        Vector2 endPos = opening ? Vector2.zero : el.slideOffset;
        Vector3 startScale = opening ? Vector3.zero : Vector3.one;
        Vector3 endScale = opening ? Vector3.one : Vector3.zero;

        float timer = 0;
        while (timer < el.duration)
        {
            timer += Time.deltaTime;
            float t = el.curve.Evaluate(timer / el.duration);

            el.transform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            if (el.useScale) el.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            if (el.useFade) cg.alpha = opening ? t : 1 - t;

            yield return null;
        }
    }
}