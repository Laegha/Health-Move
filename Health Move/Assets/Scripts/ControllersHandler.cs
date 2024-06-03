using System;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using System.Linq;

public class ControllersHandler : MonoBehaviour
{
    //List<IntPtr> _controllers = new List<IntPtr>();
    Dictionary<IntPtr, Controller> _controllers = new Dictionary<IntPtr, Controller>();
    [SerializeField] Color _leds;
    Color _prevLeds;

    public Dictionary<IntPtr, Controller> Controllers {  get { return _controllers; } }

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
            _controllers.Add(ControllerHelper.psmove_connect_by_id(i), new Controller());
        }

        //foreach(var controller in _controllers)
            //ControllerHelper.psmove_set_leds(controller, 255, 255, 255);
    }


    void Update()
    {
        if (_controllers.Count < 0)
            return;

        foreach(var controller in _controllers)
        {
            if (ControllerHelper.psmove_poll(controller.Key) == 0)
                continue;

            //Debug.Log(controller + " has pressed buttons: " + ControllerHelper.psmove_get_buttons(controller));

            if (_leds != _prevLeds)
            {
                ControllerHelper.psmove_set_leds(controller.Key, (byte)(_leds.r * 255), (byte)(_leds.g * 255), (byte)(_leds.b * 255));
                _prevLeds = _leds;
            }

            ControllerHelper.psmove_update_leds(controller.Key);

            
            //ControllerHelper.psmove_get_accelerometer(controller, ref x, ref y, ref z);
            ControllerHelper.psmove_get_accelerometer(controller.Key, ref controller.Value.accelX, ref controller.Value.accelY, ref controller.Value.accelZ);
            controller.Value.accel = new Vector3(MapValue(controller.Value.accelX), MapValue(controller.Value.accelY), MapValue(controller.Value.accelZ));



            //ControllerHelper.psmove_get_accelerometer_frame(controller, 1, ref x, ref y, ref z);


            float xAccel = TruncateDecimals(4, controller.Value.accel.x);

            if (xAccel > 0.1)
                Debug.Log(controller + " has accel: " + xAccel);
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

    public IntPtr GetControllerByIndex(int index)
    {
        List<IntPtr> list = _controllers.Keys.ToList();
        return list[index];
    }
}
