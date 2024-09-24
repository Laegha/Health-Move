using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersTracker
{
    bool _firstExecution = true;
    float minMovement = .5f;

    public void Update()
    {
        ControllerHelper.psmove_tracker_update_image(ControllersManager.controllersManager.Camera);

        foreach (var controller in ControllersManager.controllersManager.Controllers)
        {
            if (ControllerHelper.psmove_tracker_update(ControllersManager.controllersManager.Camera, controller.Key) == 0)
            {
                Debug.Log("Tracking Update failed");
                controller.Value.trackingSuccesful = false;
                return;
            }
            Debug.Log("Tracking Update succesfull");
            controller.Value.trackingSuccesful = true;

            #region Position Tracking
            float posX = 0;
            float posY = 0;
            float radius = 0;

            ControllerHelper.psmove_tracker_get_position(ControllersManager.controllersManager.Camera, controller.Key, ref posX, ref posY, ref radius);
            float posZ = ControllerHelper.psmove_tracker_distance_from_radius(ControllersManager.controllersManager.Camera, radius);
            posZ = TruncateDecimals(0, posZ);

            Vector3 newControllerPosition = new Vector3(posX, posY, posZ);

            if(_firstExecution)
            {
                _firstExecution = false;
                controller.Value.position = newControllerPosition;

            }
            Vector3 movement = newControllerPosition - controller.Value.position;
            
            if (Math.Abs(movement.x) < minMovement)
                movement.x = 0;
            if (Math.Abs(movement.y) < minMovement)
                movement.y = 0;
            if (Math.Abs(movement.z) < minMovement)
                movement.z = 0;

            controller.Value.movement = movement;
            Debug.Log(controller.Value.movement);
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
