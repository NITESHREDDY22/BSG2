using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
public class SlingShot : MonoBehaviour
{
    private Vector3 slingShootMiddleVector;
    [HideInInspector]
    public SlingshotState slingShootState;
    public Transform leftSlingShootOrigin, rightSlingShootOrigin, leftOrgin2, rightOrigin2;
    public LineRenderer slingShootLineRenderer1, slingShootLineRenderer2, trajectoryLineRenderer, slingBeltRenderer;
    //[HideInInspector]
    public GameObject birdToThrow;
    public Transform birdWaitPosition;
    public float throwSpeed;
    [HideInInspector]
    public float timeSinceThrown;
    public delegate void BirdThrown();
    public event BirdThrown birdThrown;
    private Touch firstTouch;
    public Vector2 dragDist;
    bool ribbonPullSoundPlayed;

    [HideInInspector]
    public GameObject SlingBlastPrefab;


    void Awake()
    {
        try
        {
            Global.count = 0;
            Global.birdCount = 0;
            ribbonPullSoundPlayed = false;
            trajectoryLineRenderer.sortingLayerName = "Foreground";
            slingShootState = SlingshotState.Idle;
            Debug.LogError("SlingShot AWAKE CALLL....................   " + slingShootState);
            slingShootLineRenderer1.SetPosition(0, leftSlingShootOrigin.position);
            slingShootLineRenderer2.SetPosition(0, rightSlingShootOrigin.position);
            slingShootMiddleVector = new Vector3((leftSlingShootOrigin.position.x + rightSlingShootOrigin.position.x) / 2,
                (leftSlingShootOrigin.position.y + rightSlingShootOrigin.position.y) / 2, 0);
            xOrigin = slingbase.transform.position.x;
            yOrigin = slingbase.transform.position.y;
            myQuaternion = slingbase.transform.rotation;
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void Start()
    {
        try
        {
            SlingBlastPrefab = Resources.Load("SlingBlast") as GameObject;

            // Invoke("SlingBlast", 2f);
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    bool anyWhereDragEnabled = true;
    Vector3 startLocation, birdLocation;

    int fingerprintid = 0;
    void Update()
    {
        try
        {
            if (this.gameObject.activeInHierarchy && GameManager.Instance.gameState == GameState.Playing)
            {
                switch (slingShootState)
                {
                    case SlingshotState.Idle:
                        InitializeBird();
                        DisplaySlingshootLineRenderers();
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (birdToThrow == null)
                            {
                                //Debug.Log("no ball is exist.. so create one");
                                // FindObjectOfType<GameManager>().AnimateBirdToSlingshot();
                            }
                            if (anyWhereDragEnabled)
                            {
                                startLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                startLocation.z = 0;
                                if (birdToThrow != null)
                                {
                                    birdToThrow.GetComponent<CircleCollider2D>().radius = Global.BirdColliderRadiusZero;
                                    slingShootState = SlingshotState.UserPulling;
                                    Debug.LogError("SlingShot CASE IDLEIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII   " + slingShootState);
                                }
                            }
                            else
                            {
                                Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                                if (birdToThrow != null)
                                {
                                    if (birdToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                                    {
                                        birdToThrow.GetComponent<CircleCollider2D>().radius = Global.BirdColliderRadiusZero;
                                        slingShootState = SlingshotState.UserPulling;
                                        Debug.LogError("SlingShot CASE IDLEIIIIIIIIIIIIIIIIIIIIIIANYWHEREDRAGFALSE   " + slingShootState);

                                    }
                                }
                            }

                        }
                        break;
                    case SlingshotState.UserPulling:
                        SetSlingshotLinerenderersActive(true);
                        //slingShootLineRenderer1.sortingOrder = 1;
                        if (Input.touchCount > 0)
                        {
                            firstTouch = Input.GetTouch(0);
                            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                            {
                                dragDist = firstTouch.deltaPosition;
                                //Debug.Log(firstTouch.deltaPosition.magnitude / firstTouch.deltaTime + "  dragspeed");
                                if (firstTouch.deltaPosition.magnitude / firstTouch.deltaTime > 150f)
                                {
                                    if (!ribbonPullSoundPlayed)
                                    {
                                        //SoundManager.PlaySFX("Sling shot");
                                        if (!SoundManager.IsMuted())
                                        {
                                            SoundsHandler.Instance.PlaySource2Clip(2, 0);
                                        }
                                        ribbonPullSoundPlayed = true;
                                    }
                                }
                            }
                        }
                        DisplaySlingshootLineRenderers();

                        bendSlingShot();

                        if (Input.GetMouseButton(0))
                        {
                            Vector3 location;
                            float sligSize = 1.5f;
                            if (anyWhereDragEnabled)
                            {
                                location = Vector3.zero;
                                location = slingShootMiddleVector;
                                Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                loc.z = 0;
                                if ((loc - startLocation).magnitude > 0.5)
                                {
                                    location += (loc - startLocation);
                                }
                                location.z = 0f;
                            }
                            else
                            {
                                location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                location.z = 0f;
                            }

                            if (birdToThrow != null)
                            {
                                if (Vector3.Distance(location, slingShootMiddleVector) > sligSize)
                                {
                                    var maxPosition = (location - slingShootMiddleVector).normalized * sligSize + slingShootMiddleVector;
                                    birdToThrow.transform.position = maxPosition;
                                }
                                else
                                {
                                    birdToThrow.transform.position = location;
                                }
                                var distance = Vector3.Distance(slingShootMiddleVector, birdToThrow.transform.position);
                                DisplayTrajectoryLineRenderer(distance);
                            }
                        }
                        else
                        {
                            timeSinceThrown = Time.time;
                            float distance = Vector3.Distance(slingShootMiddleVector, birdToThrow.transform.position);
                            if (distance > .5f)
                            {
                                SetSlingshotLinerenderersActive(false);
                                SetTrajectoryLineRendererActive(false);
                                slingShootState = SlingshotState.BirdFlying;
                                Debug.LogError("SlingShot CASE USERPULLING....................  " + slingShootState);
                                ThrowBird(distance);
                            }
                            else
                            {
                                if (distance <= 0)
                                {
                                    distance = 0.01f;
                                }
                                birdToThrow.transform.positionTo(distance / 10, birdWaitPosition.position);
                                //birdToThrow.transform.position = birdWaitPosition.position;
                                InitializeBird();
                            }
                        }
                        break;

                    case SlingshotState.BirdFlying:
                        //bendSlingShot();
                        SetBaseToNormal();
                        break;
                }
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    private void InitializeBird()
    {
        try
        {
            if (birdToThrow != null)
            {
                birdToThrow.SetActive(true);
                birdToThrow.transform.position = birdWaitPosition.position;
                slingShootState = SlingshotState.Idle;
                //SetSlingshotLinerenderersActive (true);
                object[] anArray = Global.brokenBotList.ToArray();

                if (null != anArray && anArray.Length > 0)
                {

                    for (int i = 0; i < anArray.Length; i++)
                    {

                        ((Explodable)anArray[i]).deleteFragments();
                    }
                }
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    public void SetSlingshotLinerenderersActive(bool active)
    {
        try
        {
            slingShootLineRenderer1.enabled = active;
            slingShootLineRenderer2.enabled = active;
            slingBeltRenderer.enabled = active;
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void DisplaySlingshootLineRenderers()
    {
        try
        {
            if (birdToThrow != null)
            {
                Vector2 newPos;
                newPos.x = birdToThrow.transform.position.x + ((birdToThrow.transform.position.x - leftOrgin2.position.x) /
                    Vector2.Distance(birdToThrow.transform.position, leftOrgin2.position)) * 0.25f;
                newPos.y = birdToThrow.transform.position.y + ((birdToThrow.transform.position.y - leftOrgin2.position.y) /
                    Vector2.Distance(birdToThrow.transform.position, leftOrgin2.position)) * 0.25f;

                slingShootLineRenderer1.SetPosition(1, newPos);
                slingShootLineRenderer2.SetPosition(1, newPos);



                Vector3 beltPos;
                beltPos.x = birdToThrow.transform.position.x;
                beltPos.y = birdToThrow.transform.position.y;
                beltPos.z = 0f;
                slingBeltRenderer.SetPosition(0, beltPos);
                slingBeltRenderer.SetPosition(1, newPos);
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void SetTrajectoryLineRendererActive(bool active)
    {
        trajectoryLineRenderer.enabled = active;
    }

    Vector2 segVelocity;
    float _interval = 0.7f;
    [SerializeField] bool oldLine = true;

    void DisplayTrajectoryLineRenderer(float distance)
    {
        try
        {
            SetTrajectoryLineRendererActive(true);
            Vector3 v2 = slingShootMiddleVector - birdToThrow.transform.position;
            //v2.y += 1.5f;
            int segmentCount = 25;
            Vector2[] segments = new Vector2[segmentCount];
            segments[0] = birdToThrow.transform.position;
            segVelocity = new Vector2(v2.x, v2.y) * throwSpeed * distance;
            for (int i = 1; i < segmentCount; i++)
            {
                float time = i * Time.fixedDeltaTime * _interval;
                segments[i] = segments[0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);
            }

            if (oldLine)
            {
                trajectoryLineRenderer.SetVertexCount(segmentCount);
                for (int i = 0; i < segmentCount; i++)
                    trajectoryLineRenderer.SetPosition(i, segments[i]);
            }
            else
            {

                CreateDotsLine(segments);
            }

            float currentDist = Mathf.Abs(segments[0].x - (segments[segments.Length - 1].x));
            trajectoryLineRenderer.material.mainTextureScale = new Vector2(currentDist * .25f, 1);
            //Debug.Log("" + currentDist);
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    float maxScale = .15f, scaleDecreaseRate = 0.01f;
    [SerializeField] GameObject _dot;
    private GameObject[] spots;
    bool created = false;

    void CreateDotsLine(Vector2[] poses)
    {
        try
        {
            if (!created)
            {
                spots = new GameObject[poses.Length];

                for (int i = 0; i < poses.Length; i++)
                //for(int i = poses.Length-1;i>-1;i--)
                {
                    GameObject gob = Instantiate(_dot) as GameObject;
                    //gob.transform.localScale = new Vector3(.35f-(i*(_interval*.01f)), .35f-(i* (_interval * .01f)), 1);
                    gob.transform.localScale = new Vector3(.3f - (i * scaleDecreaseRate), .3f - (i * scaleDecreaseRate), 1);
                    spots[i] = gob;

                    //if (_ballType != BallType.tragectory)
                    //    spots[i].SetActive(false);
                }
                created = true;
                spots[0].SetActive(false);
            }
            for (int i = 0; i < poses.Length; i++)
            {
                spots[i].transform.position = new Vector3(poses[i].x, poses[i].y, 0);
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void ClearDots()
    {
        if (oldLine) return;

        foreach (GameObject go in spots)
            Destroy(go);

        created = false;
    }

    Transform[] objs;

    private void ThrowBird(float distance)
    {
        try
        {
            Physics2D.IgnoreLayerCollision(0, 9, false);
            ClearDots();
            Global.birdCount = Global.birdCount + 1;
            Vector3 velocity = slingShootMiddleVector - birdToThrow.transform.position;
            birdToThrow.GetComponent<Bird>().OnThrow();
            if (_ballType == BallType.splitballs)
            {
                GameObject gob = Instantiate(birdToThrow) as GameObject;
                gob.transform.position += gob.transform.up * .1f;
                CalcBallVelocity(gob, velocity, distance);
                Physics2D.IgnoreCollision(birdToThrow.GetComponent<Collider2D>(), gob.GetComponent<Collider2D>());

            }
            //birdToThrow.GetComponent<Rigidbody2D> ().velocity = new Vector2 (velocity.x, velocity.y) * throwSpeed * distance;
            CalcBallVelocity(birdToThrow, velocity, distance);
            if (birdThrown != null)
                birdThrown();
            ribbonPullSoundPlayed = false;
            if (BirdAnimations.Instance != null)
            {
                objs = BirdAnimations.Instance.GetComponentsInChildren<Transform>();
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm.currentBirdIndex == 1)
                    foreach (Transform tr in objs)
                    {
                        tr.transform.localPosition = new Vector2(4, 1);
                    }
                if (gm.currentBirdIndex == 2)
                    foreach (Transform tr in objs)
                    {
                        tr.transform.localPosition = new Vector2(-2, 2);
                    }
                if (gm.currentBirdIndex == 3)
                    foreach (Transform tr in objs)
                    {
                        tr.transform.localPosition = new Vector2(3, -1);
                    }
                if (gm.currentBirdIndex == 4)
                    foreach (Transform tr in objs)
                    {
                        tr.transform.localPosition = new Vector2(-1, 3);
                    }
                BirdAnimations.Instance.PlayAnimations();
            }
            if (_ballType == BallType.magic)
            {
                Physics2D.IgnoreLayerCollision(0, 9);
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    void CalcBallVelocity(GameObject birdToThrow, Vector2 velocity, float distance)
    {
        try
        {
            if (_ballType == BallType.magic)
                birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * (throwSpeed) * distance;
            else if (_ballType == BallType.splitballs)
            {
                birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * (throwSpeed) * (distance);
            }
            else if (_ballType == BallType.stone)
            {
                birdToThrow.GetComponent<Rigidbody2D>().mass = 60;
                birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * (throwSpeed) * distance;
            }
            else if (_ballType == BallType.bomb)
            {
                //birdToThrow.GetComponent<Rigidbody2D>().mass = 60;
                birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * (throwSpeed) * distance;
            }
            else if (_ballType == BallType.tragectory)
            {
                //birdToThrow.GetComponent<Rigidbody2D>().mass = 60;
                birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * (throwSpeed) * distance;
            }
            else if (_ballType == BallType.normal)
            {
                //birdToThrow.GetComponent<Rigidbody2D>().mass = 60;
                birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * (throwSpeed) * distance;
            }
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }

    [SerializeField] Transform slingbase;
    float xOrigin = 0, yOrigin = 0;
    public bool isLeftSide = true;
    void bendSlingShot()
    {
        slingShootLineRenderer1.SetPosition(0, leftSlingShootOrigin.position);
        slingShootLineRenderer2.SetPosition(0, rightSlingShootOrigin.position);
        slingShootMiddleVector = new Vector3((leftSlingShootOrigin.position.x + rightSlingShootOrigin.position.x) / 2,
            (leftSlingShootOrigin.position.y + rightSlingShootOrigin.position.y) / 2, 0);

        if(birdToThrow!=null){
        Vector2 newPos = Vector2.zero;
        Vector3 oldBirdToThrowPos = birdToThrow.transform.position;
        Vector3 oldLeftOrigin2 = leftOrgin2.position;
        newPos.x = birdToThrow.transform.position.x + ((birdToThrow.transform.position.x - leftOrgin2.position.x) /
            Vector2.Distance(birdToThrow.transform.position, leftOrgin2.position)) * 0.25f;
        //Debug.Log("BirdPosition.. " + birdToThrow.transform.position.x + "\n" + "leftOrigin2.. " + leftOrgin2.position.x + "\n");
        //Debug.Log("sling line " + newPos.x+" origin "+xOrigin);


        if (newPos.x < (xOrigin - .1f))
        {
            slingbase.eulerAngles = new Vector3(0, 0, newPos.x * (isLeftSide ? -3 : 8));
            birdToThrow.transform.position = oldBirdToThrowPos;
            leftOrgin2.position = oldLeftOrigin2;
        }
        else if (newPos.x > (xOrigin + .1f))
        {
            slingbase.eulerAngles = new Vector3(0, 0, newPos.x * (!isLeftSide ? -3 : 8));
            birdToThrow.transform.position = oldBirdToThrowPos;
            leftOrgin2.position = oldLeftOrigin2;
        }
        }
        // Debug.Log("minus  = " + (xOrigin - .1f) + "\n" + (xOrigin + .1f));
        once = false;
    }
    bool once = false;
    Quaternion myQuaternion;
    void SetBaseToNormal()
    {
        //slingbase.eulerAngles = Vector3.Lerp(slingbase.eulerAngles, Vector3.zero, Time.deltaTime * 30);
        if (!once)
        {
            if(birdToThrow!=null){
            birdToThrow.transform.SetParent(null);
            }
            slingbase.DORotate(myQuaternion.eulerAngles, 1).SetEase(Ease.OutElastic);
            once = true;
        }
    }

    [SerializeField] public Sprite normalSkin, splitSkin, metalSkin, bombSkin, tragectSkin, magicSkin;
    [SerializeField] public GameObject blastEffect;
    public delegate void changeBallSkin();
    public static event changeBallSkin _changeSkinEvent;
    public void ChangeBallType(int serial)
    {
        //FindObjectOfType<GameManager>().AnimateBirdToSlingshot();
        if (!GameObject.FindObjectOfType<GameManager>().ChangeBallCounts(serial))
        {
            return;
        }

        switch (serial)
        {
            case 0:
                _ballType = BallType.tragectory;
                break;
            case 1:
                _ballType = BallType.stone;
                break;
            case 2:
                _ballType = BallType.splitballs;
                break;
            case 3:
                _ballType = BallType.bomb;
                break;
            case 4:
                _ballType = BallType.magic;
                break;
        }

        //if (_changeSkinEvent != null)
        //    _changeSkinEvent();

        //StartCoroutine(changeSkin());
        FindObjectOfType<GameManager>().getBall().GetComponent<Bird>().ChangeSkin();
        ChangeDisplayBallSkin();
    }

    [SerializeField] UnityEngine.UI.Image displayBall;
    public void ChangeDisplayBallSkin()
    {
        if (_ballType == SlingShot.BallType.magic)
            displayBall.sprite = magicSkin;
        if (_ballType == SlingShot.BallType.splitballs)
            displayBall.sprite = splitSkin;
        if (_ballType == SlingShot.BallType.stone)
            displayBall.sprite = metalSkin;
        if (_ballType == SlingShot.BallType.bomb)
            displayBall.sprite = bombSkin;
        if (_ballType == SlingShot.BallType.tragectory)
            displayBall.sprite = tragectSkin;
        if (_ballType == SlingShot.BallType.normal)
            displayBall.sprite = normalSkin;
    }
    IEnumerator changeSkin()
    {
        yield return new WaitForSeconds(.5f);
        birdToThrow.GetComponent<Bird>().ChangeSkin();
    }

    public BallType _ballType = BallType.normal;
    public enum BallType
    {
        magic, splitballs, stone, bomb, tragectory, normal
    }

    public void SlingBlast()
    {
        try
        {
            if (GameManager.Instance.slingShot.birdToThrow != null)
            {
                GameManager.Instance.slingShot.birdToThrow.GetComponent<SpriteRenderer>().enabled = false;

            }
            leftOrgin2.parent.GetComponent<SpriteRenderer>().enabled = false;
            rightOrigin2.parent.GetComponent<SpriteRenderer>().enabled = false;

            slingShootLineRenderer1.enabled = false;
            slingShootLineRenderer2.enabled = false;
            Transform levelnode = transform.parent;
            GameObject SB = Instantiate(SlingBlastPrefab, levelnode, true);
            SB.transform.localPosition = transform.localPosition;
            SB.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        }
        catch (Exception exp)
        {
            try
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exp, true);
                var stackFrame = trace.GetFrame(trace.FrameCount - 1);
                var lineNumber = stackFrame.GetFileLineNumber();
                string errorline = "Line:" + lineNumber;
                if (lineNumber == 0)
                {
                    int index = exp.ToString().IndexOf("at");
                    int length = exp.ToString().Substring(index).Length;
                    if (length > 99)
                    {
                        errorline = "Line:" + exp.ToString().Substring(index, 100);
                    }
                    else
                    {
                        errorline = "Line2:" + exp.ToString().Substring(index);
                    }
                }
                if (FirebaseEvents.instance != null)
                {
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "Slinghshot", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
} // SlingShot