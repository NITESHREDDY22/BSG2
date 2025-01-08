using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


/// <summary>
/// Written by Kranthi Gunnam
/// </summary>

public class UIBase : CommonUIActions
{

	[SerializeField] public UiElementSet topElements, bottomElements, leftElements, rightElements, onlyFadeElements, AnimElements, scaleElements;
	void OnEnable ()
	{
		//TweenInAllElements ();
	}

	void OnDisable ()
	{
		
	}

	[SerializeField]Ease momentEaseIn = Ease.OutBack;
	[SerializeField]Ease momentEaseOut = Ease.OutBounce;


	public void TweenInAllElements ()
	{
        // one min , lets disalbe doTween if you want and see this fix ? without tween it will be ok we can directly set its position. we can try it , do it
		for (int i = 0; i < topElements.elements.Length; i++) {
			if(topElements.needFade)
			FadeIn (topElements.elements[i],topElements.Delay+(topElements.elementsDelay*i),topElements.time);
				
			topElements.elements[i].localPosition += Vector3.up * topElements.distance;
			topElements.elements [i].DOLocalMove (topElements.elements [i].localPosition - Vector3.up * topElements.distance,topElements.time).SetDelay (topElements.Delay+(topElements.elementsDelay*i)).SetEase (momentEaseIn);
			
		}
		for (int i = 0; i < bottomElements.elements.Length; i++) {
			if(bottomElements.needFade)
			FadeIn (bottomElements.elements [i],bottomElements.Delay+(bottomElements.elementsDelay*i),bottomElements.time);
			
			bottomElements.elements [i].localPosition -= Vector3.up * bottomElements.distance; 
			bottomElements.elements [i].DOLocalMove (bottomElements.elements [i].localPosition + Vector3.up *bottomElements.distance, bottomElements.time).SetDelay (bottomElements.Delay+(bottomElements.elementsDelay*i)).SetEase (momentEaseIn);
		}
		for (int i = 0; i < leftElements.elements.Length; i++) {
			if(leftElements.needFade)
			FadeIn (leftElements.elements [i],leftElements.Delay+(leftElements.elementsDelay*i),leftElements.time);
			
			leftElements.elements [i].localPosition -= Vector3.right *leftElements.distance; 
			leftElements.elements [i].DOLocalMove (leftElements.elements [i].localPosition + Vector3.right * leftElements.distance, leftElements.time).SetDelay (leftElements.Delay+(leftElements.elementsDelay*i)).SetEase (momentEaseIn);
		}
		for (int i = 0; i < rightElements.elements.Length; i++) {
			if(rightElements.needFade)
			FadeIn (rightElements.elements [i],rightElements.Delay+(rightElements.elementsDelay*i),rightElements.time);
			
			rightElements.elements [i].localPosition += Vector3.right * rightElements.distance; 
			rightElements.elements [i].DOLocalMove (rightElements.elements [i].localPosition - Vector3.right * rightElements.distance, rightElements.time).SetDelay (rightElements.Delay+(rightElements.elementsDelay*i)).SetEase (momentEaseIn);
		}

		for (int i = 0; i < onlyFadeElements.elements.Length; i++) {
			FadeIn (onlyFadeElements.elements [i], onlyFadeElements.Delay+(onlyFadeElements.elementsDelay*i),onlyFadeElements.time);
		}
		ScaleAllElements ();

	}


	float outDelay = 0.1f;

	public void TweenOutAllElements ()
	{
		float totalDelay = 0;
		for (int i = 0; i < topElements.elements.Length; i++) {
			if(topElements.needFade)
				FadeOut (topElements.elements[i],topElements.Delay+(topElements.elementsDelay*i),topElements.time);

			topElements.elements [i].DOLocalMove (topElements.elements [i].localPosition + Vector3.up * topElements.distance,topElements.time).SetEase (momentEaseOut);
			if(i == 0)
			totalDelay += topElements.time;
			

		}
		for (int i = 0; i < bottomElements.elements.Length; i++) {
			if(bottomElements.needFade)
				FadeOut (bottomElements.elements [i],bottomElements.Delay+(bottomElements.elementsDelay*i),bottomElements.time);

			bottomElements.elements [i].DOLocalMove (bottomElements.elements [i].localPosition - Vector3.up *bottomElements.distance, bottomElements.time).SetEase (momentEaseOut);


			if(i == 0)
				totalDelay += bottomElements.time;
			
		}
		for (int i = 0; i < leftElements.elements.Length; i++) {
			if(leftElements.needFade)
				FadeOut (leftElements.elements [i],leftElements.Delay+(leftElements.elementsDelay*i),leftElements.time);

			leftElements.elements [i].DOLocalMove (leftElements.elements [i].localPosition - Vector3.right * leftElements.distance, leftElements.time).SetEase (momentEaseOut);

			if(i == 0)
				totalDelay += leftElements.time;
			
		}
		for (int i = 0; i < rightElements.elements.Length; i++) {
			if(rightElements.needFade)
				FadeOut (rightElements.elements [i],rightElements.Delay+(rightElements.elementsDelay*i),rightElements.time);

			rightElements.elements [i].DOLocalMove (rightElements.elements [i].localPosition + Vector3.right * rightElements.distance, rightElements.time).SetEase (momentEaseOut);

			if(i == 0)
				totalDelay += rightElements.time;

		}

		for (int i = 0; i < onlyFadeElements.elements.Length; i++) {
			FadeOut (onlyFadeElements.elements [i], onlyFadeElements.Delay+(onlyFadeElements.elementsDelay*i),onlyFadeElements.time);

			if(i == 0)
				totalDelay += onlyFadeElements.time;

		}

		Invoke("OnCompleteTween",totalDelay+0.2f);
	}

	public void ScaleAllElements ()
	{
		for (int i = 0; i < scaleElements.elements.Length; i++) {
			scaleElements.elements [i].localScale = Vector3.zero;
			scaleElements.elements [i].DOScale (Vector3.one, scaleElements.time).SetDelay (scaleElements.Delay+(scaleElements.elementsDelay*i)).SetEase (momentEaseIn);;
		}
	}

	public virtual void OnCompleteTween ()
	{

		for (int i = 0; i < topElements.elements.Length; i++) {
			topElements.elements[i].localPosition -= Vector3.up * topElements.distance;
		}
		for (int i = 0; i < bottomElements.elements.Length; i++) {
			bottomElements.elements [i].localPosition += Vector3.up * bottomElements.distance; 
		}
		for (int i = 0; i < leftElements.elements.Length; i++) {
			leftElements.elements [i].localPosition += Vector3.right *leftElements.distance; 

		}
		for (int i = 0; i < rightElements.elements.Length; i++) {
			rightElements.elements [i].localPosition -= Vector3.right * rightElements.distance; 
		}

		gameObject.SetActive (false);
	}


	[SerializeField]bool needFade = true;

	void FadeIn (Transform gob, float delay,float _time)
	{
		if (!needFade)
			return;
			
		Image[] imgs = gob.GetComponentsInChildren<Image> ();
		Text[] txts = gob.GetComponentsInChildren<Text> ();
		SpriteRenderer[] sprites = gob.GetComponentsInChildren<SpriteRenderer> ();

		for (int i = 0; i < imgs.Length; i++) {
			Color clr = imgs [i].color;
			clr.a = 0;
			imgs [i].color = clr;
			imgs [i].DOFade (1, _time).SetDelay (delay).SetEase (momentEaseIn);
		}

		for (int i = 0; i < txts.Length; i++) {
			Color clr = txts [i].color;
			clr.a = 0;
			txts [i].color = clr;
			txts [i].DOFade (1, _time).SetDelay (delay).SetEase (momentEaseIn);
		}

		for (int i = 0; i < sprites.Length; i++) {
			Color clr = sprites [i].color;
			clr.a = 0;
			sprites [i].color = clr;
			sprites [i].DOFade (1, _time).SetDelay (delay).SetEase (momentEaseIn);
		}
	}

	public void FadeOut (Transform gob, float delay,float _time)
	{
		Image[] imgs = gob.GetComponentsInChildren<Image> ();
		Text[] txts = gob.GetComponentsInChildren<Text> ();
		SpriteRenderer[] sprites = gob.GetComponentsInChildren<SpriteRenderer> ();

		for (int i = 0; i < imgs.Length; i++) {
			imgs [i].DOFade (0, _time).SetDelay (outDelay - outDelay).SetEase (momentEaseOut);

		}

		for (int i = 0; i < txts.Length; i++) {
			txts [i].DOFade (0, _time).SetDelay (outDelay - outDelay).SetEase (momentEaseOut);
		}

		for (int i = 0; i < sprites.Length; i++) {
			sprites [i].DOFade (0, _time).SetDelay (outDelay - outDelay).SetEase (momentEaseOut);
		}

	}

	[SerializeField]bool IsScaleAnim;

	public void ScaleLoopObjects ()
	{
		if (IsScaleAnim) {
			for (int i = 0; i < AnimElements.elements.Length; i++) {
				AnimElements.elements [i].DOPunchScale (Vector3.one * 0.1f, 1, 1).SetLoops (-1, LoopType.Yoyo);
			}
		}
	}

	public void StopScaleLoopObjects ()
	{
		for (int i = 0; i < AnimElements.elements.Length; i++) {
			AnimElements.elements [i].DOKill ();
		}
	}


}

[System.Serializable]
public class UiElementSet{
	public Transform[] elements;
	public float distance = 500;
	public float time = 0.4f;
	public float Delay = 0,elementsDelay=0.01f;
	public bool needFade = true;
}