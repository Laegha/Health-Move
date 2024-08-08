using System;
using System.Linq;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    IntPtr _assignedController;

    static readonly float pixelToUnit = 0.005f;
    static readonly float cmToPixel = 200f;

    [SerializeField] float handSpeed;

    public IntPtr AssignedController { get { return _assignedController; } set { _assignedController = value; } }

    public void MovementUpdate()
    {
        if(_assignedController == IntPtr.Zero) 
            _assignedController = ControllersManager.controllersManager.Controllers.Keys.ToArray()[0];

        Vector3 position = ControllersManager.controllersManager.Controllers[AssignedController].position;
        Vector3 pixelMovement = ControllersManager.controllersManager.Controllers[AssignedController].movement;
        pixelMovement = new Vector3(pixelMovement.x * position.z * pixelToUnit, pixelMovement.y * position.z * pixelToUnit, pixelMovement.z * cmToPixel * pixelToUnit);
        print(pixelMovement);
        Vector3 processedMovement = pixelMovement * Time.deltaTime * handSpeed /** sensitivity */;

        //print(position.z);

        transform.position -= processedMovement;
    }
}
