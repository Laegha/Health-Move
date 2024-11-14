using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _profileNameText;
    ProfileManagerMenu _belongingMenu;
    Profile _profile;

    public Profile Profile {  get { return _profile; } set { _profile = value; } }
    public ProfileManagerMenu BelongingMenu {  get { return _belongingMenu; } set { _belongingMenu = value; } }

    public virtual void Initiate()
    {
        _profileNameText.text = Profile.name;   
    }
}
