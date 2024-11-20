using System;
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
        KillTracking(() =>
        {
            CalibrateControllers(() =>
            {
                CalibrateProfile();
            });
        });
        
    }

    public virtual void KillTracking(Action callback)
    {
        GameManager.gm.RoutineRunner(GameManager.gm.KillControllerTracking(callback));
    }

    public virtual void CalibrateControllers(Action callback)
    {
        GameManager.gm.RoutineRunner(ControllerCalibration.controllerCalibration.StartCalibration(callback));
    }

    public virtual void CalibrateProfile()
    {
        if (!currPlayerProfile.calibrated)
            CalibrateSensitivity(() =>
            {
                CalibratePosition();
            });
        else
            CalibratePosition();
    }

    public virtual void CalibrateSensitivity(Action callback)
    {
        currPlayerProfile.calibrated = true;
        GameManager.gm.GetProfileSensitivity(currPlayerProfile, callback);
    }

    public virtual void CalibratePosition()
    {
        GameManager.gm.CalibratePosition();
    }

    public virtual void OnTurnEnded() { }
    public virtual void OnTurnStarted() { }
}
