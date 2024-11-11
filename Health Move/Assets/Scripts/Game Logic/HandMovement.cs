using System;
using System.Linq;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    static readonly float pixelToUnit = 0.005f;
    static readonly float cmToPixel = 200f;

    [SerializeField] float handSpeed;

    PlayerIdentifier _playerIdentifier;

    public PlayerIdentifier PlayerIdentifier { get { return _playerIdentifier; } set { _playerIdentifier = value; } }

    public void MovementUpdate()
    {
        if (_playerIdentifier == null)
            return;

        if (!PlayerIdentifier.ControllerData.trackingSuccesful)
            return;

        float positionZ = PlayerIdentifier.ControllerData.position.z;
        Vector3 pixelMovement = PlayerIdentifier.ControllerData.movement;
        pixelMovement = new Vector3(pixelMovement.x * positionZ * pixelToUnit, pixelMovement.y * positionZ * pixelToUnit, pixelMovement.z * cmToPixel * pixelToUnit);


        var rectTransform = GetComponent<RectTransform>();

        if(rectTransform != null )
        {
            Vector3 processedMovement = pixelMovement * Time.deltaTime * handSpeed;
            rectTransform.position -= new Vector3(processedMovement.x, processedMovement.y, 0);
        }
        else
        {
            Vector3 processedMovement = pixelMovement * Time.deltaTime * handSpeed * PlayerIdentifier.Profile.sensitivity;
            transform.position -= processedMovement;
        }
    }
}
