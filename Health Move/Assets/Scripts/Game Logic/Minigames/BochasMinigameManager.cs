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

    int _bochasThrown = 0;

    bool _newRound = true;

    Dictionary<string, BochasThrowingMode> _playersThrowingModes = new Dictionary<string, BochasThrowingMode>(); //if needed, the key could be changed to Profile

    List<Profile> _profilesNotPlayedInRound;

    BochaGenerator _bochaGenerator;

    Dictionary<string, int> _scored = new Dictionary<string, int>()
    {
        {TeamsHandler.tm.teamsByMinigame["BochasMinigame"][0].teamName, 0 },
        {TeamsHandler.tm.teamsByMinigame["BochasMinigame"][1].teamName, 0 }
    };

    int _scoreToWin = 6;



    public Bochin ThrownBochin { get { return _bochin; } set { _bochin = value; } }

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
        _scoreToWin *= ProfileManager.pm.Profiles.Count() / 2;

        Team startingTeam = teams[Random.Range(0, teams.Length - 1)];
        GameManager.gm.ChangePlayer(startingTeam.teamColor);

        ChangeCurrProfile(BochasThrowingMode.Arrimador, startingTeam.teamName);
        GameManager.gm.GeneratedHands += OnTurnStarted;
        RestartControllers();
    }

    public override void OnTurnStarted()
    {
        base.OnTurnStarted();

        //give a bocha to the player
        _bochaGenerator.GenerateBocha(_newRound ? "bochin" : currPlayerProfile.teamName);

    }

    public override void OnTurnEnded()
    {
        if(_newRound)
        {
            _newRound = false;
            ThrownBochin = GameObject.FindObjectOfType<Bochin>();
            OnTurnStarted();
            return;
        }
        if (_bochasThrown % 2 != 0)
        {
            OnTurnStarted();
            return;
        }

        bool roundEnded = false;
        if (_bochasThrown >= ProfileManager.pm.Profiles.Count * 2)//round ended
        {
            _bochasThrown = 0;
            roundEnded = true;

            bool gameEnded = RoundEnded();
            _thrownBochas.ForEach(x => GameObject.Destroy(x.gameObject));
            _thrownBochas.Clear();
            GameObject.Destroy(ThrownBochin.gameObject);
            _newRound = true;
            if(gameEnded)
            {
                GameManager.gm.EndMinigame();
                return;
            }
        }

        BochasThrowingMode currThrowingMode = PlayerThrowingModes[currPlayerProfile.name];
        if (_bochasThrown % 4 == 0 && !roundEnded)//throwing mode change
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

        ChangeCurrProfile(roundEnded ? BochasThrowingMode.Arrimador : currThrowingMode, currPlayerProfile.teamName == teams[0].teamName ? teams[1].teamName : teams[0].teamName);
        GameManager.gm.ChangePlayer(teams.Where(x => x.teamName == currPlayerProfile.teamName).ToList()[0].teamColor);
        //recalibrate controllers
        RestartControllers();
    }

    public void ThrownBocha(Transform bocha)
    {
        _thrownBochas.Add(bocha.GetComponent<Bocha>());
        ThrownBochin.justThrownBocha = bocha;
        _bochasThrown ++;
    }

    public bool RoundEnded()
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

        _scored[closestBocha.Key] += scoredPoints;

        if (_scored[closestBocha.Key] > _scoreToWin)
            return true;

        return false;

    }

    float GetDistBocha(Transform bocha)
    {
        return Mathf.Abs((bocha.transform.position - ThrownBochin.transform.position).magnitude);
    }

    void ChangeCurrProfile(BochasThrowingMode bochasThrowingMode, string newPlayerTeam)
    {
        List<Profile> possibleProfiles = _profilesNotPlayedInRound
            .Where(x => PlayerThrowingModes[x.name] == bochasThrowingMode && x.teamName == newPlayerTeam)
            .ToList();
        if(possibleProfiles.Count <= 0)
        {
            possibleProfiles = ProfileManager.pm.Profiles
            .Where(x => PlayerThrowingModes[x.name] == bochasThrowingMode && x.teamName == newPlayerTeam)
            .ToList();
            possibleProfiles.ForEach(x => _profilesNotPlayedInRound.Add(x));
        }
        if (possibleProfiles.Count <= 0)
        {
            ChangeCurrProfile(bochasThrowingMode == BochasThrowingMode.Arrimador ? BochasThrowingMode.Bochador : BochasThrowingMode.Arrimador, newPlayerTeam);
            return;
        }
        int index = Random.Range(0, possibleProfiles.Count);
        Debug.Log("Index: " + possibleProfiles[0]);
        currPlayerProfile = possibleProfiles[index];
        _profilesNotPlayedInRound.Remove(currPlayerProfile);
    }
}
