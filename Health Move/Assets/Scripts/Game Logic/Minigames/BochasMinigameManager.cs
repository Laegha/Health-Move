using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    Bochin _bochin;
    List<Bocha> _thrownBochas = new List<Bocha>();

    int _bochasThrown = 0;

    bool _newRound = true;

    Dictionary<Profile, int> _profilesTimesPlayedInRound;

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

    float _winScreenTime = 4;

    string _lastWinnerTeam;





    public Bochin ThrownBochin { get { return _bochin; } set { _bochin = value; } }

    public List<Bocha> ThrownBochas { get { return _thrownBochas; } set { _thrownBochas = value; } }

    public bool RoundEnding { get { return _roundEnding; } set { _roundEnding = value; } }

    public CinemachineVirtualCamera PlayerCam { get { return _playerCam; } }

    public Dictionary<string, int> Scored { get { return _scored; } }

    public BochasMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BochasHands", typeof(GameObject)) as GameObject;

    }

    public override void CalibrateProfile()
    {
        Team team = teams.Where(x => x.teamName == _lastWinnerTeam).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor);
        //Choose profile from team
        ProfileSelectScreen profileSelectScreen = GameManager.gm.GenerateScreen("profileselect").GetComponent<ProfileSelectScreen>();
        
        foreach(var profile in GetPossibleNextProfiles(_lastWinnerTeam))
        {
            ProfileSelectBtn profileSelectBtn = GameObject.Instantiate(profileSelectScreen.buttonPrefab, profileSelectScreen.grid.transform).GetComponent<ProfileSelectBtn>();
            profileSelectBtn.ProfileName = profile.name;
            profileSelectBtn.Text.text = ProfileManager.pm.GetProfileTextFromKey(profile.name);
            profileSelectBtn.CallbackOnPressed = ProfileSelectBtnCallback;
        }

    }

    void ProfileSelectBtnCallback(string selectedProfileName)
    {
        currPlayerProfile = _profilesTimesPlayedInRound.Keys.Where(profile => profile.name == selectedProfileName).FirstOrDefault();
        UpdateGfx();
        SelectedProfile();

        base.CalibrateProfile();
    }

    void UpdateGfx()
    {
        Team team = teams.Where(x => x.teamName == _lastWinnerTeam).ToList()[0];

        _playingPlayerText.text = ProfileManager.pm.GetProfileTextFromKey(currPlayerProfile.name);
        _playingPlayerText.color = team.teamColor;
    }

    public override void Start()
    {
        base.Start();
        _bochaGenerator = GameObject.FindObjectOfType<BochaGenerator>();
        ProfileManager.pm.Profiles.ForEach(profile => _profilesTimesPlayedInRound.Add(profile, 0));

        _scoreToWin *= ProfileManager.pm.Profiles.Count() / 2;

        Team startingTeam = teams[Random.Range(0, teams.Length)];

        _playingPlayerText = GameObject.FindObjectOfType<PositionCalibrationScreen>().transform.Find("GFX").transform.Find("TeamTxt").GetComponent<TextMeshProUGUI>();

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
        ResetPlayerPosition(GameManager.gm.ActiveHand.transform);
        if ((CinemachineVirtualCamera)(_cmBrain.ActiveVirtualCamera) != _playerCam)
        {
            _cmBrain.ActiveVirtualCamera.Priority = 0;
            _playerCam.Priority = 1;
        }

    }

    void ResetPlayerPosition(Transform player)
    {
        player.position = GameObject.Find("HandSpawner").transform.position;
        player.rotation = Quaternion.identity;
    }

    public override void OnTurnEnded()
    {
        if (_newRound)
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

        while (_cmBrain.IsBlending) yield return null;

        bool roundEnded = false;
        if (_bochasThrown >= ProfileManager.pm.Profiles.Count * 2)//round ended
        {
            _bochasThrown = 0;
            roundEnded = true;

            bool gameEnded = RoundEnded();
            if (gameEnded)
            {
                Transform endScreen = GameManager.gm.FindInChildren(GameObject.Find("Canvas").transform, "EndMinigameScreen");
                endScreen.gameObject.SetActive(true);
                TextMeshProUGUI winnerText = endScreen.transform.Find("WinnerText").GetComponent<TextMeshProUGUI>();
                winnerText.text = currPlayerProfile.teamName;
                winnerText.color = teams.Where(x => x.teamName == currPlayerProfile.teamName).ToArray()[0].teamColor;

                yield return new WaitForSeconds(_winScreenTime);
                GameManager.gm.EndMinigame();
                yield break;
            }

            while (RoundEnding) yield return null;
            _thrownBochas.ForEach(x => GameObject.Destroy(x.gameObject));
            _thrownBochas.Clear();
            GameObject.Destroy(ThrownBochin.gameObject);
            _newRound = true;

        }

        

        //recalibrate controllers
        RestartControllers();
    }

    public void ThrownBocha(Transform bocha)
    {
        _thrownBochas.Add(bocha.GetComponent<Bocha>());
        ThrownBochin.justThrownBocha = bocha;
        _bochasThrown++;
    }

    public bool RoundEnded()
    {
        float firstBochaDist = GetDistBocha(_thrownBochas[0].transform);
        KeyValuePair<string, float> closestBocha = new KeyValuePair<string, float>(_thrownBochas[0].bochaTeam, firstBochaDist);
        _thrownBochas[0].bochaDist = firstBochaDist;

        KeyValuePair<string, float> closestBochaRival = new KeyValuePair<string, float>("null", 1000);//this num is so big so the first bochas of the same team can score

        List<Bocha> scoringBochas = new List<Bocha>() { _thrownBochas[0] };
        for (int i = 1; i < _thrownBochas.Count; i++)
        {
            float bochaDist = GetDistBocha(_thrownBochas[i].transform);
            _thrownBochas[i].bochaDist = bochaDist;

            if (_thrownBochas[i].bochaTeam != closestBocha.Key)
            {
                Debug.Log("Bocha enemiga: " + _thrownBochas[i].name);
                scoringBochas
                    .Where(bocha => bochaDist < bocha.bochaDist)
                    .ToList()
                    .ForEach(bocha => scoringBochas
                    .Remove(bocha));
                if (bochaDist < closestBocha.Value)
                {
                    scoringBochas.Add(_thrownBochas[i]);

                    closestBochaRival = new KeyValuePair<string, float>(closestBocha.Key, closestBocha.Value);
                    closestBocha = new KeyValuePair<string, float>(_thrownBochas[i].bochaTeam, bochaDist);

                }
                else if (bochaDist < closestBochaRival.Value)
                {
                    closestBochaRival = new KeyValuePair<string, float>(_thrownBochas[i].bochaTeam, bochaDist);
                }
            }

            else if (_thrownBochas[i].bochaTeam == closestBocha.Key && bochaDist < closestBochaRival.Value)
            {
                scoringBochas.Add(_thrownBochas[i]);
                if (bochaDist < closestBocha.Value)
                    closestBocha = new KeyValuePair<string, float>(closestBocha.Key, bochaDist);
            }
        }


        _scored[closestBocha.Key] += scoringBochas.Count;
        _lastWinnerTeam = closestBocha.Key;
        scoringBochas.ForEach(x => x.scoring = true);
        GameManager.gm.OnScored(GameManager.gm.ActiveHand.GetComponent<PlayerIdentifier>());
        RoundEnding = true;

        if (_scored[closestBocha.Key] >= _scoreToWin)
            return true;

        return false;
    }

    float GetDistBocha(Transform bocha)
    {
        return Mathf.Abs((bocha.transform.position - ThrownBochin.transform.position).magnitude);
    }

    List<Profile> GetPossibleNextProfiles(string profileTeam)
    {
        List<Profile> possibleProfiles = _profilesTimesPlayedInRound.Keys
            .Where(x => x.teamName == profileTeam)
            .ToList();
        if (possibleProfiles.Count == 0)
        {
            possibleProfiles = ProfileManager.pm.Profiles
                .Where(x => x.teamName == profileTeam)
                .ToList();

            possibleProfiles.ForEach(profile => _profilesTimesPlayedInRound.Add(profile, 0));

        }
        return possibleProfiles;
    }

    void SelectedProfile()
    {

    }
}