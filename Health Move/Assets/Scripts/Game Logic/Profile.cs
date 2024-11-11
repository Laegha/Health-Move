using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile
{
    public static float sensitivityStandard;
    public string name;
    public string teamName;
    public float sensitivity;
    public bool calibrated = false;

    public Profile(string name, string teamName)
    {
        this.name = name;
        this.teamName = teamName;
    }
}
