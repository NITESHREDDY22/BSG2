using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkipLvlClick : MonoBehaviour {
    [SerializeField]static GameObject skipLevelPopUp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void showWatchVideoPopUp()
    {
        GameObject watchVideoPopup = Instantiate(skipLevelPopUp, transform);
        watchVideoPopup.transform.localScale = Vector3.one;
     

    }
}
