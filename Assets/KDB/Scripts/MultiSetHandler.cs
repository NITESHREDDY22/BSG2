using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MultiSetHandler : MonoBehaviour
{
    public static Action OnBottlesBreak;
    public static Action OnBotlleAnimation;
    public static Action OnSetChanged;
    public CameraFollow CameraFollow;
    public int targetSet;
    private int currentSet;
    private int totalBottlesBroke;
    public int currentSetTargetCount;
    public SlingShot slingShot;
    public Transform slingShotBalls;
    public List<MultiSet> multiSets;
    public List<GameObject> allBottles;

    private Vector3 cacheCameraStartPostion;
    [System.Serializable]
    public class MultiSet
    {
        public Transform slingShotTargetPosition;
        public int setTargetBottleCount;
        public bool isAnimationDone;
    }

    private void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            if(CameraFollow == null)
            CameraFollow = gameManager.GetComponentInChildren<CameraFollow>();
            if(slingShot == null)
            slingShot = gameManager.GetComponentInChildren<SlingShot>();
            if(slingShotBalls == null)
            slingShotBalls = gameManager.birdsgroup.transform;
        }
        if (multiSets.Count>0 && allBottles.Count <= 0)
        {
            // Get the parent transform
            Transform parentTransform = multiSets[0].slingShotTargetPosition.parent;

            // Find all transforms in children, then filter by tag
            if(allBottles == null || allBottles.Count <= 0)
            {
                allBottles = parentTransform.GetComponentsInChildren<Transform>()
                    .Where(t => t.CompareTag(bottleTag))
                    .Select(t => t.gameObject)
                    .ToList();
                multiSets[0].setTargetBottleCount = allBottles.Count;
            }
        }
        RefreshAndAnimateAllBottles();
    }
    private void OnEnable()
    {
        OnBottlesBreak += OnTargetBottlesBreak;
        OnBotlleAnimation += SetBottlesAnimation;
        if(multiSets.Count>0)
        {
            currentSetTargetCount = multiSets[0].setTargetBottleCount;
        }
        TutorialOverlay.OnTutorialClosed += OnTutorialClosed;
    }

    private void OnTutorialClosed(bool obj)
    {
        addedDelay = 1;
        RefreshAndAnimateAllBottles();

    }
    private void OnDisable()
    {
        OnBottlesBreak -= OnTargetBottlesBreak;
        OnBotlleAnimation -= SetBottlesAnimation;
        TutorialOverlay.OnTutorialClosed -= OnTutorialClosed;

    }


    private void OnTargetBottlesBreak()
    {
        if(multiSets.Count<=0)return;

        totalBottlesBroke++;
        if (currentSet>targetSet)
        {
            return;
        }

        if (currentSetTargetCount <= totalBottlesBroke)
        {
            Vector3 pos= multiSets[currentSet].slingShotTargetPosition.position;
            slingShot.transform.position = pos;
            Vector3 slingshotballpos = slingShotBalls.transform.position;
            slingshotballpos.x = pos.x - 10;
            slingShotBalls.transform.position = slingshotballpos;

            for (int i = 0; i < allBottles.Count; i++)
            {
                allBottles[i].GetComponent<Rigidbody2D>().isKinematic = true;
                allBottles[i].transform.localScale= Vector3.zero;
            }
            
            cacheCameraStartPostion = pos;
           // OnSetChanged?.Invoke();            
            currentSet++;
            StartCoroutine(checkCameraPosition());
        }
    }

    private IEnumerator checkCameraPosition()
    {
        yield return new WaitForSeconds(1);
        CameraFollow.ResetCameraTargetPosition(cacheCameraStartPostion);
        if (cacheCameraStartPostion != CameraFollow.transform.position && !GameManager.Instance.isCameraTransition)
        {
            SetBottlesAnimation();
            //GameManager.Instance.ForceMoveCamera();
            //cacheCameraStartPostion = Vector3.zero;
        }
        /*
        while (cacheCameraStartPostion != Vector3.zero)
        {
            if (cacheCameraStartPostion != Vector3.zero)
            {
                if (cacheCameraStartPostion != CameraFollow.startingPosition)
                {
                    OnSetChanged?.Invoke();
                    GameManager.Instance.resetFlag();
                    GameManager.Instance.AnimateCameraToStartPosition();
                    cacheCameraStartPostion = Vector3.zero;
                }
            }
            yield return new WaitForSeconds(1);
        }*/
    }

    private void SetBottlesAnimation()
    {
        if (currentSet < 1)
            return;

        bool flag = multiSets[currentSet - 1].isAnimationDone;
        if (flag)
            return;

        for (int i = 0; i < allBottles.Count; i++)
        {           
              allBottles[i].transform.DOScale(1, (i + 1) * 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
            });

            allBottles[i].GetComponent<Rigidbody2D>().isKinematic = false;

        }
        multiSets[currentSet - 1].isAnimationDone = true;
    }


    [Header("Auto Animation Settings")]
    private string bottleTag = "pig";
    private float staggerDelay = .3f;
    private float animationDuration = 0.5f;

    /// <summary>
    /// Finds all bottles in the scene, adds them to the list, 
    /// and plays the scale-up bounce animation.
    /// </summary>
    float addedDelay = 0;
    public void RefreshAndAnimateAllBottles()
    {
        GameObject[] foundBottles = GameObject.FindGameObjectsWithTag(bottleTag);

        for (int i = 0; i < foundBottles.Length; i++)
        {
            GameObject bottle = foundBottles[i];
            // --- ADD THIS CHECK ---
            if (bottle.TryGetComponent<DoNotAnimate>(out _)) continue;

            Rigidbody2D rb = bottle.GetComponent<Rigidbody2D>();

            bottle.transform.localScale = Vector3.zero;
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
            }

            bottle.transform.DOScale(Vector3.one, animationDuration)
                .SetDelay(i * staggerDelay+addedDelay)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.velocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                    }
                });
        }
    }
}
