using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BochasProfileLabel : ProfileLabel
{
    [SerializeField] TextMeshProUGUI _setThrowingModeBtnText;
    BochasProfileManagerMenu _bochasProfileMenu;

    public override void Initiate()
    {
        base.Initiate();
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerThrowingModes.Add(Profile, BochasMinigameManager.BochasThrowingMode.Arrimador);
        _bochasProfileMenu = BelongingMenu as BochasProfileManagerMenu;
    }

    public void ChangeThrowingMode()
    {
        _bochasProfileMenu.SelectThrowingModeStart(Profile.name);
        _bochasProfileMenu.callbackOnModeChange = SetProfileThrowingMode;
    }

    public void SetProfileThrowingMode(BochasMinigameManager.BochasThrowingMode bochasThrowingMode)
    {
        (GameManager.gm.CurrMinigameManager as BochasMinigameManager).PlayerThrowingModes[Profile] = bochasThrowingMode;
        _setThrowingModeBtnText.text = bochasThrowingMode.ToString();
    }
}
