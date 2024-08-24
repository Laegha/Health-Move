using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandRotation : MonoBehaviour
{
    PlayerIdentifier _playerIdentifier;

    public PlayerIdentifier PlayerIdentifier { get { return _playerIdentifier; } set { _playerIdentifier = value; } }

    public void RotationUpdate()
    {
        if (_playerIdentifier == null)
            return;
        
        if (!PlayerIdentifier.ControllerData.trackingSuccesful)
            return;

        ControllerHelper.PSMoveButton requiredButtons = ControllerHelper.PSMoveButton.Cross | ControllerHelper.PSMoveButton.Trigger;
        if (PlayerIdentifier.ControllerData.pressedButtons == requiredButtons && PlayerIdentifier.ControllerData.prevPressedButtons != PlayerIdentifier.ControllerData.pressedButtons)
        {
            ControllerHelper.psmove_reset_orientation(PlayerIdentifier.AssignedController);
        }

        Quaternion orientation = PlayerIdentifier.ControllerData.orientation;
        transform.rotation = orientation;
    }
}
