using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasProfileManagerMenu : ProfileManagerMenu
{
    [SerializeField] GameObject _throwingModeSelectLayout;

    string _throwingModeSelectProfile;

    public Action<BochasMinigameManager.BochasThrowingMode> callbackOnModeChange = delegate(BochasMinigameManager.BochasThrowingMode mode) { };
    public void SelectThrowingModeStart(string changingProfile)
    {
        _throwingModeSelectProfile = changingProfile;
        _throwingModeSelectLayout.SetActive(true);
        defaultLayout.SetActive(false);
    }

    public void SelectThrowingModeEnd(string bochasThrowingMode)
    {
        callbackOnModeChange((BochasMinigameManager.BochasThrowingMode) Enum.Parse(typeof(BochasMinigameManager.BochasThrowingMode), bochasThrowingMode));
        callbackOnModeChange = delegate (BochasMinigameManager.BochasThrowingMode mode) { };
        _throwingModeSelectLayout.SetActive(false);
        defaultLayout.SetActive(true);
    }
}