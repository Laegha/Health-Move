using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class BasquetMinigameManager : MinigameManager
{
    Dictionary<string, int> _scored = new Dictionary<string, int>();
    int _neededScore = 3;
    string _currentTeam;

    TextMeshProUGUI _teamNameText;

    Animator _textAnimator;

    readonly float _endScreenSeconds = 2;

    public Dictionary<string, int> Scored {  get { return _scored; } }

    public BasquetMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BasquetHands", typeof(GameObject)) as GameObject;
    }

    public override void Start()
    {
        base.Start();

        foreach (Team x in teams)
        {
            _scored.Add(x.teamName, 0);

        }

        _currentTeam = _scored.Keys.ToList()[0];

        Team team = teams.Where(x => x.teamName == _currentTeam).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor, team.teamName);

        _textAnimator = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Animator>();

        _teamNameText = GameObject.FindObjectOfType<PositionCalibrationScreen>().transform.Find("GFX").transform.Find("TeamTxt").GetComponent<TextMeshProUGUI>();
        _teamNameText.color = team.teamColor;
        _teamNameText.text = team.teamName;


        Debug.Log(_teamNameText);
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
        base.OnTurnEnded();

        if (_currentTeam == _scored.Keys.ToList()[0])
            _currentTeam = _scored.Keys.ToList()[1];
        
        else 
            _currentTeam = _scored.Keys.ToList()[0];

        Team team = teams.Where(x => x.teamName == _currentTeam).ToList()[0];
        
        GameManager.gm.ChangePlayer(team.teamColor, team.teamName);
        _teamNameText.text = team.teamName;
        _teamNameText.color = team.teamColor;
        GameManager.gm.RoutineRunner(GameManager.gm.RecalibrateControllers());
    }

    IEnumerator EndMinigame()
    {
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
