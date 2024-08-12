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
        int connectedControllers = ControllerHelper.psmove_count_connected();

        for (int i = 0; i < connectedControllers; i++)
        {
            ControllersManager.controllersManager.Controllers.Add(ControllerHelper.psmove_connect_by_id(i), new ControllerData());
        }
        StartCoroutine(ControllersManager.controllersManager.UpdateHandler());

        bool stillCalibrating = true;
        List<IntPtr> calibratedControllers = new List<IntPtr>();

        while(stillCalibrating)
        {
            foreach(var controller in ControllersManager.controllersManager.Controllers)
            {
                if(controller.Value.pressedButtons == calibrationButton)
                {
                    ControllersManager.controllersManager.CalibrateController(controller.Key);
                    calibratedControllers.Add(controller.Key);
                }
                
                if(controller.Value.pressedButtons == endCalibrationButton && calibratedControllers.Contains(controller.Key))
                    stillCalibrating = false; 

            }

        }

        foreach(var controller in ControllersManager.controllersManager.Controllers)
        {
            if(!calibratedControllers.Contains(controller.Key))
                ControllersManager.controllersManager.Controllers.Remove(controller.Key);
        }
    }
}
