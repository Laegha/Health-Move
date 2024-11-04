using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasProfileLabel : ProfileLabel
{
    public override void Start()
    {
        base.Start();
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerThrowingModes.Add(ProfileName, BochasPlayer.BochasThrowingMode.Arrimador);
    }

    public void SetProfileThrowingMode(BochasPlayer.BochasThrowingMode bochasThrowingMode)
    {
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerThrowingModes[ProfileName] = bochasThrowingMode;
    }
}
