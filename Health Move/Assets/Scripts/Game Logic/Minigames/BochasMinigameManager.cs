using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
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

    CinemachineVirtualCamera _playerCam;

    CinemachineBrain _cmBrain;

    bool _roundEnding = false;

    TextMeshProUGUI _playingPlayerText;



    public Bochin ThrownBochin { get { return _bochin; } set { _bochin = value; } }

    public List<Bocha> ThrownBochas { get { return _thrownBochas; } set { _thrownBochas = value; } }

    public Dictionary<string, BochasThrowingMode> PlayerThrowingModes { get { return _playersThrowingModes; } }

    public bool RoundEnding{ get { return _roundEnding; } set { _roundEnding = value; } }

    public CinemachineVirtualCamera PlayerCam { get { return _playerCam; } }

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

        Team startingTeam = teams[Random.Range(0, teams.Length)];

        _playingPlayerText = GameObject.FindObjectOfType<PositionCalibrationScreen>().transform.Find("GFX").transform.Find("TeamTxt").GetComponent<TextMeshProUGUI>();

        ChangeCurrProfile(BochasThrowingMode.Arrimador, startingTeam.teamName);

        GameManager.gm.ChangePlayer(startingTeam.teamColor);
        if (currPlayerProfile.name[0] == ':')
            _playingPlayerText.text = "";
        else
        {
            int cutPosition = currPlayerProfile.name.IndexOf(".");
            if (cutPosition > 0)
                _playingPlayerText.text = currPlayerProfile.name.Substring(0, cutPosition);
            else
                _playingPlayerText.text = currPlayerProfile.name;

        }
        _playingPlayerText.color = teams.Where(x => x.teamName == currPlayerProfile.teamName).ToArray()[0].teamColor;

        _playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        _cmBrain = GameObject.FindObjectOfType<CinemachineBrain>();

        GameManager.gm.GeneratedHands += OnTurnStarted;
        RestartControllers();
    }

    public override void OnTurnStarted()
    {
        base.OnTurnStarted();

        //give a bocha to the player
        _bochaGenerator.GenerateBocha(_newRound ? "bochin" : currPlayerProfile.teamName);
        GameManager.gm.ActiveHand.GetComponent<HandMovement>().isMoving = true;
        if ((CinemachineVirtualCamera)(_cmBrain.ActiveVirtualCamera) != _playerCam)
        {
            _cmBrain.ActiveVirtualCamera.Priority = 0;
            _playerCam.Priority = 1;
        }

    }

    public override void OnTurnEnded()
    {
        if(_newRound)
        {
            _newRound = false;
            ThrownBochin = GameObject.FindObjectOfType<Bochin>();
            GameManager.gm.RoutineRunner(ThrownBochin.GetComponent<BochaCamera>().FocusBocha(OnTurnStarted));
            return;
        }
        if (_bochasThrown % 2 != 0)
        {
            GameManager.gm.RoutineRunner(_thrownBochas.Last().GetComponent<BochaCamera>().FocusBocha(OnTurnStarted));
            return;
        }

        GameManager.gm.RoutineRunner(_thrownBochas.Last().GetComponent<BochaCamera>().FocusBocha(EndedProfileTurns));


    }

    void EndedProfileTurns()
    {
        GameManager.gm.RoutineRunner(EndedProfileTurnsRoutine());
    }

    IEnumerator EndedProfileTurnsRoutine()
    {
        _cmBrain.ActiveVirtualCamera.Priority = 0;
        _playerCam.Priority = 1;

        while(_cmBrain.IsBlending) yield return null;

        bool roundEnded = false;
        if (_bochasThrown >= ProfileManager.pm.Profiles.Count * 2)//round ended
        {
            _bochasThrown = 0;
            roundEnded = true;

            bool gameEnded = RoundEnded(); 
            if (gameEnded)
            {
                GameManager.gm.EndMinigame();
                yield break;
            }

            while (RoundEnding) yield return null;
            _thrownBochas.ForEach(x => GameObject.Destroy(x.gameObject));
            _thrownBochas.Clear();
            GameObject.Destroy(ThrownBochin.gameObject);
            _newRound = true;
            
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
        Team team = teams.Where(x => x.teamName == currPlayerProfile.teamName).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor);
        //recalibrate controllers
        if (currPlayerProfile.name[0] == ':')
            _playingPlayerText.text = "";
        else
        {
            int cutPosition = currPlayerProfile.name.IndexOf(".");
            if (cutPosition > 0)
                _playingPlayerText.text = currPlayerProfile.name.Substring(0, cutPosition);
            else
                _playingPlayerText.text = currPlayerProfile.name;

        }
        _playingPlayerText.color = team.teamColor;
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
        List<Bocha> scoringBochas = new List<Bocha>(){ _thrownBochas[0] };
        for(int i = 1; i < _thrownBochas.Count; i++)
        {
            float bochaDist = GetDistBocha(_thrownBochas[i].transform);
            if (_thrownBochas[i].bochaTeam == closestBocha.Key)
            {
                scoringBochas.Add(_thrownBochas[i]);
                if (bochaDist < closestBocha.Value)
                    closestBocha = new KeyValuePair<string, float>(closestBocha.Key, bochaDist);
            }
            else
            {
                if (bochaDist < closestBocha.Value)
                {
                    scoringBochas.Clear();
                    closestBocha = new KeyValuePair<string, float>(_thrownBochas[i].bochaTeam, bochaDist);

                }
            }
        }

        _scored[closestBocha.Key] += scoringBochas.Count;
        scoringBochas.ForEach(x => x.scoring = true);
        GameManager.gm.OnScored(GameManager.gm.ActiveHand.GetComponent<PlayerIdentifier>());
        RoundEnding = true;

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
        currPlayerProfile = possibleProfiles[index];
        _profilesNotPlayedInRound.Remove(currPlayerProfile);
    }
}
