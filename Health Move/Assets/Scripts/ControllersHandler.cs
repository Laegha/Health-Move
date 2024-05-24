using System;
using System.Collections;
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
            Debug.Log("Failed to initialize PSMoveAPI. Probably using a wrong version");
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
        _controllers.ForEach(controller => Debug.Log(controller + " has pressed buttons: " + ControllerHelper.psmove_get_buttons(controller)));
    }
}
