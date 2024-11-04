using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BochasProfileLabel : ProfileLabel
{
    [SerializeField] TextMeshProUGUI _setThrowingModeBtnText;
    public override void Start()
    {
        base.Start();
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerThrowingModes.Add(ProfileName, BochasPlayer.BochasThrowingMode.Arrimador);
    }

    public void SetProfileThrowingMode(BochasPlayer.BochasThrowingMode bochasThrowingMode)
    {
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerThrowingModes[ProfileName] = bochasThrowingMode;
        _setThrowingModeBtnText.text = bochasThrowingMode.ToString();
    }
}
