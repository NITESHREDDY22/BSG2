using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongPressCheat : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float holdTime = 3f; // seconds to hold
    private float timer;
    private bool isHolding;
    public Graphic targetGraphic; // assign a UI element (Text, Image, etc.)
    public float blinkDuration = 0.2f; // how long each blink lasts
    private Color originalColor;

    void Update()
    {
        if (isHolding)
        {
            timer += Time.deltaTime;
            if (timer >= holdTime)
            {
                ActivateCheat();
                isHolding = false; // reset so it doesn’t trigger repeatedly
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        timer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        timer = 0f;
    }

    private void ActivateCheat()
    {
        Debug.Log("Cheat Activated!");
        WorldSelectionHandler.UnlockAllWorldsAndLevels();
        StartCoroutine(BlinkTwice());

        // Call your CheatManager or open cheat menu here
    }

    private IEnumerator BlinkTwice()
    {
        if (targetGraphic != null)
            originalColor = targetGraphic.color;

        for (int i = 0; i < 3; i++)
        {
            targetGraphic.color = Color.red;
            yield return new WaitForSeconds(blinkDuration);
            targetGraphic.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

}