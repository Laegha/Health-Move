using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    string _loadingMinigame;
    public static Action loadMinigame = delegate { };
    private void Start()
    {
        loadMinigame = LoadMinigame;
    }

    public void SetMinigame(string minigame) => _loadingMinigame = minigame;

    public void DisplayProfileManagerMenu()
    {
        foreach (var button in FindObjectsOfType<PsmoveButton>())
        {
            button.isInteractable = false;
        }
        ProfileManager.pm.CreateMenu(_loadingMinigame);
        FindObjectOfType<HandMovement>().transform.SetAsLastSibling();
    }

    public void SetMinigameMangager() => GameManager.gm.StartMinigame(_loadingMinigame + "Manager");

    public void LoadMinigame() => SceneManager.LoadScene(_loadingMinigame);

}