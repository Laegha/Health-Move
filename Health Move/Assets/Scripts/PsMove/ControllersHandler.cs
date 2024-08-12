using System;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using System.Linq;

public class ControllersHandler
{
    int _accelDecimals = 4;
    ControllersManager controllersManager;

    public ControllersHandler(ControllersManager controllersManager)
    {
        this.controllersManager = controllersManager;
    }
    public void Update()
    {
        foreach (var controller in controllersManager.Controllers)
        {
            if (ControllerHelper.psmove_poll(controller.Key) == 0)
                continue;

            controller.Value.prevPressedButtons = controller.Value.pressedButtons;
            controller.Value.pressedButtons = ControllerHelper.psmove_get_buttons(controller.Key);

            #region Setting Accel
            //ControllerHelper.psmove_get_accelerometer(controller, ref x, ref y, ref z);
            int accelXRaw = 0;
            int accelYRaw = 0;
            int accelZRaw = 0;

            ControllerHelper.psmove_get_accelerometer(controller.Key, ref accelXRaw, ref accelYRaw, ref accelZRaw);

            float accelX = TruncateDecimals(_accelDecimals, MapValue(accelXRaw));
            float accelY = TruncateDecimals(_accelDecimals, MapValue(accelYRaw));
            float accelZ = TruncateDecimals(_accelDecimals, MapValue(accelZRaw));

            //if(accelX > 0 && accelX < _sensitivityMin || accelX < 0 && accelX > -_sensitivityMin)
            //{
            //    accelX = 0;
            //    //print("x < 0: " + accelX);

            //}

            //if(accelY > 0 && accelY < _sensitivityMin || accelY < 0 && accelY > -_sensitivityMin)
            //{
            //    //print("y < 0: " + accelY );
            //    accelY = 0;

            //}    

            //if(accelZ > 0 && accelZ < _sensitivityMin || accelZ < 0 && accelZ > -_sensitivityMin)
            //{
            //    //print("z < 0: " + accelZ);
            //    accelZ = 0;
            //}
            ControllerHelper.psmove_has_calibration(controller.Key);
            controller.Value.accel = new Vector3(accelX, accelY, accelZ);
            #endregion

            #region Setting Gyroscope

            int gyroXRaw = 0;
            int gyroYRaw = 0;
            int gyroZRaw = 0;

            ControllerHelper.psmove_get_gyroscope(controller.Key, ref gyroXRaw, ref gyroYRaw, ref gyroZRaw);

            controller.Value.gyro = new Vector3(gyroXRaw, gyroYRaw, gyroZRaw);
            #endregion

            #region Setting Orientation

            float orientationW = 0;
            float orientationX = 0;
            float orientationY = 0;
            float orientationZ = 0;

            ControllerHelper.psmove_get_orientation(controller.Key, ref orientationW, ref orientationX, ref orientationY, ref orientationZ);

            controller.Value.orientation = new Quaternion(-orientationX, orientationZ, orientationY, orientationW);

            #endregion

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
        List<IntPtr> list = controllersManager.Controllers.Keys.ToList();
        
        return index < list.Count ? list[index] : (IntPtr)0;
    }

    public void SetLeds(IntPtr move, int red, int green, int blue) => ControllerHelper.psmove_set_leds(move, (byte)(red), (byte)(green), (byte)(blue));




    //IntPtr assignedController;
    //IEnumerator Rainbow()
    //{
    //    int red = 255;
    //    int green = 0;
    //    int blue = 0;
    //    bool isRedGrowing = false;
    //    bool isGreenGrowing = true;
    //    bool isBlueGrowing = true;

    //    int speed = 10;

    //    while (true)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        red += isRedGrowing ? speed : -speed;
    //        green += isGreenGrowing ? speed : -speed;
    //        blue += isBlueGrowing ? speed : -speed;

    //        if (isRedGrowing && red > 255 || !isRedGrowing && red < 0)
    //            isRedGrowing = !isRedGrowing;

    //        if (isGreenGrowing && green > 255 || !isGreenGrowing && green < 0)
    //            isGreenGrowing = !isGreenGrowing;

    //        if (isBlueGrowing && blue > 255 || !isBlueGrowing && blue < 0)
    //            isBlueGrowing = !isBlueGrowing;

    //        SetLeds(assignedController, red, green, blue);
    //    }
    //}

}
