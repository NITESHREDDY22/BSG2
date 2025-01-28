using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    [SerializeField] int fps=45;

    void Start()
    {
        Application.targetFrameRate = fps;

        "PlayerPref.DeleteAll()"

    }

}
