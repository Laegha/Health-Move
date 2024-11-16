using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _profileNameText;
    ProfileManagerMenu _belongingMenu;
    string _profileName = "";

    public string ProfileName {  get { return _profileName; } set { _profileName = value; } }
    public ProfileManagerMenu BelongingMenu {  get { return _belongingMenu; } set { _belongingMenu = value; } }

    public virtual void Initiate()
    {
        if (ProfileName[0] == ':')
            _profileNameText.text = "";
        else
        {
            int cutPosition = ProfileName.IndexOf(".");
            if(cutPosition > 0)
                _profileNameText.text = ProfileName.Substring(0, cutPosition);   
            else
                _profileNameText.text = ProfileName;

        }

    }
}
