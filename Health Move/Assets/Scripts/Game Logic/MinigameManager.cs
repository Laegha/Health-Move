using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager
{
    public bool hasEnded = false;

    public virtual void OnScored() { }
    public virtual void OnRecieved() { }
}
