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

    public BasquetMinigameManager()
    {
        minigameHandPrefab = Resources.Load("Prefabs/Hands/BasquetHands", typeof(GameObject)) as GameObject;
    }

    public override void OnScored(PlayerIdentifier scorer)
    {
        base.OnScored(scorer);

        //_scored[scorer.playerID]++;
        //if (_scored[scorer.playerID] == _neededScore)
        //    GameManager.gm.EndMinigame();

        scored++;
        if(scored >= _neededScore) 
            GameManager.gm.EndMinigame();
    }
}
