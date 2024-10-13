using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BasquetMinigameManager : MinigameManager
{
    Dictionary<int, int> _scored = new Dictionary<int, int>();
    int _neededScore = 3;
    public int scored = 0;
    int _currentPlayer = 0;

    public BasquetMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BasquetHands", typeof(GameObject)) as GameObject;
    }

    public override void Start()
    {
        base.Start();

        foreach (Team team in teams)
            _scored.Add(team.teamIndex, 0);
    }

    public override void OnScored(PlayerIdentifier scorer)
    {
        base.OnScored(scorer);

        _scored[scorer.playerID]++;
        if (_scored[scorer.playerID] == _neededScore)
            GameManager.gm.EndMinigame();
    }

    public override void OnTurnEnded()
    {
        base.OnTurnEnded();

        if (_currentPlayer == 0)
            _currentPlayer = 1;
        
        else 
            _currentPlayer = 0;
        
        GameManager.gm.ChangePlayer(teams[_currentPlayer].teamColor, teams[_currentPlayer].teamIndex);
        GameManager.gm.RecalibrateControllers();
    }
}
