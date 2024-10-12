using PsMoveAPI;
using System;
using System.Linq;
using UnityEngine;

public class HandInstantiator : MonoBehaviour
{
    [SerializeField] GameObject handPrefab;
    void Update()
    {
        if (ControllersManager.controllersManager.Controller.Key == IntPtr.Zero)
            return;

        ControllerData controller = ControllersManager.controllersManager.Controller.Value;

        ControllerHelper.PSMoveButton requiredButtons = ControllerHelper.PSMoveButton.Circle | ControllerHelper.PSMoveButton.Trigger;
        if (controller.pressedButtons == requiredButtons && controller.prevPressedButtons != controller.pressedButtons)
        {
            GameObject hand = Instantiate(handPrefab, transform.position, Quaternion.identity);
            //ControllersManager.controllersManager.Controller = ControllersManager.controllersManager.Controller.Key;
        }
    }
}
