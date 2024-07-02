using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void SetMinigameMangager(string minigameMangagerType) => GameManager.gm.StartMinigame(minigameMangagerType);
    public void LoadMinigame(string scene) => SceneManager.LoadScene(scene);
}