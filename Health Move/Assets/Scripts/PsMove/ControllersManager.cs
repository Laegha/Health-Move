using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllersManager : MonoBehaviour
{
    KeyValuePair<IntPtr, ControllerData> _controller = new KeyValuePair<IntPtr, ControllerData>(IntPtr.Zero, null);
    IntPtr _camera;

    [SerializeField] Color _leds;

    ControllersHandler _controllersHandler;
    ControllersTracker _controllersTracker;

    bool _tracking = true;

    public KeyValuePair<IntPtr, ControllerData> Controller { get { return _controller; } set { _controller = value; } }
    public IntPtr Camera { get { return _camera; } }

    static ControllersManager instance;
    public static ControllersManager controllersManager { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (ControllerHelper.psmove_init(ControllerHelper.PSMove_Version.PSMOVE_CURRENT_VERSION) == ControllerHelper.PSMove_Bool.PSMove_False)
        {
            Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
            return;
        }

        _camera = ControllerHelper.psmove_tracker_new();
        ControllerHelper.psmove_tracker_enable_deinterlace(_camera, true);

        _controllersHandler = new ControllersHandler();
        _controllersTracker = new ControllersTracker();

    }

    private void Start()
    {
        StartCoroutine(ControllerCalibration.controllerCalibration.StartCalibration());
    }

    public void CalibrateController(IntPtr controller)
    {
        ControllerHelper.psmove_enable_orientation(controller, true);
        //while (ControllerHelper.psmove_tracker_enable(_camera, controller) != 2) ;
        while (ControllerHelper.psmove_tracker_enable_with_color(_camera, controller, (byte)(GameManager.gm.CurrPlayerColor.r * 255), (byte)(GameManager.gm.CurrPlayerColor.g * 255), (byte)(GameManager.gm.CurrPlayerColor.b * 255)) != 2) ;
    }

    public IEnumerator UpdateHandler()
    {
        while (true)
        {
            if(!_tracking)
                yield break;

            yield return null;

            if (Controller.Key == IntPtr.Zero)
                continue;

            _controllersHandler.Update();
            GameManager.gm.UpdateHandsRotation();
        }
    }

    public IEnumerator UpdateTracker()
    {
        while (true)
        {
            if (!_tracking)
                yield break;

            yield return null;
            if (Controller.Key == IntPtr.Zero)
                continue;

            _controllersTracker.Update();
            GameManager.gm.UpdateHandsPosition();
        }
    }

    public IEnumerator KillTracking()
    {
        _tracking = false;
        yield return new WaitForEndOfFrame();
        _tracking = true;

    }

    public void EmptyCamera() => ControllerHelper.psmove_tracker_free(Camera);

    private void OnDestroy() => EmptyCamera();
}
