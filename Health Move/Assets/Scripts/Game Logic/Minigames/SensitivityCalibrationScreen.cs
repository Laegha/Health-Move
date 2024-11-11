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
                if(!firstInputRecieved)
                {
                    firstInputRecieved = true;
                    firstGfx.SetActive(false);
                    secondGfx.SetActive(true);
                    firstControllerPosition = ControllersManager.controllersManager.Controller.Value.position;
                }
                secondInputRecieved = true;
                secondControllerPosition = ControllersManager.controllersManager.Controller.Value.position;
            }

            yield return null;
        }

        float obtainedSensitivity = firstControllerPosition.magnitude - secondControllerPosition.magnitude;
        obtainedSensitivity += 1 + obtainedSensitivity - Profile.sensitivityStandard; //this is intended for the sensitivity to be one when it matches the standard
        editingProfile.sensitivity = obtainedSensitivity;

        firstGfx.SetActive(false);
        secondGfx.SetActive(false);

        callbackOnEnded?.Invoke();
    }
}
