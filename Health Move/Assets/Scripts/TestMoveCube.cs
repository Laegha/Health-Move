using System;
using System.Collections;
using UnityEngine;

public class TestMoveCube : MonoBehaviour
{
    [SerializeField] ControllersHandler controllersHandler;
    IntPtr assignedController;

    float speed = 10;

    void Start()
    {
        assignedController = controllersHandler.GetControllerByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        //cambiar la posicion segun controllersHandler.Controllers[assignedController].accel
        Vector3 accel = controllersHandler.Controllers[assignedController].accel * Time.deltaTime * speed;

        transform.position = new Vector3(transform.position.x + accel.x , transform.position.y + accel.y, transform.position.z + accel.z);
    }
}
