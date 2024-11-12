using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BasquetMinigameManager : MinigameManager
{
    Dictionary<string, int> _scored = new Dictionary<string, int>();
    int _neededScore = 3;
    string _currentTeam;

    TextMeshProUGUI _playingPlayerText;

    Animator _textAnimator;

    readonly float _endScreenSeconds = 1.5f;

    BallGenerator _ballGenerator;

    List<Profile> _profilesNotPlayedInTurn;

    bool _hasEnded = false;

    public Dictionary<string, int> Scored {  get { return _scored; } }

    public BasquetMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BasquetHands", typeof(GameObject)) as GameObject;
    }

    public override void Start()
    {
        base.Start();

        _profilesNotPlayedInTurn = new List<Profile>(ProfileManager.pm.Profiles);
        foreach (Team x in teams)
        {
            _scored.Add(x.teamName, 0);

        }

        _currentTeam = Scored.Keys.ToList()[Random.Range(0, Scored.Count)];
        Profile[] possibleProfiles = _profilesNotPlayedInTurn.Where(x => x.teamName == _currentTeam).ToArray();
        currPlayerProfile = possibleProfiles[Random.Range(0, possibleProfiles.Length -1)];
        _profilesNotPlayedInTurn.Remove(currPlayerProfile);

        Team team = teams.Where(x => x.teamName == _currentTeam).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor);

        _textAnimator = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Animator>();

        _playingPlayerText = GameObject.FindObjectOfType<PositionCalibrationScreen>().transform.Find("GFX").transform.Find("TeamTxt").GetComponent<TextMeshProUGUI>();
        _playingPlayerText.color = team.teamColor;
        _playingPlayerText.text = currPlayerProfile.name;

        _ballGenerator = GameObject.FindObjectOfType<BallGenerator>();
        _ballGenerator.Initiate();
        
        GameManager.gm.GeneratedHands += OnTurnStarted;
        RestartControllers();
    }

    public override void OnTurnStarted()
    {
        _ballGenerator.GenerateBall();
    }

    public override void OnScored(PlayerIdentifier scorer)
    {
        base.OnScored(scorer);

        _textAnimator.Play("Scored");

        _scored[scorer.playerTeam]++;
        if (_scored[scorer.playerTeam] == _neededScore)
            GameManager.gm.RoutineRunner(EndMinigame());
    }

    public override void OnTurnEnded()
    {
        if (_hasEnded)
            return;
        base.OnTurnEnded();

        if (_currentTeam == _scored.Keys.ToList()[0])
            _currentTeam = _scored.Keys.ToList()[1];
        
        else 
            _currentTeam = _scored.Keys.ToList()[0];

        Team team = teams.Where(x => x.teamName == _currentTeam).ToList()[0];

        List<Profile> possibleProfiles = _profilesNotPlayedInTurn.Where(x => x.teamName == _currentTeam).ToList();
        if(possibleProfiles.Count == 0)
        {
            foreach(Profile profile in ProfileManager.pm.Profiles) 
            {
                if (profile.teamName == _currentTeam)
                {
                    _profilesNotPlayedInTurn.Add(profile);
                    possibleProfiles.Add(profile);
                }
            }
        }
        currPlayerProfile = possibleProfiles[Random.Range(0, possibleProfiles.Count - 1)];
        _profilesNotPlayedInTurn.Remove(currPlayerProfile);

        GameManager.gm.ChangePlayer(team.teamColor);
        _playingPlayerText.text = currPlayerProfile.name;
        _playingPlayerText.color = team.teamColor;

        RestartControllers();
    }

    IEnumerator EndMinigame()
    {
        _hasEnded = true;
        Transform endScreen = GameManager.gm.FindInChildren(GameObject.Find("Canvas").transform, "EndMinigameScreen");
        endScreen.gameObject.SetActive(true);
        string winnerTeam = _scored[_scored.Keys.ToList()[0]] > _scored[_scored.Keys.ToList()[1]] ? _scored.Keys.ToList()[0] : _scored.Keys.ToList()[1];
        TextMeshProUGUI winnerText = endScreen.Find("WinnerTxt").GetComponent<TextMeshProUGUI>();
        winnerText.text = winnerTeam;
        winnerText.color = teams.Where(x => x.teamName == winnerTeam).ToList()[0].teamColor;
        yield return new WaitForSeconds(_endScreenSeconds);
        GameManager.gm.EndMinigame();
    }
}
