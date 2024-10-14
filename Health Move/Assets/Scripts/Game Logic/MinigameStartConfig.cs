using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameStartConfig : MonoBehaviour
{
    [SerializeField] Team[] teams;

    private void Start()
    {
        GameManager.gm.AddTeams(teams);
        GameManager.gm.CurrMinigameManager.Start();
        StartCoroutine(ControllersManager.controllersManager.KillTracking());
        StartCoroutine(ControllerCalibration.controllerCalibration.StartCalibration());
    }
}
