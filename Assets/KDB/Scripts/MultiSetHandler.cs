using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
    private void OnEnable()
    {
        OnBottlesBreak += OnTargetBottlesBreak;
        OnBotlleAnimation += SetBottlesAnimation;
        currentSetTargetCount = multiSets[0].setTargetBottleCount;
    }

    

    private void OnDisable()
    {
        OnBottlesBreak -= OnTargetBottlesBreak;
        OnBotlleAnimation -= SetBottlesAnimation;


    }


    private void OnTargetBottlesBreak()
    {
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
}
