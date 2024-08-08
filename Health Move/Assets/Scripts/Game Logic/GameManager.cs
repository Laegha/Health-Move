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
    List<PointScoreReciever> scoredRecievers = new List<PointScoreReciever>();
    MinigameManager currMinigameManager;

    public void OnScored() //Is called by elements on scene
    {
        foreach (PointScoreReciever reciever in scoredRecievers)
            reciever.OnScored();

        currMinigameManager.OnScored(new PlayerIdentifier());
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

    public void UpdateHandsRotation()
    {
        foreach (HandRotation hand in FindObjectsOfType<HandRotation>())
            hand.RotationUpdate();
    }

    public void UpdateHandsPosition()
    {
        //Add behaviour for menu cursors

        foreach(HandMovement hand in FindObjectsOfType<HandMovement>())
            hand.MovementUpdate();
        
    }
}
