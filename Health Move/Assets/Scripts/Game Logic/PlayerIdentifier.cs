using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public int playerID;
    IntPtr _assignedController;
    ControllerData _controllerData;

    public IntPtr AssignedController {  get { return _assignedController; } set { _assignedController = value; } }
    public ControllerData ControllerData {  get { return _controllerData; } set { _controllerData = value; } }

    private void Start()
    {
        while(AssignedController == null) { }

        ControllerData = ControllersManager.controllersManager.Controllers[AssignedController];
        GetComponent<HandRotation>().PlayerIdentifier = this;
        GetComponent<HandMovement>().PlayerIdentifier = this;
    }
}
