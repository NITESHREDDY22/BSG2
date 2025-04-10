using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.UIElements.GraphView;
using System;

public class Bird : MonoBehaviour
{

    public BirdState birdState { set; get; }
    private TrailRenderer lineRenderer;
    private Rigidbody2D myBody;
    private CircleCollider2D myCollider;
    private float orthoOrg;
    private float orthoCurr;
    private bool isfire;
    [HideInInspector]
    public Vector3 posOrg, birdPos;
    GameObject gob;
    ParticleSystem fire;
    ParticleSystem.EmissionModule fireEm;
    TrailRenderer magicTrail;
    public Sprite[] BallsSkins;

    void Awake()
    {
        InitializeVariables();
        gob = Instantiate(Resources.Load("Flames")) as GameObject;
        gob.SetActive(false);
        gob.transform.SetParent(transform);
        fire = gob.GetComponent<ParticleSystem>();
        fireEm = gob.GetComponent<ParticleSystem>().emission;
        fireEm.enabled = false;
        fire.transform.localPosition = Vector3.zero;
        fire.transform.localScale = Vector3.one;
        if (transform.Find("magicTrail") != null)
        {
            magicTrail = transform.Find("magicTrail").GetComponent<TrailRenderer>();
            if (magicTrail)
                magicTrail.enabled = false;
        }
    }

    void Start()
    {
        try
        {
            GetComponent<SpriteRenderer>().sprite = BallsSkins[PlayerPrefs.GetInt("BallSelected", 0)];
            isfire = true;
            gob.SetActive(isfire);
            fireEm.enabled = isfire;
            orthoOrg = Camera.main.orthographicSize;//+0.5f;
            orthoCurr = orthoOrg;
            posOrg = Camera.main.WorldToViewportPoint(transform.position);
            birdPos = myBody.transform.position;
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
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "bird_start", exp.Message + "at " + errorline);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

    }

    void Update()
    {
        var osize = Camera.main.orthographicSize;
        if (orthoCurr != osize)
        {
            orthoCurr = osize;
            if (birdState == BirdState.BeforeThrown)
            {
                transform.position = Camera.main.ViewportToWorldPoint(posOrg);
            }
        }

        if (isfire)
        {
            fireEm.enabled = (myBody.velocity.normalized.x > .7f) && (myBody.transform.position.x > birdPos.x + 8);
        }
    }

    int ballCollsionCount = 0, groundCollisionCount = 0;

    float maximumVelo = 0;


    void OnCollisionEnter2D(Collision2D target)
    {
        isfire = false;
        fireEm.enabled =isfire;
        gob.SetActive(isfire);
        if (transform.position.x > -14f && transform.position.x < 25)
        {
            if (!(new List<string> { "ground", "pig", "borderwall" }).Contains(target.gameObject.tag))
            {
                ballCollsionCount += 1;
                if (myBody.velocity.magnitude > maximumVelo)
                {
                    maximumVelo = myBody.velocity.magnitude;
                }
                if (ballCollsionCount < 5 && myBody.velocity.magnitude > 1.5f)
                {
                    float p = myBody.velocity.sqrMagnitude / 10f;
                    //SoundManager.PlaySFX("GroundTap", false, 0, Mathf.Clamp(p, 0.1f, 4.0f), myBody.velocity.magnitude / 0.1f);
                }

            }
            if (target.gameObject.tag == "enemy")
            {
                //call enemy damage..
                if (target.gameObject.GetComponent<Enemy>() != null)
                {
                    target.gameObject.GetComponent<Enemy>().Damage();
                }
            }

         
            if (target.gameObject.tag == "ground")
            {
                groundCollisionCount += 1;
                if (groundCollisionCount < 7 && myBody.velocity.magnitude > 1.5f)
                {
                    float p = myBody.velocity.sqrMagnitude / 10f;
                    //SoundManager.PlaySFX("WoodTap", false, 0, Mathf.Clamp(p, 0.1f, 4.0f));

                }
            }
            if (target.gameObject.tag == "laser")
            {
                //SoundManager.PlaySFX(target.gameObject.GetComponent<LaserEnemy>().CollisionSFX);
                GameObject Go = Instantiate(target.gameObject.GetComponent<LaserEnemy>().CollisionParticleFX, gameObject.transform.position, Quaternion.identity);
                Go.transform.parent = target.transform.parent;
            }
            if (target.gameObject.tag == "portal")
            {
                target.gameObject.GetComponent<portal>().ballaccleration = (GetComponent<Rigidbody2D>().velocity.magnitude / Time.deltaTime);
                target.gameObject.GetComponent<portal>().getObjectin(this.gameObject);
            }
            if (target.gameObject.tag == "bomb")
            {
                target.gameObject.GetComponent<BombScript>().explode(true);
            }
            if (target.gameObject.tag == "pig")
            {
                bool first = true;
                if (first)
                {
                    //SoundManager.PlaySFX("bottle_bottle2");
                    first = false;
                }

                BlastEffect(target.contacts[0].point);

                if (FindObjectOfType<SlingShot>()._ballType == SlingShot.BallType.stone)
                {
                    if (target.transform.GetComponent<Explodable>())
                    {
                        target.transform.GetComponent<Explodable>().explode();
                    }
                }
            }
            if (target.gameObject.CompareTag("Brick"))
            {
                BlastEffect(target.contacts[0].point);
                Debug.Log(" hits brick " + FindObjectOfType<SlingShot>()._ballType);
                if (FindObjectOfType<SlingShot>()._ballType == SlingShot.BallType.stone)
                {
                    target.transform.GetComponent<Rigidbody2D>().isKinematic = false;
                    target.transform.GetComponent<Explodable>().explode();
                }
            }
        }
    }

    IEnumerator keyaction(GameObject t)
    {
        t.GetComponent<keyObj>().callkeycageAction();
        yield return new WaitForSeconds(1f);
        t.SetActive(false);
    }

    private static bool ballsCreated = false;

    void InitializeVariables()
    {
        lineRenderer = GetComponent<TrailRenderer>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        lineRenderer.enabled = false;
        lineRenderer.sortingLayerName = "Foreground";
        myBody.isKinematic = true;
        myCollider.radius = Global.BirdColliderRadiusBig;
        birdState = BirdState.BeforeThrown;
    }

    public void OnThrow()
    {
        //SoundManager.PlaySFX("Whoosh Sound");
        if(!SoundManager.IsMuted())
        {
            SoundsHandler.Instance.PlaySource2Clip(5, 0.2f);
        }
        lineRenderer.enabled = true;
        myBody.isKinematic = false;
        myCollider.radius = Global.BirdColliderRadiusNormal;
        birdState = BirdState.Thrown;
        if (birdState == BirdState.BeforeThrown)
        {
          transform.position = Camera.main.ViewportToWorldPoint(posOrg);
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        Destroy(gameObject);
        yield return new WaitForSeconds(delay);
        object[] anArray = Global.brokenBotList.ToArray();
        if (null != anArray && anArray.Length > 0)
        {
            for (int i = 0; i < anArray.Length; i++)
            {
                ((Explodable)anArray[i]).deleteFragments();
            }
        }
    }
   

    public void ChangeSkin()
    {
        SlingShot _sligShot = FindObjectOfType<SlingShot>();
       // Debug.Log(" ChangeSkin " + _sligShot);
        if (_sligShot._ballType == SlingShot.BallType.magic)
            GetComponent<SpriteRenderer>().sprite = _sligShot.magicSkin;
        if (_sligShot._ballType == SlingShot.BallType.splitballs)
            GetComponent<SpriteRenderer>().sprite = _sligShot.splitSkin;
        if (_sligShot._ballType == SlingShot.BallType.stone)
            GetComponent<SpriteRenderer>().sprite = _sligShot.metalSkin;
        if (_sligShot._ballType == SlingShot.BallType.bomb)
            GetComponent<SpriteRenderer>().sprite = _sligShot.bombSkin;
        if (_sligShot._ballType == SlingShot.BallType.tragectory)
            GetComponent<SpriteRenderer>().sprite = _sligShot.tragectSkin;
        if (_sligShot._ballType == SlingShot.BallType.normal)
            GetComponent<SpriteRenderer>().sprite = _sligShot.normalSkin;
    }
    bool isBlasted = false;
    void BlastEffect(Vector3 pos)
    {
        if (isBlasted) return;

        Debug.Log("Blast effect");
        SlingShot _sligShot = FindObjectOfType<SlingShot>();
        if (_sligShot._ballType == SlingShot.BallType.bomb)
        {
            GameObject gob = Instantiate(_sligShot.blastEffect, pos, Quaternion.identity) as GameObject;
            Destroy(gob, 2);
            List<Explodable> expls = nearExplodables();
            foreach (Explodable exp in expls)
            {
                if (exp.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static)
                    exp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                exp.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.right * 200, exp.transform.position);
            }
            //exp.explode();
        }
        isBlasted = true;
    }

    List<Explodable> nearExplodables()
    {
        Explodable[] expls = GameObject.FindObjectsOfType<Explodable>();
        List<Explodable> near = new List<Explodable>();
        foreach (Explodable exp in expls)
        {
            if (Vector3.Distance(exp.transform.position, transform.position) < 20)
            {
                near.Add(exp);
            }
        }

        return near;
    }

    public void AddSkinEvent()
    {
        SlingShot._changeSkinEvent += ChangeSkin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(" triggers to  " + collision.tag);
        if (collision.CompareTag("Brick") && FindObjectOfType<SlingShot>()._ballType == SlingShot.BallType.magic)
        {
            magicTrail.enabled = true;
            fire.gameObject.SetActive(false);
        }
        if (collision.CompareTag("key"))
        {
            //call enemy damage..
            if (collision.gameObject != null)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                collision.gameObject.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(this.keyaction(collision.gameObject));
                //instantiate  key collect particle..(auto destroy particles)
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(" trigger exits " + collision.tag);
        if (collision.CompareTag("Brick") && FindObjectOfType<SlingShot>()._ballType == SlingShot.BallType.magic)
        {
            magicTrail.time = 0;
            magicTrail.enabled = false;
            fire.gameObject.SetActive(true);
        }
    }

} 