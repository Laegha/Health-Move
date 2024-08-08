using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BasquetMinigameManager : MinigameManager
{
    Dictionary<int, int> _scored = new Dictionary<int, int>();
    int _neededScore = 3;

    public override void OnScored(PlayerIdentifier scorer)
    {
        base.OnScored(scorer);
        Debug.Log("Score = " + _scored);
        _scored[scorer.playerID]++;
        GameObject.FindGameObjectsWithTag("Counter").ToList().Where(x => x.GetComponent<PlayerIdentifier>().playerID == scorer.playerID).ToList()[0].GetComponent<Text>().text = _scored[scorer.playerID].ToString();
        if (_scored[scorer.playerID] == _neededScore)
            GameManager.gm.EndMinigame();
    }
}
