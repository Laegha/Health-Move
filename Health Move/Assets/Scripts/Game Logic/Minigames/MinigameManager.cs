using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles logic of a minigame
public class MinigameManager
{
    public bool hasEnded = false;

    public GameObject minigameHandPrefab;

    public Team[] teams;

    public Profile currPlayerProfile;

    public virtual void Start() 
    {
        GameManager.gm.RoutineRunner(ControllersManager.controllersManager.KillTracking());
        teams = TeamsHandler.tm.teamsByMinigame[GetType().ToString()];
    }
    public virtual void OnScored(PlayerIdentifier scorer) { } //Is called by GM OnScored

    public virtual void RestartControllers() 
    {
        GameManager.gm.RoutineRunner(GameManager.gm.KillControllerTracking(() =>
        {
            GameManager.gm.RoutineRunner(ControllerCalibration.controllerCalibration.StartCalibration(() =>
            {
                if (!currPlayerProfile.calibrated)
                {
                    currPlayerProfile.calibrated = true;
                    //Calibrate profile
                }
                GameManager.gm.ResetHands();
            }));
        }));
        
    }
    
    public virtual void OnTurnEnded() { }
    public virtual void OnTurnStarted() { }
}
