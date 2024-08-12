using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCalibration : MonoBehaviour
{
    [SerializeField] ControllerHelper.PSMoveButton calibrationButton;
    [SerializeField] ControllerHelper.PSMoveButton endCalibrationButton;

    //private void Awake()
    //{
    //    if (ControllerHelper.psmove_init(ControllerHelper.PSMove_Version.PSMOVE_CURRENT_VERSION) == ControllerHelper.PSMove_Bool.PSMove_False)
    //    {
    //        Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
    //        return;
    //    }
    //}

    void StartCalibration()
    {
        foreach(var controller in ControllersManager.controllersManager.Controllers) //disconnect all controllers from hardware
        {
            ControllerHelper.psmove_disconnect(controller.Key);
            ControllerHelper.psmove_tracker_disable(ControllersManager.controllersManager.Camera, controller.Key);
        }

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
                if(controller.Value.pressedButtons == calibrationButton)//if willing to connect, call CalibrateController and keep track of it
                {
                    ControllersManager.controllersManager.CalibrateController(controller.Key);
                    calibratedControllers.Add(controller.Key);
                }
                
                if(controller.Value.pressedButtons == endCalibrationButton && calibratedControllers.Contains(controller.Key))//if connection was ended, end loop
                    stillCalibrating = false; 

            }

        }

        foreach(var controller in ControllersManager.controllersManager.Controllers)//remove from dictionary all controllers that weren't enabled with camera
        {
            if(!calibratedControllers.Contains(controller.Key))
            {
                ControllerHelper.psmove_disconnect(controller.Key);
                ControllersManager.controllersManager.Controllers.Remove(controller.Key);
            }
        }

        StartCoroutine(ControllersManager.controllersManager.UpdateTracker());
    }
}
