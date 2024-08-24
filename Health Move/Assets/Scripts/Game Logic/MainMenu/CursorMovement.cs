using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    static float rotationToUnits = 0.05f;

    PlayerIdentifier _playerIdentifier;

    RectTransform _rectTransform;

    public PlayerIdentifier PlayerIdentifier { get { return _playerIdentifier; } set { _playerIdentifier = value; } }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void CursorUpdate()
    {
        Vector3 eulers = PlayerIdentifier.ControllerData.orientation.eulerAngles;
        Vector3 positionByRotation = new Vector3(eulers.x, eulers.y, 0) * rotationToUnits;

        _rectTransform.localPosition = positionByRotation;
    }
}
