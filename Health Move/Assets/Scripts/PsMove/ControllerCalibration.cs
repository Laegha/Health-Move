using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerCalibration : MonoBehaviour
{
    [SerializeField] ControllerHelper.PSMoveButton calibrationButton;
    [SerializeField] ControllerHelper.PSMoveButton endCalibrationButton;

    [SerializeField] GameObject calibrationScreen;

    static ControllerCalibration instance;
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
        FindObjectsOfType<PsmoveButton>().ToList().ForEach(button => { button.isInteractable = false; });
        calibrationScreen.SetActive(true);

        if (ControllersManager.controllersManager.Controllers.Count > 0)
            foreach(var controller in ControllersManager.controllersManager.Controllers) //disconnect all controllers from hardware
            {
                ControllerHelper.psmove_disconnect(controller.Key);
                ControllerHelper.psmove_tracker_disable(ControllersManager.controllersManager.Camera, controller.Key);
            }

        print("Calibrating");
        ControllersManager.controllersManager.Controllers.Clear(); //clear controllers dictionary

        int connectedControllers = ControllerHelper.psmove_count_connected();

        for (int i = 0; i < connectedControllers; i++) //connect all available controllers without camera
        {
            ControllersManager.controllersManager.Controllers.Add(ControllerHelper.psmove_connect_by_id(i), new ControllerData());
        }
        StartCoroutine(ControllersManager.controllersManager.UpdateHandler());

        bool stillCalibrating = true;
        List<IntPtr> calibratedControllers = new List<IntPtr>();

        while(stillCalibrating)//this loop calibrates with camera all controllers willing to connect
        {
            foreach(var controller in ControllersManager.controllersManager.Controllers)
            {
                if(controller.Value.pressedButtons == (calibrationButton | ControllerHelper.PSMoveButton.Trigger))//if willing to connect, call CalibrateController and keep track of it
                {
                    ControllersManager.controllersManager.CalibrateController(controller.Key);
                    calibratedControllers.Add(controller.Key);
                    ControllerHelper.psmove_tracker_update_image(ControllersManager.controllersManager.Camera);
                    ControllerHelper.psmove_tracker_update(ControllersManager.controllersManager.Camera, controller.Key);
                }

                if (controller.Value.pressedButtons == (endCalibrationButton | ControllerHelper.PSMoveButton.Trigger) && calibratedControllers.Contains(controller.Key))//if connection was ended, end loop
                {
                    stillCalibrating = false;
                    print("Ended calibration");
                }

            }
            yield return new WaitForEndOfFrame();
        }

        foreach(var controller in ControllersManager.controllersManager.Controllers)//remove from dictionary all controllers that weren't enabled with camera
        {
            if(!calibratedControllers.Contains(controller.Key))
            {
                ControllerHelper.psmove_disconnect(controller.Key);
                ControllersManager.controllersManager.Controllers.Remove(controller.Key);
                continue;
            }
            ControllerHelper.psmove_reset_orientation(controller.Key);
        }

        StartCoroutine(ControllersManager.controllersManager.UpdateTracker());

        print("Ended Calibration");
        calibrationScreen.SetActive(false);
        FindObjectsOfType<PsmoveButton>().ToList().ForEach(button => { button.isInteractable = true; });
        GameManager.gm.GenerateHands();
    }
}
