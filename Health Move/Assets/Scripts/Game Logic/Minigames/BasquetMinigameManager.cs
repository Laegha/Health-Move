using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasquetMinigameManager : MinigameManager
{
    Dictionary<string, int> _scored = new Dictionary<string, int>();
    int _neededScore = 3;
    public int scored = 0;
    string _currentTeam = "Azul";

    TextMeshProUGUI _teamNameText;

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

        Team team = teams.Where(x => x.teamName == _currentTeam).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor, team.teamName);

        _teamNameText = GameObject.FindObjectOfType<PositionCalibrationScreen>().transform.Find("GFX").transform.Find("TeamTxt").GetComponent<TextMeshProUGUI>();
        _teamNameText.color = team.teamColor;
    }

    public override void OnScored(PlayerIdentifier scorer)
    {
        base.OnScored(scorer);

        _scored[scorer.playerTeam]++;
        if (_scored[scorer.playerTeam] == _neededScore)
            GameManager.gm.EndMinigame();
    }

    public override void OnTurnEnded()
    {
        base.OnTurnEnded();

        if (_currentTeam == "Azul")
            _currentTeam = "Rojo";
        
        else 
            _currentTeam = "Azul";

        Team team = teams.Where(x => x.teamName == _currentTeam).ToList()[0];
        GameManager.gm.ChangePlayer(team.teamColor, team.teamName);
        _teamNameText.text = team.teamName;
        _teamNameText.color = team.teamColor;
        GameManager.gm.RecalibrateControllers();
    }
}
