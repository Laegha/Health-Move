using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    Bochin _bochin;
    List<Transform> _thrownBochas = new List<Transform>();

    BochasPlayer.BochasThrowingMode _currBochasThrowingMode = BochasPlayer.BochasThrowingMode.Arrimador;

    int _turnsLapsed = 0;

    bool _newRound = true;

    Dictionary<string, BochasPlayer.BochasThrowingMode> _playersThrowingModes = new Dictionary<string, BochasPlayer.BochasThrowingMode>(); //if needed, the key could be changed to Profile

    BochaGenerator _bochaGenerator;

    public Bochin Bochin { get { return _bochin; } set { _bochin = value; } }

    public List<Transform> ThrownBochas { get { return _thrownBochas; } set { _thrownBochas = value; } }

    public Dictionary<string, BochasPlayer.BochasThrowingMode> PlayerThrowingModes { get { return _playersThrowingModes; } }

    public BochasMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BochasHands", typeof(GameObject)) as GameObject;

    }

    public override void Start()
    {
        base.Start();
        _bochaGenerator = GameObject.FindObjectOfType<BochaGenerator>();
    }

    void SetThrowerMode()
    {
        GameManager.gm.ActiveHand.GetComponent<BochasPlayer>().throwingMode = _currBochasThrowingMode;
    }

    public override void OnTurnEnded()
    {
        base.OnTurnEnded();

        if(_newRound)
        {
            //do bochin stuff
            _newRound = false;
            OnTurnStarted();
            Bochin = GameObject.FindObjectOfType<Bochin>();
            return;
        }

        if (_turnsLapsed > ProfileManager.pm.Profiles.Count * 2)//round ended
        {
            _turnsLapsed = 0;
            RoundEnded();
        }

        if (_turnsLapsed % 4 == 0)//throwing mode change
        {
            switch (_currBochasThrowingMode)
            {
                case BochasPlayer.BochasThrowingMode.Bochador:
                    _currBochasThrowingMode = BochasPlayer.BochasThrowingMode.Arrimador;
                    break;
                case BochasPlayer.BochasThrowingMode.Arrimador:
                    _currBochasThrowingMode = BochasPlayer.BochasThrowingMode.Bochador;
                    break;
            }

        }

        if (_turnsLapsed % 2 == 0)//team change
        {
            ChangeCurrProfile();

            //GameManager.gm.ChangePlayer(teams[_currTeam].teamColor);

        }
    }

    public void ThrownBocha(Transform bocha)
    {
        _thrownBochas.Add(bocha);
        Bochin.justThrownBocha = bocha;
    }

    public override void OnTurnStarted()
    {
        base.OnTurnStarted();

        //recalibrate controllers
        GameManager.gm.KillControllerTracking(base.RestartControllers);

        //give a bocha to the player
        _bochaGenerator.GenerateBocha(_newRound ? "bochin" : currPlayerProfile.teamName);      
       
    }

    public void RoundEnded()
    {
        Dictionary<string, float> bochasDistances = new Dictionary<string, float>();
        foreach(Transform bocha in ThrownBochas)
        {
            float distance = Vector3.Distance(Bochin.transform.position, bocha.position);
            bochasDistances.Add(bocha.GetComponent<PlayerIdentifier>().playerTeam, distance);
        }
    }

    void ChangeCurrProfile()
    {

    }
}
