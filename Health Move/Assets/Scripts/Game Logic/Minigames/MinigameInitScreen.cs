using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using UnityEngine.UI;
using TMPro;

public class MinigameInitScreen : MonoBehaviour
{
    Dictionary<ControllerData, bool> readyControllers = new Dictionary<ControllerData, bool>();

    int readyControllersCount = 0;

    TextMeshProUGUI readyPlayers;

    void Start()
    {
        foreach(ControllerData controllerData in ControllersManager.controllersManager.Controllers.Values)
        {
            readyControllers.Add(controllerData, false);
        }
        readyPlayers = transform.Find("ReadyPlayers").GetComponent<TextMeshProUGUI>();
        readyPlayers.text = "Jugadores listos: 0/" + readyControllers.Count;
    }

    void Update()
    {
        foreach(ControllerData controller in readyControllers.Keys)
        {
            if (readyControllers[controller])
                continue;

            if(controller.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && controller.pressedButtons != controller.prevPressedButtons)
            {
                readyControllers[controller] = true;
                readyControllersCount ++;
                readyPlayers.text = "Jugadores listos: " + readyControllersCount + "/" + readyControllersCount;
            }
        }
        if(readyControllersCount == readyControllers.Count) 
        {
            //Start minigame fr
            GameManager.gm.GenerateHands();
            Destroy(gameObject);
        }
    }
}
