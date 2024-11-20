using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    Bochin _bochin;
    List<Bocha> _thrownBochas = new List<Bocha>();

    int _bochasThrown = 0;

    bool _newRound = true;

    Dictionary<Profile, int> _profilesTimesPlayedInRound = new Dictionary<Profile, int>();

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

    string _currPlayingTeam;

    GameObject _profileSelectScreen;
    GameObject _cursor;



    public Bochin ThrownBochin { get { return _bochin; } set { _bochin = value; } }

    public List<Bocha> ThrownBochas { get { return _thrownBochas; } set { _thrownBochas = value; } }

    public bool RoundEnding { get { return _roundEnding; } set { _roundEnding = value; } }

    public CinemachineVirtualCamera PlayerCam { get { return _playerCam; } }

    public Dictionary<string, int> Scored { get { return _scored; } }

    public BochasMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BochasHands", typeof(GameObject)) as GameObject;

    }
    public override void Start()
    {
        base.Start();
        _bochaGenerator = GameObject.FindObjectOfType<BochaGenerator>();
        ProfileManager.pm.Profiles.ForEach(profile => _profilesTimesPlayedInRound.Add(profile, 0));

        //_scoreToWin *= ProfileManager.pm.Profiles.Count() / 2;

        _currPlayingTeam = teams[Random.Range(0, teams.Length)].teamName;

        _playingPlayerText = GameObject.FindObjectOfType<PositionCalibrationScreen>().transform.Find("GFX").transform.Find("TeamTxt").GetComponent<TextMeshProUGUI>();

        _playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        _cmBrain = GameObject.FindObjectOfType<CinemachineBrain>();

        GameManager.gm.GeneratedHands += OnTurnStarted;
        RestartControllers();
    }


    public override void CalibrateControllers(System.Action callback)
    {
        Team team = teams.Where(x => x.teamName == _currPlayingTeam).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor);

        base.CalibrateControllers(callback);
    }

    public override void CalibrateProfile()
    {
        Team team = teams.Where(x => x.teamName == _currPlayingTeam).ToList()[0];
        //Choose profile from team
        _profileSelectScreen = GameManager.gm.GenerateScreen("profileselect");
        ProfileSelectScreen profileSelectScreen = _profileSelectScreen.GetComponent<ProfileSelectScreen>();
        Vector2 cellSize = profileSelectScreen.grid.cellSize;

        List<Profile> availableProfiles = GetPossibleNextProfiles(_currPlayingTeam);
        int generatedBtns = 0;
        foreach (var profile in availableProfiles)
        {
            generatedBtns++;
            ProfileSelectBtn profileSelectBtn = GameObject.Instantiate(profileSelectScreen.buttonPrefab, profileSelectScreen.grid.transform).GetComponent<ProfileSelectBtn>();
            profileSelectBtn.ProfileName = profile.name;
            profileSelectBtn.Text.text = ProfileManager.pm.GetProfileTextFromKey(profile.name);
            profileSelectBtn.Text.color = team.teamColor;
            profileSelectBtn.CallbackOnPressed = ProfileSelectBtnCallback;
        }
        profileSelectScreen.grid.cellSize = cellSize / generatedBtns;

        _cursor = GameManager.gm.GenerateCursor();
        GameManager.gm.RoutineRunner(ResetCursorPosition());
    }

    IEnumerator ResetCursorPosition()//this is SUPER gross
    {
        yield return new WaitForSeconds(0.01f);

        _cursor.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
    }

    void ProfileSelectBtnCallback(string selectedProfileName)
    {
        currPlayerProfile = _profilesTimesPlayedInRound.Keys.Where(profile => profile.name == selectedProfileName).FirstOrDefault();
        UpdateGfx();
        SelectedProfile();
        GameObject.Destroy(_profileSelectScreen);
        GameObject.Destroy(_cursor);

        base.CalibrateProfile();
    }

    void UpdateGfx()
    {
        Team team = teams.Where(x => x.teamName == _currPlayingTeam).ToList()[0];

        _playingPlayerText.text = ProfileManager.pm.GetProfileTextFromKey(currPlayerProfile.name);
        _playingPlayerText.color = team.teamColor;
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

        GameManager.gm.RoutineRunner(_thrownBochas.Last().GetComponent<BochaCamera>().FocusBocha(EndedTurn));


    }

    void EndedTurn()
    {
        GameManager.gm.RoutineRunner(EndedTurnRoutine());
    }

    IEnumerator EndedTurnRoutine()
    {
        _cmBrain.ActiveVirtualCamera.Priority = 0;
        _playerCam.Priority = 1;

        while (_cmBrain.IsBlending) yield return null;

        bool roundEnded = false;
        if (_bochasThrown >= ProfileManager.pm.Profiles.Count * 2)//round ended
        {
            roundEnded = true;
            _bochasThrown = 0;

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
        if(_currPlayingTeam == teams[0].teamName && !roundEnded)
            _currPlayingTeam = teams[1].teamName;
        else if(_currPlayingTeam == teams[1].teamName && !roundEnded)
            _currPlayingTeam = teams[0].teamName;
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
        _currPlayingTeam = closestBocha.Key;
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
        _profilesTimesPlayedInRound[currPlayerProfile]++;
        if (_profilesTimesPlayedInRound[currPlayerProfile] >= 2)
            _profilesTimesPlayedInRound.Remove(currPlayerProfile);
    }
}