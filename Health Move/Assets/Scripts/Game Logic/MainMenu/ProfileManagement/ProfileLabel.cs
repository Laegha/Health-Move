using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _profileNameText;
    string _profileName;

    public string ProfileName {  get { return _profileNameText.text; } set { _profileName = value; } }

    public virtual void Start()
    {
        _profileNameText.text = ProfileName;   
    }
}
