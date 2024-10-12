using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    Transform _bochin;

    int _team1Points;
    int _team2Points;

    public Transform Bochin {  get { return _bochin; } set { _bochin = value; } }

    public override void OnTurnEnded()
    {
        base.OnTurnEnded();
        //change thrower mode?
        //team change?
        //round ended?
        //display position calibrate screen
    }

    public void BochinStopped()
    {
        //display position indicator message
        //give a bocha to the thrower
    }
}
