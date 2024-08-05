using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandRotation : MonoBehaviour
{
    IntPtr _assignedController;

    public IntPtr AssignedController { get { return _assignedController; } set { _assignedController = value; } }

    public void RotationUpdate()
    {
        if (_assignedController == IntPtr.Zero)
            _assignedController = ControllersHandler.controllersHandler.Controllers.Keys.ToArray()[0];

        if (Input.GetKeyDown(KeyCode.R))
        {
            PsMoveAPI.ControllerHelper.psmove_reset_orientation(AssignedController);
        }

        Quaternion orientation = ControllersHandler.controllersHandler.Controllers[AssignedController].orientation;
        transform.rotation = orientation;
    }
}
