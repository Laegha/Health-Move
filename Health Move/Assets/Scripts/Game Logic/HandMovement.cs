using System;
using System.Linq;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    IntPtr _assignedController;

    public IntPtr AssignedController { get { return _assignedController; } set { _assignedController = value; } }

    static readonly float pixelToUnit = 0.07f;

    void Start()
    {
    }

    public void ControllerUpdate()
    {
        if(_assignedController == IntPtr.Zero) 
            _assignedController = ControllersHandler.controllersHandler.Controllers.Keys.ToArray()[0];

        Vector3 position = ControllersHandler.controllersHandler.Controllers[AssignedController].position;
        Vector3 pixelMovement = ControllersHandler.controllersHandler.Controllers[AssignedController].movement;
        pixelMovement = new Vector3(pixelMovement.x * position.z, pixelMovement.y * position.z, pixelMovement.z);
        Vector3 processedMovement = ControllersHandler.controllersHandler.Controllers[AssignedController].movement * Time.deltaTime * pixelToUnit /** sensitivity */;

        print("Hola");
        print(processedMovement);

        transform.position -= processedMovement;
    }
}
