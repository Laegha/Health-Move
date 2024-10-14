using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerCalibration : MonoBehaviour
{
    [SerializeField] GameObject _calibrationScreenPrefab;

    static ControllerCalibration instance;

    bool _calibrating = false;
    public static ControllerCalibration controllerCalibration { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator StartCalibration()
    {
        if(_calibrating)
            yield break;

        _calibrating = true;
        FindObjectsOfType<PsmoveButton>().ToList().ForEach(button => { button.isInteractable = false; });

        GameObject calibrationScreen = Instantiate(_calibrationScreenPrefab, FindObjectOfType<Canvas>().transform);
        calibrationScreen.SetActive(true);
        
        if (ControllersManager.controllersManager.Controller.Key != IntPtr.Zero)
        {
            ControllerHelper.psmove_tracker_disable(ControllersManager.controllersManager.Camera, ControllersManager.controllersManager.Controller.Key);
            ControllerHelper.psmove_disconnect(ControllersManager.controllersManager.Controller.Key);
            ControllersManager.controllersManager.Controller = new KeyValuePair<IntPtr, ControllerData>(IntPtr.Zero, null);
        }

        while (ControllerHelper.psmove_count_connected() <= 0)
        {
            Debug.Log("No connected controllers");
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        ControllersManager.controllersManager.Controller = new KeyValuePair<IntPtr, ControllerData>(ControllerHelper.psmove_connect_by_id(0), new ControllerData());

        StartCoroutine(ControllersManager.controllersManager.UpdateHandler());

        bool stillCalibrating = true;
        while (stillCalibrating)//this loop waits for input in order to calibrate controller
        {
            if (ControllersManager.controllersManager.Controller.Value.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && ControllersManager.controllersManager.Controller.Value.pressedButtons != ControllersManager.controllersManager.Controller.Value.prevPressedButtons)//if willing to connect, call CalibrateController and keep track of it
            {
                stillCalibrating = false;
                ControllersManager.controllersManager.CalibrateController(ControllersManager.controllersManager.Controller.Key);
                ControllerHelper.psmove_tracker_update_image(ControllersManager.controllersManager.Camera);
                ControllerHelper.psmove_tracker_update(ControllersManager.controllersManager.Camera, ControllersManager.controllersManager.Controller.Key);
            }

            yield return null;
        }


        StartCoroutine(ControllersManager.controllersManager.UpdateTracker());

        print("Ended Calibration");
        Destroy(calibrationScreen);

        GameManager.gm.ResetHands();

        yield return new WaitForEndOfFrame();

        FindObjectsOfType<PsmoveButton>().ToList().ForEach(button => { button.isInteractable = true; });
        _calibrating = false;
    }
}
