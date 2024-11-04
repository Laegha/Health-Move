using PsMoveAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateOnStart : MonoBehaviour
{
    static bool started;

    void Start()
    {
        if(started)
        {
            StartCoroutine(ControllerCalibration.controllerCalibration.StartCalibration(() =>
            {
                GameManager.gm.ResetHands();
            }));
        }
        started = true;
    }
}
