using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class portal : MonoBehaviour
{
    public GameObject PortalExit;
    public portalexitdir PEdir = portalexitdir.left;
    public float ballaccleration = 0;
    private int sign = 1;
    public AudioClip ballin, ballout;
    public void getObjectin(GameObject ballobj)
    {
        StartCoroutine(DoRepose(ballobj));
    }

    IEnumerator DoRepose(GameObject ballobj)
    {
        //SoundManager.PlaySFX(ballin, false);
        ballobj.transform.DOMove(transform.position, 0.4f);
        //ballobj.transform.DOScale(0f, 1f);
        ballobj.GetComponent<SpriteRenderer>().enabled = false;
        ballobj.GetComponent<TrailRenderer>().enabled = false;
        ballobj.GetComponent<CircleCollider2D>().enabled = false;

        ballobj.GetComponent<TrailRenderer>().Clear();

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(dores(ballobj));
    }


    IEnumerator dores(GameObject ballobj) {
        if (ballobj != null)
        {
            ballobj.transform.position = PortalExit.transform.position;
            ballobj.transform.DOScale(1f, 0.0f);
            //SoundManager.PlaySFX(ballout, false);
            Vector3 dir = Vector3.zero;
            if (this.PEdir == portalexitdir.left)
            {
                dir = -PortalExit.transform.right;
            }
            else
            {
                dir = PortalExit.transform.right;
            }
            ballobj.GetComponent<SpriteRenderer>().enabled = true;
            float bAcc = Mathf.Clamp(ballaccleration, 350f, 500f);
            ballobj.GetComponent<CircleCollider2D>().enabled = true;
            //ballobj.GetComponent<Rigidbody2D>().AddForce(((sign*PortalExit.transform.right)+ new Vector3(0,((ballaccleration)/1000),0)) * ballaccleration*1.5f,ForceMode2D.Force);
            //ballobj.GetComponent<Rigidbody2D>().AddForce(((sign * PortalExit.transform.right)) * ballaccleration * 1.25f, ForceMode2D.Force);
            ballobj.GetComponent<Rigidbody2D>().AddRelativeForce(((dir)*bAcc),ForceMode2D.Force);

            yield return new WaitForSeconds(0.2f);
            ballobj.GetComponent<TrailRenderer>().enabled = true;
        }
    }

}

public enum portalexitdir
{
    left,
    right
}