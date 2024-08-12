using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersManager : MonoBehaviour
{
    Dictionary<IntPtr, ControllerData> _controllers = new Dictionary<IntPtr, ControllerData>();
    IntPtr _camera;
    
    [SerializeField] Color _leds;

    ControllersHandler _controllersHandler;
    ControllersTracker _controllersTracker;

    public Dictionary<IntPtr, ControllerData> Controllers { get { return _controllers; } }
    public IntPtr Camera { get { return _camera; } }
    
    static ControllersManager instance;
    public static ControllersManager controllersManager { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (ControllerHelper.psmove_init(ControllerHelper.PSMove_Version.PSMOVE_CURRENT_VERSION) == ControllerHelper.PSMove_Bool.PSMove_False)
        {
            Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
            return;
        }
        _camera = ControllerHelper.psmove_tracker_new();
        ControllerHelper.psmove_tracker_enable_deinterlace(_camera, true);

        _controllersHandler = new ControllersHandler(this);
        _controllersTracker = new ControllersTracker(this);
    }

    void Calibrate()
    {
        print("Calibrating...");
        int connectedControllers = ControllerHelper.psmove_count_connected();

        for (int i = 0; i < connectedControllers; i++)
        {
            _controllers.Add(ControllerHelper.psmove_connect_by_id(i), new ControllerData());
        }

        foreach (var controller in _controllers)
        {
            ControllerHelper.psmove_enable_orientation(controller.Key, true);
            while (ControllerHelper.psmove_tracker_enable(_camera, controller.Key) != 2) ;
        }

        if (_controllers.Count <= 0)
            return;

        StartCoroutine(UpdateHandler());
        StartCoroutine(UpdateTracker());
    }

    public void CalibrateController(IntPtr controller)
    {
        ControllerHelper.psmove_enable_orientation(controller, true);
        while (ControllerHelper.psmove_tracker_enable(_camera, controller) != 2) ;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Calibrate();
    }

    public IEnumerator UpdateHandler()
    {
        while (true)
        {
            if (controllersManager.Controllers.Count <= 0)
                continue;

            _controllersHandler.Update();
            GameManager.gm.UpdateHandsRotation();
            yield return null;
        }
    }

    public IEnumerator UpdateTracker()
    {
        while (true)
        {
            if (controllersManager.Controllers.Count <= 0)
                continue;

            _controllersTracker.Update();
            GameManager.gm.UpdateHandsPosition();
            yield return null;
        }
    }

    private void OnDestroy() => ControllerHelper.psmove_tracker_free(controllersManager.Camera);
}
