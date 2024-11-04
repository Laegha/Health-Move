using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    Transform _bochin;
    List<Transform> _thrownBochas = new List<Transform>();

    int _team1Points;
    int _team2Points;

    BochasPlayer.BochasThrowingMode _bochasThrowingMode = BochasPlayer.BochasThrowingMode.Arrimador;

    int _turnsLapsed = 0;
    int _playerAmmount = 4; //this is intended to be selectable by the player

    int _currTeam;

    public Transform Bochin { get { return _bochin; } set { _bochin = value; } }
    public List<Transform> ThrownBochas { get { return _thrownBochas; } set { _thrownBochas = value; } }

    public BochasMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BochasHands", typeof(GameObject)) as GameObject;

    }

    void SetThrowerMode()
    {
        GameManager.gm.ActiveHand.GetComponent<BochasPlayer>().throwingMode = _bochasThrowingMode;
    }

    public override void OnTurnEnded()
    {
        base.OnTurnEnded();

        if (_turnsLapsed > _playerAmmount * 2)//round ended
        {
            _turnsLapsed = 0;
            RoundEnded();
        }

        if (_turnsLapsed % 2 == 0)//team change
        {
            if (_currTeam == 0)
                _currTeam = 1;
            else
                _currTeam = 0;
            GameManager.gm.ChangePlayer(teams[_currTeam].teamColor, teams[_currTeam].teamName);
            base.RestartControllers();

        }

        if(_turnsLapsed % 4 == 0)//throwing mode change
        {
            switch (_bochasThrowingMode)
            {
                case BochasPlayer.BochasThrowingMode.Bochador:
                    _bochasThrowingMode = BochasPlayer.BochasThrowingMode.Arrimador;
                    break;
                case BochasPlayer.BochasThrowingMode.Arrimador:
                    _bochasThrowingMode = BochasPlayer.BochasThrowingMode.Bochador;
                    break;
            }

        }
        //give a bocha to the player
        //display position calibrate screen
    }

    public void RoundEnded()
    {
        Dictionary<string, float> bochasDistances = new Dictionary<string, float>();
        foreach(Transform bocha in ThrownBochas)
        {
            float distance = Vector3.Distance(_bochin.position, bocha.position);
            bochasDistances.Add(bocha.GetComponent<PlayerIdentifier>().playerTeam, distance);
        }
    }

    public void BochinStopped()
    {
        //display position indicator message
        //give a bocha to the thrower
    }
}
