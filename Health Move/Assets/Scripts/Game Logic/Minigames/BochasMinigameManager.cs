using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    public enum BochasThrowingMode
    {
        Bochador,
        Arrimador,
        Medio
    }

    Bochin _bochin;
    List<Bocha> _thrownBochas = new List<Bocha>();


    int _turnsLapsed = 0;

    bool _newRound = true;

    Dictionary<string, BochasThrowingMode> _playersThrowingModes = new Dictionary<string, BochasThrowingMode>(); //if needed, the key could be changed to Profile

    List<Profile> _profilesNotPlayedInRound;

    BochaGenerator _bochaGenerator;

    public Bochin Bochin { get { return _bochin; } set { _bochin = value; } }

    public List<Bocha> ThrownBochas { get { return _thrownBochas; } set { _thrownBochas = value; } }

    public Dictionary<string, BochasThrowingMode> PlayerThrowingModes { get { return _playersThrowingModes; } }

    public BochasMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BochasHands", typeof(GameObject)) as GameObject;

    }

    public override void Start()
    {
        base.Start();
        _bochaGenerator = GameObject.FindObjectOfType<BochaGenerator>();
        _profilesNotPlayedInRound = new List<Profile>(ProfileManager.pm.Profiles);
        ChangeCurrProfile(BochasThrowingMode.Arrimador, teams[Random.Range(0, teams.Length)].teamName);
        GameManager.gm.GeneratedHands += AddOnTurnStartedToGenerateHands;
        RestartControllers();
    }

    void AddOnTurnStartedToGenerateHands()
    {
        GameManager.gm.GeneratedHands -= AddOnTurnStartedToGenerateHands;
        GameManager.gm.GeneratedHands += OnTurnStarted;
        _bochaGenerator.GenerateBocha(_newRound ? "bochin" : currPlayerProfile.teamName);
    }

    public override void OnTurnStarted()
    {
        base.OnTurnStarted();

        //recalibrate controllers
        RestartControllers();

        //give a bocha to the player
        _bochaGenerator.GenerateBocha(_newRound ? "bochin" : currPlayerProfile.teamName);

    }

    public override void OnTurnEnded()
    {
        base.OnTurnEnded();

        if(_newRound)
        {
            //do bochin stuff like camera movement
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

        BochasThrowingMode currThrowingMode = PlayerThrowingModes[currPlayerProfile.name];
        if (_turnsLapsed % 4 == 0)//throwing mode change
        {
            switch (currThrowingMode)
            {
                case BochasThrowingMode.Bochador:
                    currThrowingMode = BochasThrowingMode.Arrimador;
                    break;
                case BochasThrowingMode.Arrimador:
                    currThrowingMode = BochasThrowingMode.Bochador;
                    break;
            }

        }

        if (_turnsLapsed % 2 == 0)//team change
        {
            ChangeCurrProfile(currThrowingMode, currPlayerProfile.teamName == teams[0].teamName ? teams[1].teamName : teams[0].teamName);

            GameManager.gm.ChangePlayer(teams.Where(x => x.teamName == currPlayerProfile.teamName).ToList()[0].teamColor);

        }
    }

    public void ThrownBocha(Transform bocha)
    {
        _thrownBochas.Add(bocha.GetComponent<Bocha>());
        Bochin.justThrownBocha = bocha;
    }

    public void RoundEnded()
    {
        KeyValuePair<string, float> closestBocha = new KeyValuePair<string, float>(_thrownBochas[0].bochaTeam, GetDistBocha(_thrownBochas[0].transform));
        int scoredPoints = 1;
        for(int i = 1; i < _thrownBochas.Count; i++)
        {
            float bochaDist = GetDistBocha(_thrownBochas[i].transform);
            if (_thrownBochas[i].bochaTeam == closestBocha.Key)
            {
                scoredPoints++;
                if (bochaDist < closestBocha.Value)
                    closestBocha = new KeyValuePair<string, float>(closestBocha.Key, bochaDist);
            }
            else
            {
                if (bochaDist < closestBocha.Value)
                {
                    scoredPoints = 1;
                    closestBocha = new KeyValuePair<string, float>(_thrownBochas[i].bochaTeam, bochaDist);

                }
            }
        }
        
    }

    float GetDistBocha(Transform bocha)
    {
        return Mathf.Abs((bocha.transform.position - Bochin.transform.position).magnitude);
    }

    void ChangeCurrProfile(BochasThrowingMode bochasThrowingMode, string newPlayerTeam)
    {
        List<Profile> possibleProfiles = _profilesNotPlayedInRound
            .Where(x => PlayerThrowingModes[x.name] == bochasThrowingMode && x.teamName == newPlayerTeam)
            .ToList();
        currPlayerProfile = possibleProfiles[Random.Range(0, possibleProfiles.Count)];
    }
}
