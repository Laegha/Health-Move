using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using UnityEngine.UI;
using TMPro;

public class PositionCalibrationScreen : MonoBehaviour
{
    [SerializeField] GameObject gfx;

    public IEnumerator WaitForInput()
    {
        gfx.SetActive(true);
        bool inputRecieved = false;
        while (!inputRecieved)
        {
            if (ControllersManager.controllersManager.Controller.Value.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && ControllersManager.controllersManager.Controller.Value.pressedButtons != ControllersManager.controllersManager.Controller.Value.prevPressedButtons)
                inputRecieved = true;
            
                yield return null;
        }

        //Start minigame
        Debug.Log("Generadas las hands");
        GameManager.gm.GenerateHands();
        gfx.SetActive(false);
    }
}
