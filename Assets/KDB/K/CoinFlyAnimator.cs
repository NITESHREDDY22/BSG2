using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinFlyAnimator : MonoBehaviour
{
    [Header("References")]
    public RectTransform coinPrefab;

    [Header("Settings")]
    public int coinCount = 10;
    public float spreadRadius = 120f;
    public float flyDuration = 0.6f;
    public AnimationCurve moveCurve;
    public AnimationCurve scaleCurve;

    public static CoinFlyAnimator Instance;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(RectTransform startScreenPos, RectTransform target,Transform canvasRoot, System.Action onComplete = null)
    {
        StartCoroutine(AnimateCoins(startScreenPos, target,canvasRoot, onComplete));
    }

    IEnumerator AnimateCoins(
    RectTransform from,
    RectTransform to,
    Transform canvasRoot,
    System.Action onComplete)
{
    int completed = 0;

    Vector2 startPos;
    Vector2 endPos;

    RectTransform canvasRect = canvasRoot as RectTransform;

    // Convert to local UI space
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect,
        from.position,
        null,
        out startPos);

    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect,
        to.position,
        null,
        out endPos);

    for (int i = 0; i < coinCount; i++)
    {
        RectTransform coin = Instantiate(coinPrefab, canvasRect);
        coin.gameObject.SetActive(true);
        coin.anchoredPosition = startPos;
        coin.localScale = Vector3.zero;

        Vector2 midPoint = startPos + Random.insideUnitCircle * spreadRadius;

        StartCoroutine(FlyCoinUI(
            coin,
            startPos,
            midPoint,
            endPos,
            () =>
            {
                Destroy(coin.gameObject);
                completed++;
                if (completed == coinCount)
                    onComplete?.Invoke();
            }));

        yield return new WaitForSeconds(0.1f);
    }
}


    IEnumerator FlyCoinUI(
    RectTransform coin,
    Vector2 start,
    Vector2 mid,
    Vector2 end,
    System.Action onDone)
{
    float t = 0f;

    while (t < flyDuration)
    {
        float p = t / flyDuration;

        Vector2 a = Vector2.Lerp(start, mid, p);
        Vector2 b = Vector2.Lerp(mid, end, p);
        coin.anchoredPosition = Vector2.Lerp(a, b, moveCurve.Evaluate(p));

        coin.localScale = Vector3.one * scaleCurve.Evaluate(p);

        t += Time.deltaTime;
        yield return null;
    }

    coin.anchoredPosition = end; // snap to final
    onDone?.Invoke();
}

}
