using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class CoinAnimation : MonoBehaviour
{

    public IEnumerator Animate(float delay, Transform destination)
    {
        yield return new WaitForSeconds(delay);
		try
		{
			Transform endPoint = destination;
			//transform.DOLocalJump(endPoint.localPosition, 5, 1, 1);
			transform.DOLocalMove(endPoint.localPosition, .75f).OnComplete(getKill);
			transform.GetComponent<UnityEngine.UI.Image>().DOFade(.5f, .75f);
			//if(SoundsHandler.Instance)SoundsHandler.Instance.PlaySource2Clip(0, 0); 
			int soundProp = PlayerPrefs.GetInt("SOUND");
			if (soundProp == 1)
			{
				gameObject.AddComponent<AudioSource>().clip = SoundsHandler.Instance.source2clips[0];
				gameObject.GetComponent<AudioSource>().Play();
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
                    FirebaseEvents.instance.LogFirebaseEvent("Exception", "LM_Read", exp.Message + "at " + errorline);
                }
			}
			catch (Exception e)
			{
				//
			}
		}
        //Debug.Log("Animate coin");
    }
    void getKill()
    {
        Destroy(gameObject);
    }
}
