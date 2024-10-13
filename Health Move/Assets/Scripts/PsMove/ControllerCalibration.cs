using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerCalibration : MonoBehaviour
{
    [SerializeField] GameObject calibrationScreen;

    static ControllerCalibration instance;
    public static ControllerCalibration controllerCalibration { get { return instance; } }

    bool _controllerCalibrated = false;

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
        FindObjectsOfType<PsmoveButton>().ToList().ForEach(button => { button.isInteractable = false; });
        calibrationScreen.SetActive(true);

        if (ControllersManager.controllersManager.Controller.Key != IntPtr.Zero)
        {
            ControllerHelper.psmove_disconnect(ControllersManager.controllersManager.Controller.Key);
            ControllerHelper.psmove_tracker_disable(ControllersManager.controllersManager.Camera, ControllersManager.controllersManager.Controller.Key);
            ControllersManager.controllersManager.Controller = new KeyValuePair<IntPtr, ControllerData>(IntPtr.Zero, null);
        }

        while(ControllerHelper.psmove_count_connected() <= 0)
            yield return new WaitForEndOfFrame();

        ControllersManager.controllersManager.Controller = new KeyValuePair<IntPtr, ControllerData>(ControllerHelper.psmove_connect_by_id(0), new ControllerData());

        StartCoroutine(ControllersManager.controllersManager.UpdateHandler());

        bool stillCalibrating = true;

        while(stillCalibrating)//this loop waits for input in order to calibrate controller
        {
            if (ControllersManager.controllersManager.Controller.Value.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && ControllersManager.controllersManager.Controller.Value.pressedButtons != ControllersManager.controllersManager.Controller.Value.prevPressedButtons)//if willing to connect, call CalibrateController and keep track of it
            {
                stillCalibrating = false;
                ControllersManager.controllersManager.CalibrateController(ControllersManager.controllersManager.Controller.Key);
                _controllerCalibrated = true;
                ControllerHelper.psmove_tracker_update_image(ControllersManager.controllersManager.Camera);
                ControllerHelper.psmove_tracker_update(ControllersManager.controllersManager.Camera, ControllersManager.controllersManager.Controller.Key);
            }

            yield return new WaitForEndOfFrame();
        }
        ControllerHelper.psmove_reset_orientation(ControllersManager.controllersManager.Controller.Key);


        StartCoroutine(ControllersManager.controllersManager.UpdateTracker());

        print("Ended Calibration");
        calibrationScreen.SetActive(false);
        FindObjectOfType<PositionCalibrationScreen>().gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();

        FindObjectsOfType<PsmoveButton>().ToList().ForEach(button => { button.isInteractable = true; });
    }
}
