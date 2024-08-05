using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRotationGetter : MonoBehaviour
{
    //List<IntPtr> _controllers = new List<IntPtr>();
    Dictionary<IntPtr, ControllerData> _controllers = new Dictionary<IntPtr, ControllerData>();
    [SerializeField] Color _leds;
    Color _prevLeds;

    int _accelDecimals = 4;
    [SerializeField] float _sensitivity = 0.1f;

    public Dictionary<IntPtr, ControllerData> Controllers { get { return _controllers; } }


    static ControllersHandler instance;
    public static ControllersHandler controllersHandler { get { return instance; } }

    private void Awake()
    {
        if (ControllerHelper.psmove_init(ControllerHelper.PSMove_Version.PSMOVE_CURRENT_VERSION) == ControllerHelper.PSMove_Bool.PSMove_False)
        {
            Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
            return;
        }
    }

    void Calibrate()
    {

        int connectedControllers = ControllerHelper.psmove_count_connected();

        for (int i = 0; i < connectedControllers; i++)
        {
            _controllers.Add(ControllerHelper.psmove_connect_by_id(i), new ControllerData());
        }


        foreach (var controller in _controllers)
        {
            ControllerHelper.psmove_enable_orientation(controller.Key, true);
            //assignedController = controller.Key;
            //StartCoroutine(Rainbow());
            //ControllerHelper.psmove_set_leds(controller, 255, 255, 255);
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Calibrate();

        if (_controllers.Count <= 0)
            return;


        foreach (var controller in _controllers)
        {

            if (ControllerHelper.psmove_poll(controller.Key) == 0)
                continue;

            print("Update succesfull");

            //Debug.Log(controller + " has pressed buttons: " + ControllerHelper.psmove_get_buttons(controller));

            //byte red = 0;
            //byte green = 0;
            //byte blue = 0;

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

        GameManager.gm.ControllersUpdated();
    }

}
