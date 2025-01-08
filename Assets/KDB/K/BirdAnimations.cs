using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimations : MonoBehaviour {

    public static BirdAnimations Instance;
    ParticleSystem[] anims;
	Transform [] objs;
	GameManager gm;
	SlingshotState bs;
	void Start () {
		Instance = this;


        anims = transform.GetComponentsInChildren<ParticleSystem>();

    }

    public void PlayAnimations()
    {
        foreach (ParticleSystem ps in anims)
        {
            var em = ps.emission;
            em.enabled = true;
            ps.Emit(1);
        }
    }
}
