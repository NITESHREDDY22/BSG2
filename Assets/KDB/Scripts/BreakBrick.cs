using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour {

    private Explodable _explodable;

    // Use this for initialization
    void Start () {

        _explodable = GetComponent<Explodable>();

    }

    void explodebottle()
    {
        _explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
        //_explodable.deleteFragments ();
        //Destroy (ef);
       // Global.brokenBotList.Add(_explodable);
    }

    void OnCollisionEnter2D(Collision2D target)
    {


       

        if (target.gameObject.tag == "Bird")
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

    // Update is called once per frame
    void Update () {
		
	}
}
