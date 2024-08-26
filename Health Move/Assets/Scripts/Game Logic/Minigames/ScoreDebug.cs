using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreDebug : MonoBehaviour
{
    ControllerData controllerData;
    // Start is called before the first frame update
    void Start()
    {
        controllerData = ControllersManager.controllersManager.Controllers.First().Value;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerData.pressedButtons == (PsMoveAPI.ControllerHelper.PSMoveButton.Triangle | PsMoveAPI.ControllerHelper.PSMoveButton.Trigger) && controllerData.pressedButtons != controllerData.prevPressedButtons)
        {
            print("Scored");
            GameManager.gm.OnScored(null);
        }
    }
}
