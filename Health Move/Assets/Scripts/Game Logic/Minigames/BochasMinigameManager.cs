using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasMinigameManager : MinigameManager
{
    Transform _bochin;

    int _player1Points;
    int _player2Points;

    public Transform Bochin {  get { return _bochin; } set { _bochin = value; } }

    public void BochinStopped()
    {

    }
}
