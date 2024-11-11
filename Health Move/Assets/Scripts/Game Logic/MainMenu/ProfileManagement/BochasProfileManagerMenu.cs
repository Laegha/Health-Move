using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasProfileManagerMenu : ProfileManagerMenu
{
    [SerializeField] GameObject _throwingModeSelectLayout;

    string _throwingModeSelectProfile;

    public Action<BochasPlayer.BochasThrowingMode> callbackOnModeChange = delegate(BochasPlayer.BochasThrowingMode mode) { };
    public void SelectThrowingModeStart(string changingProfile)
    {
        _throwingModeSelectProfile = changingProfile;
        _throwingModeSelectLayout.SetActive(true);
        defaultLayout.SetActive(false);
    }

    public void SelectThrowingModeEnd(string bochasThrowingMode)
    {
        callbackOnModeChange((BochasPlayer.BochasThrowingMode) Enum.Parse(typeof(BochasPlayer.BochasThrowingMode), bochasThrowingMode));
        callbackOnModeChange = delegate (BochasPlayer.BochasThrowingMode mode) { };
        _throwingModeSelectLayout.SetActive(false);
        defaultLayout.SetActive(true);
    }
}