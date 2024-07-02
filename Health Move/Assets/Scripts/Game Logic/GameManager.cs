using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager gm
    {
        get { return instance;}
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    List<PointScoredReciever> scoredRecievers = new List<PointScoredReciever>();
    MinigameManager currMinigameManager;

    public void OnScored() //Is called by elements on scene
    {
        foreach (PointScoredReciever reciever in scoredRecievers)
            reciever.OnScored();

        currMinigameManager.OnScored();
        //if(currMinigameManager.hasEnded)
        //end minigame
    }

    public void EndMinigame()
    {
        Debug.Log("Minigame Ended");
    }

    public void StartMinigame(string minigameManagerType)
    {
        Type type = Type.GetType(minigameManagerType);
        currMinigameManager = (MinigameManager) Activator.CreateInstance(type);
    }
}
