using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        HandRotation handRotation = GetComponent<HandRotation>();
        HandMovement handMovement = GetComponent<HandMovement>();
        CursorMovement cursorMovement = FindObjectsOfType<CursorMovement>().Where(x => x.transform.root == transform).First();

        if(handRotation != null )
            handRotation.PlayerIdentifier = this;

        if(handMovement != null )
            handMovement.PlayerIdentifier = this;
        
        if(cursorMovement != null )
            cursorMovement.PlayerIdentifier = this;
    }
}
