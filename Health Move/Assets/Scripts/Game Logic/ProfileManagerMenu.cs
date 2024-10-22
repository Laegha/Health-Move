using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManagerMenu : MonoBehaviour
{
    string composingName;
    public void AddProfile()
    {
        //display keyboard
        //
    }

    public void AddCharToName(string newChar)
    {
        composingName += newChar;
    }

    public void CancelAddProfile()
    {
        composingName = "";
    }
}
