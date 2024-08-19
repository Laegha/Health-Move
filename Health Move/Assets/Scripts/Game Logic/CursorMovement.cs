using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    static float rotationToUnits = 0.05f;

    PlayerIdentifier _playerIdentifier;

    public PlayerIdentifier PlayerIdentifier { get { return _playerIdentifier; } set { _playerIdentifier = value; } }

    private void Update()
    {
        Vector3 eulers = PlayerIdentifier.ControllerData.orientation.eulerAngles;
        Vector3 positionByRotation = new Vector3(eulers.x, eulers.y, eulers.z + 90) * rotationToUnits;

        transform.localPosition = positionByRotation;
    }
}
