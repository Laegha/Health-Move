using PsMoveAPI;
using System;
using System.Linq;
using UnityEngine;

public class HandInstantiator : MonoBehaviour
{
    [SerializeField] GameObject handPrefab;
    void Update()
    {
        if (ControllersManager.controllersManager.Controllers.Count == 0)
            return;

        ControllerData controller = ControllersManager.controllersManager.Controllers.Values.ToArray()[0];

        ControllerHelper.PSMoveButton requiredButtons = ControllerHelper.PSMoveButton.Circle | ControllerHelper.PSMoveButton.Trigger;
        if (controller.pressedButtons == requiredButtons && controller.prevPressedButtons != controller.pressedButtons)
        {
            GameObject hand = Instantiate(handPrefab, transform.position, Quaternion.identity);
            hand.GetComponent<PlayerIdentifier>().AssignedController = ControllersManager.controllersManager.Controllers.Keys.ToArray()[0];
        }
    }
}
