using System;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using System.Linq;
using System.Collections;

public class ControllersHandler : MonoBehaviour
{
    //List<IntPtr> _controllers = new List<IntPtr>();
    Dictionary<IntPtr, Controller> _controllers = new Dictionary<IntPtr, Controller>();
    [SerializeField] Color _leds;
    Color _prevLeds;

    int _accelDecimals = 4;
    [SerializeField]float _sensitivityMin = 0.1f;

    public Dictionary<IntPtr, Controller> Controllers {  get { return _controllers; } }

    void Awake()
    {
        if(ControllerHelper.psmove_init(ControllerHelper.PSMove_Version.PSMOVE_CURRENT_VERSION) == ControllerHelper.PSMove_Bool.PSMove_False)
        {
            Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
            return;
        }

        int connectedControllers = ControllerHelper.psmove_count_connected();

        for(int i = 0; i < connectedControllers; i++)
        {
            _controllers.Add(ControllerHelper.psmove_connect_by_id(i), new Controller());
        }

        foreach (var controller in _controllers)
        {
            assignedController = controller.Key;
            StartCoroutine(Rainbow());
        }
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
                //ControllerHelper.psmove_set_leds(controller.Key, (byte)(_leds.r * 255), (byte)(_leds.g * 255), (byte)(_leds.b * 255));
                //SetLeds(controller.Key, (byte)(_leds.r * 255), (byte)(_leds.g * 255), (byte)(_leds.b * 255));
                _prevLeds = _leds;
            }

            ControllerHelper.psmove_update_leds(controller.Key);


            //ControllerHelper.psmove_get_accelerometer(controller, ref x, ref y, ref z);
            int x = 0;
            int y = 0;
            int z = 0;

            ControllerHelper.psmove_get_accelerometer(controller.Key, ref x, ref y, ref z);

            float accelX = TruncateDecimals(_accelDecimals, MapValue(x));
            float accelY = TruncateDecimals(_accelDecimals, MapValue(y));
            float accelZ = TruncateDecimals(_accelDecimals, MapValue(z));

            if(accelX > 0 && accelX < _sensitivityMin || accelX < 0 && accelX > -_sensitivityMin)
                accelX = 0;
            
            if(accelY > 0 && accelY < _sensitivityMin || accelY < 0 && accelY > -_sensitivityMin)
                accelY = 0;
            
            if(accelZ > 0 && accelZ < _sensitivityMin || accelZ < 0 && accelZ > -_sensitivityMin)
                accelZ = 0;

            controller.Value.accel = new Vector3(accelX, accelY, accelZ);


            //ControllerHelper.psmove_get_accelerometer_frame(controller, 1, ref x, ref y, ref z);


            float xAccel = TruncateDecimals(4, controller.Value.accel.x);

            //if (xAccel > 0.1)
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

    public void SetLeds(IntPtr move, int red, int green, int blue) => ControllerHelper.psmove_set_leds(move, (byte)(red), (byte)(green), (byte)(blue));




    IntPtr assignedController;
    IEnumerator Rainbow()
    {
        int red = 255;
        int green = 0;
        int blue = 0;
        bool isRedGrowing = false;
        bool isGreenGrowing = true;
        bool isBlueGrowing = true;

        int speed = 10;

        while (true)
        {
            yield return new WaitForEndOfFrame();
            red += isRedGrowing ? speed : -speed;
            green += isGreenGrowing ? speed : -speed;
            blue += isBlueGrowing ? speed : -speed;

            if (isRedGrowing && red > 255 || !isRedGrowing && red < 0)
                isRedGrowing = !isRedGrowing;

            if (isGreenGrowing && green > 255 || !isGreenGrowing && green < 0)
                isGreenGrowing = !isGreenGrowing;

            if (isBlueGrowing && blue > 255 || !isBlueGrowing && blue < 0)
                isBlueGrowing = !isBlueGrowing;

            SetLeds(assignedController, red, green, blue);
        }
    }
}
