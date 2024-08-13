using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersTracker
{
    public void Update()
    {
        ControllerHelper.psmove_tracker_update_image(ControllersManager.controllersManager.Camera);

        foreach (var controller in ControllersManager.controllersManager.Controllers)
        {
            if (ControllerHelper.psmove_tracker_update(ControllersManager.controllersManager.Camera, controller.Key) == 0)
            {
                Debug.Log("Tracking Update failed");
                return;
            }
            Debug.Log("Tracking Update succesfull");

            #region Position Tracking
            float posX = 0;
            float posY = 0;
            float radius = 0;

            ControllerHelper.psmove_tracker_get_position(ControllersManager.controllersManager.Camera, controller.Key, ref posX, ref posY, ref radius);
            float posZ = ControllerHelper.psmove_tracker_distance_from_radius(ControllersManager.controllersManager.Camera, radius);
            posZ = TruncateDecimals(0, posZ);

            Vector3 newControllerPosition = new Vector3(posX, posY, posZ);

            controller.Value.movement = newControllerPosition - controller.Value.position;
            controller.Value.position = newControllerPosition;

            #endregion
        }
    }

    float TruncateDecimals(int numberOfDecimals, float input)
    {
        int decimals = (int)(input * MathF.Pow(10, numberOfDecimals));
        float returned = decimals / MathF.Pow(10, numberOfDecimals);

        return returned;
    }
}
