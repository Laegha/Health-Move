using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using UnityEngine.UI;
using TMPro;

public class MinigameInitScreen : MonoBehaviour
{
    void Update()
    {
        if (ControllersManager.controllersManager.Controller.Value.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && ControllersManager.controllersManager.Controller.Value.pressedButtons != ControllersManager.controllersManager.Controller.Value.prevPressedButtons)
        {
            //Start minigame
            GameManager.gm.GenerateHands();
            Destroy(gameObject);
        }

    }
}
