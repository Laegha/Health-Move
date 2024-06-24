using System;
using System.Collections;
using UnityEngine;

public class TestMoveCube : MonoBehaviour
{
    [SerializeField] ControllersHandler controllersHandler;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 position;
    IntPtr assignedController;

    float speed = 10;

    void Start()
    {
        assignedController = controllersHandler.GetControllerByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (assignedController.ToInt32() == 0)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            PsMoveAPI.ControllerHelper.psmove_reset_orientation(assignedController);
            transform.position = Vector3.zero;
        }
        //cambiar la posicion segun controllersHandler.Controllers[assignedController].accel
        Vector3 accel = controllersHandler.Controllers[assignedController].accel * Time.deltaTime * speed;
        movement = accel;

        //transform.position = new Vector3(transform.position.x + accel.x, transform.position.y + accel.y, transform.position.z + accel.z);
        position = controllersHandler.Controllers[assignedController].position;
        transform.position = position;

        //Quaternion orientation = controllersHandler.Controllers[assignedController].orientation;
        //transform.rotation = orientation;
    }
}
