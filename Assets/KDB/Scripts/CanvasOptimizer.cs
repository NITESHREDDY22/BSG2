using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOptimizer : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasScaler scaler;
    [SerializeField] GraphicRaycaster raycaster;

    private void OnEnable()
    {
        HandleCanvasComponents(true);
    }

    private void OnDisable()
    {
        HandleCanvasComponents(false);
    }

    void HandleCanvasComponents(bool flag)
    {
        if (canvas != null)
        {
            canvas.enabled = flag;
        }
        if (scaler != null)
        {
            scaler.enabled = flag;
        }
        if (raycaster != null)
        {
            raycaster.enabled = flag;
        }
    }
}
