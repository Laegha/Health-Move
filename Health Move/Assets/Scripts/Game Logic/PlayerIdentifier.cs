using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public int playerID;
    ControllerData _controllerData;

    [SerializeField] Renderer[] _braceletRenderers;

    public ControllerData ControllerData {  get { return _controllerData; } set { _controllerData = value; } }
    public Renderer[] BraceletRenderers {  get { return _braceletRenderers; } set { _braceletRenderers = value; } }

    private void Start()
    {
        ControllerData = ControllersManager.controllersManager.Controller.Value;

        HandRotation handRotation = GetComponent<HandRotation>();
        HandMovement handMovement = GetComponent<HandMovement>();
        CursorMovement cursorMovement = GetCursorMovement();

        if(handRotation != null )
            handRotation.PlayerIdentifier = this;

        if(handMovement != null )
            handMovement.PlayerIdentifier = this;
        
        if(cursorMovement != null )
            cursorMovement.PlayerIdentifier = this;
    }

    CursorMovement GetCursorMovement()
    {
        List<CursorMovement> cursorMovements = FindObjectsOfType<CursorMovement>().Where(x => x.transform.parent == transform).ToList();
        if(cursorMovements.Count > 0)
            return cursorMovements[0];
        else 
            return null;
    }
}
