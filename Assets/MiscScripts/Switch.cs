using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public SwitchMove switchMove;
    public Animator anim;


	// Use this for initialization
	void Start () {
        anim.enabled = false;
	}
	
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Bird" || target.gameObject.tag == "stone" || target.gameObject.tag == "pig")
        {
            anim.enabled = true;
            switchMove.isAccessed = true;
            if (GetComponent<BoxCollider2D>() != null)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }


}
