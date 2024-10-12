using System;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using System.Linq;

public class ControllersHandler
{
    int _accelDecimals = 4;

    public void Update()
    {
        if (ControllerHelper.psmove_poll(ControllersManager.controllersManager.Controller.Key) == 0)
            return;

        ControllersManager.controllersManager.Controller.Value.prevPressedButtons = ControllersManager.controllersManager.Controller.Value.pressedButtons;
        ControllersManager.controllersManager.Controller.Value.pressedButtons = ControllerHelper.psmove_get_buttons(ControllersManager.controllersManager.Controller.Key);

        #region Setting Accel
        //ControllerHelper.psmove_get_accelerometer(controller, ref x, ref y, ref z);
        int accelXRaw = 0;
        int accelYRaw = 0;
        int accelZRaw = 0;

        ControllerHelper.psmove_get_accelerometer(ControllersManager.controllersManager.Controller.Key, ref accelXRaw, ref accelYRaw, ref accelZRaw);

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
        ControllerHelper.psmove_has_calibration(ControllersManager.controllersManager.Controller.Key);
        ControllersManager.controllersManager.Controller.Value.accel = new Vector3(accelX, accelY, accelZ);
        #endregion

        #region Setting Gyroscope

        int gyroXRaw = 0;
        int gyroYRaw = 0;
        int gyroZRaw = 0;

        ControllerHelper.psmove_get_gyroscope(ControllersManager.controllersManager.Controller.Key, ref gyroXRaw, ref gyroYRaw, ref gyroZRaw);

        ControllersManager.controllersManager.Controller.Value.gyro = new Vector3(gyroXRaw, gyroYRaw, gyroZRaw);
        #endregion

        #region Setting Orientation

        float orientationW = 0;
        float orientationX = 0;
        float orientationY = 0;
        float orientationZ = 0;

        ControllerHelper.psmove_get_orientation(ControllersManager.controllersManager.Controller.Key, ref orientationW, ref orientationX, ref orientationY, ref orientationZ);

        ControllersManager.controllersManager.Controller.Value.orientation = new Quaternion(-orientationX, orientationZ, orientationY, orientationW);

        #endregion


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
