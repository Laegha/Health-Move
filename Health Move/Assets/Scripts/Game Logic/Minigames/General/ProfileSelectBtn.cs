using System;
using TMPro;

public class ProfileSelectBtn : PsmoveButton
{
    string _profileName;
    Action<string> _callbackOnPressed;
    TextMeshProUGUI _text;

    public string ProfileName {  get { return _profileName; } set { _profileName = value; } }
    public Action<string> CallbackOnPressed { get { return _callbackOnPressed; } set { _callbackOnPressed = value; } }
    public TextMeshProUGUI Text { get { return _text; } set { _text = value; } }

    public void ButtonPressed()
    {
        _callbackOnPressed?.Invoke(ProfileName);
    }
}