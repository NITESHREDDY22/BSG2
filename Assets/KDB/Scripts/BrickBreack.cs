using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class BrickBreak : MonoBehaviour
{

    private AudioSource audioSource;

   
    private Explodable _explodable;
    //ArrayList al = new ArrayList();
    [SerializeField]
    public int count;

    private System.Boolean firstTime = true;


    void Start()
    {
        //Global.count  = 0;
        //gameOverPanel.SetActive (false);
   

    }

    void explodebottle()
    {
        _explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
        //_explodable.deleteFragments ();
        //Destroy (ef);
        Global.brokenBotList.Add(_explodable);
    }
    
    void OnCollisionEnter2D(Collision2D target)
    {


        //  if (!firstTime)
        // {
        //   audioSource.Play();

        // }


        //audioSource.Play(); // if i put this here sound comes
        firstTime = false;

        if (target.gameObject.tag == "Brick")
        {


            if (null != _explodable)
            {

                // if i comment this its working

                Invoke("explodebottle", 0.5f);

                //StartCoroutine(DestroyAfterDelay(2f,_explodable));
            }
        
            //Destroy (gameObject);
        }


    }


    void Update()
    {

        //_explodable.deleteFragments ();
    }
   

    IEnumerator DestroyAfterDelay(float delay, Explodable _explodable)
    {

        yield return new WaitForSeconds(delay);
        ///Debug.Log ("DestroyAfterDelay");
        _explodable.deleteFragments();
        //Destroy (gameObject);
        //_explodable.deleteFragments ();
    }

} // Pig