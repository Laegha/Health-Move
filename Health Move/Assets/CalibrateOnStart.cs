using PsMoveAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateOnStart : MonoBehaviour
{
    static bool started;
    [SerializeField] GameObject screenPrefab;
    GameObject instantiatedScreen;

    void Start()
    {
        if(started)
        {
            instantiatedScreen = Instantiate(screenPrefab, GameObject.Find("Canvas").transform);
            instantiatedScreen.SetActive(true);
            StartCoroutine(ControllerCalibration.controllerCalibration.StartCalibration(() =>
            {
                GameManager.gm.ResetHands();
            }));
            StartCoroutine(WaitForInput());
        }
        started = true;
    }

    IEnumerator WaitForInput()
    {
        while(true)
        {
            if (ControllersManager.controllersManager.Controller.Value.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger) && ControllersManager.controllersManager.Controller.Value.pressedButtons != ControllersManager.controllersManager.Controller.Value.prevPressedButtons)
            {
                Destroy(instantiatedScreen);
                yield break;

            }
            yield return null;
        }
    }
}
