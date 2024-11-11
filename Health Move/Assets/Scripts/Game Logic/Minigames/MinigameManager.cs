using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

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
        teams = TeamsHandler.tm.teamsByMinigame[Regex.Replace(GetType().ToString(), "Manager", "", RegexOptions.IgnoreCase)];
        
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
                    GameManager.gm.GetProfileSensitivity(currPlayerProfile, () =>
                    {
                        GameManager.gm.ResetHands();
                    });
                }
                else
                    GameManager.gm.ResetHands();
            }));
        }));
        
    }
    
    public virtual void OnTurnEnded() { }
    public virtual void OnTurnStarted() { }
}
