using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIEntranceSequencer : MonoBehaviour
{
    [System.Serializable]
    public class AnimationElement
    {
        public string label;
        public RectTransform rectTransform;
        public float duration = 0.6f;
        public float staggerDelay = 0.08f; 
        
        [Header("Motion")]
        public Vector2 slideOffset = new Vector2(0, -150);
        public float startRotation = -15f; 
        public Ease moveEase = Ease.OutBack;
        
        [Header("Visuals")]
        public bool useScale = true;
        public bool useFade = true;

        [HideInInspector] public Vector2 homePosition;
        [HideInInspector] public CanvasGroup canvasGroup;
    }

    [Header("Layer References")]
    [SerializeField] private CanvasGroup _backgroundTint;
    [SerializeField] private RectTransform _uiGroupContainer; 

    [Header("Phase 1: Background")]
    [SerializeField] private float _bgFadeDuration = 0.4f;
    
    [Header("Transition")]
    [SerializeField] private float _delayBetweenPhase1And2 = 0.15f;

    [Header("Phase 2: UI Group Juice")]
    [SerializeField] private float _groupPunchAmount = 0.1f; 
    [SerializeField] private bool _enableIdleFloat = true; // New option
    [SerializeField] private bool _enableMouseTilt = true; // New option

    [Header("Phase 3: Elements")]
    [SerializeField] private List<AnimationElement> _elements = new List<AnimationElement>();

    private CanvasGroup _uiGroupCanvas;
    private bool _hasInitialized = false; 
    private Sequence _activeSequence;
    private Vector2 _originalGroupPos;
    public bool hidBanner = false;


    private void OnEnable()
    {
        InitializeOnce();

        if (_activeSequence != null) _activeSequence.Kill();
        DOTween.Kill(_uiGroupContainer); // Kill idle tweens
        StopAllCoroutines();
        
        ResetUIImmediate();
        StartCoroutine(PlaySequenceWithDelay());

        if (hidBanner && AdManager._instance)
        {
            AdManager._instance.HidebannerAd();
        }
    }

    private void InitializeOnce()
    {
        if (_hasInitialized) return;

        if (_uiGroupContainer != null)
        {
            _uiGroupCanvas = _uiGroupContainer.GetComponent<CanvasGroup>();
            _originalGroupPos = _uiGroupContainer.anchoredPosition;
        }

        foreach (var item in _elements)
        {
            if (item.rectTransform != null)
            {
                item.homePosition = item.rectTransform.anchoredPosition;
                item.canvasGroup = item.rectTransform.GetComponent<CanvasGroup>() ?? item.rectTransform.gameObject.AddComponent<CanvasGroup>();
            }
        }
        _hasInitialized = true;
    }

    private void ResetUIImmediate()
    {
        if (_backgroundTint != null) _backgroundTint.alpha = 0;
        if (_uiGroupCanvas != null) _uiGroupCanvas.alpha = 0;
        
        if (_uiGroupContainer != null)
        {
            _uiGroupContainer.localScale = Vector3.one;
            _uiGroupContainer.anchoredPosition = _originalGroupPos;
            _uiGroupContainer.localRotation = Quaternion.identity;
        }

        foreach (var item in _elements)
        {
            if (item.rectTransform == null) continue;
            item.rectTransform.DOKill();
            item.rectTransform.anchoredPosition = item.homePosition + item.slideOffset;
            item.rectTransform.localRotation = Quaternion.Euler(0, 0, item.startRotation);
            if (item.useScale) item.rectTransform.localScale = Vector3.zero;
            if (item.useFade && item.canvasGroup != null) item.canvasGroup.alpha = 0;
        }
    }

    private IEnumerator PlaySequenceWithDelay()
    {
        yield return new WaitForEndOfFrame();

        _activeSequence = DOTween.Sequence();

        // 1. BG Fade
        if (_backgroundTint != null)
            _activeSequence.Insert(0, _backgroundTint.DOFade(1, _bgFadeDuration).SetEase(Ease.Linear));

        float uiStartTime = _bgFadeDuration + _delayBetweenPhase1And2;

        // 2. ENHANCED GROUP JUICE (Reusing _groupPunchAmount for multiple effects)
        if (_uiGroupContainer != null)
        {
            _activeSequence.Insert(uiStartTime, _uiGroupCanvas.DOFade(1, 0.2f));
            
            // Scale Punch
            _activeSequence.Insert(uiStartTime, _uiGroupContainer.DOPunchScale(Vector3.one * _groupPunchAmount, 0.6f, 5, 0.5f));
            
            // Positional "Land" (Dips down and up)
            _activeSequence.Insert(uiStartTime, _uiGroupContainer.DOPunchPosition(Vector3.down * (_groupPunchAmount * 100f), 0.6f, 8, 0.5f));
            
            // Rotation Wobble
            _activeSequence.Insert(uiStartTime, _uiGroupContainer.DOPunchRotation(new Vector3(0, 0, _groupPunchAmount * 20f), 0.6f, 5, 0.5f));
        }

        // 3. Staggered Elements
        float elementAccumulator = 0;
        foreach (var item in _elements)
        {
            if (item.rectTransform == null) continue;

            elementAccumulator += item.staggerDelay;
            float absoluteStartTime = uiStartTime + elementAccumulator;

            _activeSequence.Insert(absoluteStartTime, item.rectTransform.DOAnchorPos(item.homePosition, item.duration).SetEase(item.moveEase));
            _activeSequence.Insert(absoluteStartTime, item.rectTransform.DOLocalRotate(Vector3.zero, item.duration).SetEase(Ease.OutBack));
            
            if (item.useScale)
                _activeSequence.Insert(absoluteStartTime, item.rectTransform.DOScale(Vector3.one, item.duration).SetEase(Ease.OutBack));

            if (item.useFade && item.canvasGroup != null)
                _activeSequence.Insert(absoluteStartTime, item.canvasGroup.DOFade(1, item.duration * 0.5f));
        }

        _activeSequence.SetUpdate(true);
        _activeSequence.OnComplete(() => {
            if (_enableIdleFloat) StartIdleFloat();
        });
    }

    private void StartIdleFloat()
    {
        // Subtle breathing motion
        _uiGroupContainer.DOAnchorPos(_originalGroupPos + new Vector2(0, 10f), 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    private void Update()
    {
        if (!_enableMouseTilt || !_hasInitialized || _uiGroupCanvas == null || _uiGroupCanvas.alpha < 0.5f) return;

        // Subtle tilt parallax based on mouse position
        float xFactor = (Input.mousePosition.x / Screen.width) - 0.5f;
        float yFactor = (Input.mousePosition.y / Screen.height) - 0.5f;

        Quaternion targetRot = Quaternion.Euler(yFactor * -8f, xFactor * 8f, 0);
        _uiGroupContainer.localRotation = Quaternion.Slerp(_uiGroupContainer.localRotation, targetRot, Time.unscaledDeltaTime * 5f);
    }

    private void OnDisable()
    {
        if (_activeSequence != null) _activeSequence.Kill();
        if (_uiGroupContainer != null) _uiGroupContainer.DOKill();
    }
}