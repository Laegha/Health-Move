using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class SensitivityCalibrationScreen : MonoBehaviour
{
    [SerializeField] GameObject firstGfx;
    [SerializeField] GameObject secondGfx;

    Profile editingProfile;
    Action callbackOnEnded;
    
    public void SetProfileSensitivity(Profile profile, Action callback)
    {
        editingProfile = profile;
        callbackOnEnded = callback;
        StartCoroutine(WaitForInput());
    }

    public IEnumerator WaitForInput()
    {
        firstGfx.SetActive(true);
        bool firstInputRecieved = false;
        bool secondInputRecieved = false;
        Vector3 firstControllerPosition = Vector3.zero;
        Vector3 secondControllerPosition = Vector3.zero;
        while (!secondInputRecieved)
        {
            if (ControllersManager.controllersManager.Controller.Value.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && ControllersManager.controllersManager.Controller.Value.pressedButtons != ControllersManager.controllersManager.Controller.Value.prevPressedButtons)
            {
                if (!firstInputRecieved)
                {
                    firstInputRecieved = true;
                    firstControllerPosition = ControllersManager.controllersManager.Controller.Value.position;
                    firstGfx.SetActive(false);
                    secondGfx.SetActive(true);
                }
                else
                {
                    secondInputRecieved = true;
                    secondControllerPosition = ControllersManager.controllersManager.Controller.Value.position;

                }
            }

            yield return null;
        }

        float positionDif = Math.Abs(firstControllerPosition.z - secondControllerPosition.z);
        float obtainedSensitivity = 0;
        if (positionDif < 10)
            obtainedSensitivity = 2;
        else if (positionDif < 20)
            obtainedSensitivity = 1.75f;
        else if (positionDif < 30)
            obtainedSensitivity = 1.5f;
        else if (positionDif < 40)
            obtainedSensitivity = 1.25f;
        else if (positionDif < 50)
            obtainedSensitivity = 1;
        else
            obtainedSensitivity = .75f;

        editingProfile.sensitivity = obtainedSensitivity;
        secondGfx.SetActive(false);

        callbackOnEnded?.Invoke();
    }
}
