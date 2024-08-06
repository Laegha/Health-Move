using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles logic of a minigame
public class MinigameManager
{
    public bool hasEnded = false;

    public virtual void OnScored(PlayerIdentifier scorer) { } //Is called by GM OnScored
    public virtual void OnRecieved() { }
}
