using System;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;

public class ControllersHandler : MonoBehaviour
{
    List<IntPtr> _controllers = new List<IntPtr>();

    void Start()
    {
        if(ControllerHelper.psmove_init(ControllerHelper.PSMove_Version.PSMOVE_CURRENT_VERSION) == ControllerHelper.PSMove_Bool.PSMove_False)
        {
            UnityEngine.Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
            return;
        }
        int connectedControllers = ControllerHelper.psmove_count_connected();
        for(int i = 0; i < connectedControllers; i++)
        {
            _controllers.Add(ControllerHelper.psmove_connect_by_id(i));
        }
    }


    void Update()
    {
        if (_controllers.Count < 0)
            return;

        foreach(var controller in _controllers)
        {
            if (ControllerHelper.psmove_poll(controller) == 0)
                continue;

            //Debug.Log(controller + " has pressed buttons: " + ControllerHelper.psmove_get_buttons(controller));

            int x = 0;
            int y = 0;
            int z = 0;

            
            ControllerHelper.psmove_get_accelerometer(controller, ref x, ref y, ref z);
            Vector3 accel = new Vector3(MapValue(x), MapValue(y), MapValue(z));



            //ControllerHelper.psmove_get_accelerometer_frame(controller, 1, ref x, ref y, ref z);

            Debug.Log(controller + " has accel: " + TruncateDecimals(4, accel.magnitude));
        }
    }

    float MapValue(float input)
    {
        return Mathf.Lerp(-1, 1, Mathf.InverseLerp(-35000, 35000, input));
    }

    float TruncateDecimals(int numberOfDecimals, float input)
    {
        int decimals = (int)(input * MathF.Pow(10, numberOfDecimals));
        float returned = decimals / MathF.Pow(10, numberOfDecimals);
        return returned;
    }
}
