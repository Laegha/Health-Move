using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PsMoveAPI.ControllerHelper;

public class ControllerData
{
    public Vector3 accel;
    public Vector3 position;
    public Vector3 movement;
    public Vector3 gyro;
    public Quaternion orientation;
    public PSMoveButton pressedButtons = 0;
    public PSMoveButton prevPressedButtons;
    public bool trackingSuccesful;
}
