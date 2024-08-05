using System;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using System.Linq;
using System.Collections;

public class ControllersHandler : MonoBehaviour
{
    //List<IntPtr> _controllers = new List<IntPtr>();
    Dictionary<IntPtr, ControllerData> _controllers = new Dictionary<IntPtr, ControllerData>();
    [SerializeField] Color _leds;
    Color _prevLeds;

    int _accelDecimals = 4;
    [SerializeField]float _sensitivity = 0.1f;

    public Dictionary<IntPtr, ControllerData> Controllers {  get { return _controllers; } }

    IntPtr _camera;

    static ControllersHandler instance;
    public static ControllersHandler controllersHandler { get { return instance; } }

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);

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

        _camera = ControllerHelper.psmove_tracker_new();
       
        foreach (var controller in _controllers)
        {
            ControllerHelper.psmove_enable_orientation(controller.Key, true);
            print("Enabled: " + ControllerHelper.psmove_tracker_enable(_camera, controller.Key));
            //assignedController = controller.Key;
            //StartCoroutine(Rainbow());
            //ControllerHelper.psmove_set_leds(controller, 255, 255, 255);
        }

        ControllerHelper.psmove_tracker_enable_deinterlace(_camera, true);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            Calibrate();

        if (_controllers.Count <= 0)
            return;

        ControllerHelper.psmove_tracker_update_image(_camera);

        foreach (var controller in _controllers)
        {
            if (ControllerHelper.psmove_poll(controller.Key) == 0)
                continue;

            //if (_leds != _prevLeds)
            //{
            //    SetLeds(controller.Key, (byte)(_leds.r * 255), (byte)(_leds.g * 255), (byte)(_leds.b * 255));
            //    _prevLeds = _leds;
            //}

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

            if(ControllerHelper.psmove_tracker_update(_camera, controller.Key) == 0)
            {
                print("Update failed");
                return;
            }
            print("Update succesfull");

            #region Position Tracking
            float posX = 0;
            float posY = 0;
            float radius = 0;

            ControllerHelper.psmove_tracker_get_position(_camera, controller.Key, ref posX, ref posY, ref radius);
            float posZ = ControllerHelper.psmove_tracker_distance_from_radius(_camera, radius);
            posZ = TruncateDecimals(0, posZ);

            Vector3 newControllerPosition = new Vector3(posX, posY, posZ);

            controller.Value.movement = newControllerPosition - controller.Value.position;
            controller.Value.position = newControllerPosition;

            #endregion


            //ControllerHelper.psmove_get_accelerometer_frame(controller, 1, ref x, ref y, ref z);


            float xAccel = controller.Value.accel.x;

            //if (xAccel > 0.1)
                //Debug.Log(controller + " has accel: " + xAccel);
        }

        GameManager.gm.ControllersUpdated();
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
    private void OnDestroy()
    {
        print("Destroyed");
        ControllerHelper.psmove_tracker_free(_camera);
    }
}
