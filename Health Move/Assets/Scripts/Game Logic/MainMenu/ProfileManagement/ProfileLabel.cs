using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _profileNameText;
    ProfileManagerMenu _belongingMenu;
    string _profileName;

    public string ProfileName {  get { return _profileName; } set { _profileName = value; } }
    public ProfileManagerMenu BelongingMenu {  get { return _belongingMenu; } set { _belongingMenu = value; } }

    public virtual void Initiate()
    {
        _profileNameText.text = ProfileName;   
    }
}
