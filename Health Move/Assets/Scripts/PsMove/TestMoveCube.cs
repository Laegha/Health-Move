using System;
using System.Collections;
using UnityEngine;

public class TestMoveCube : MonoBehaviour
{
    [SerializeField] Vector3 movement;

    [SerializeField] ControllersHandler controllersHandler;
    IntPtr assignedController;

    [SerializeField] Vector3 position;
    [SerializeField] Vector3 positionZero = Vector3.zero;

    float speed = 10;

    void Start()
    {
        assignedController = controllersHandler.GetControllerByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (assignedController.ToInt32() == 0)
        {
            assignedController = controllersHandler.GetControllerByIndex(0);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PsMoveAPI.ControllerHelper.psmove_reset_orientation(assignedController);
            positionZero = controllersHandler.Controllers[assignedController].position;
        }

        //cambiar la posicion segun controllersHandler.Controllers[assignedController].accel
        //Vector3 accel = controllersHandler.Controllers[assignedController].accel * Time.deltaTime * speed;
        //movement = accel;

        //position = controllersHandler.Controllers[assignedController].position;
        //transform.position = new Vector3(position.x - positionZero.x, position.y - positionZero.y, position.z - positionZero.z);

        Quaternion orientation = controllersHandler.Controllers[assignedController].orientation;
        transform.rotation = orientation;
    }
}
