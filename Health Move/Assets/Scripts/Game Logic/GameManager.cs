using PsMoveAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [SerializeField] GameObject _cursorPrefab;

    MinigameManager _currMinigameManager;

    GameObject _activeHand;

    Color _currPlayerColor;

    int _currPlayerId;

    public MinigameManager CurrMinigameManager { get { return _currMinigameManager; } set { _currMinigameManager = value; } }
    
    public Color CurrPlayerColor { get { return _currPlayerColor; } set { _currPlayerColor = value; } }

    public void OnScored(PlayerIdentifier scorer) //Is called by elements on scene
    {
        CurrMinigameManager.OnScored(scorer);

        List<PointScoreReciever> scoredRecievers = FindObjectsOfType<PointScoreReciever>().ToList();
        foreach (PointScoreReciever reciever in scoredRecievers)
            reciever.OnScored(CurrMinigameManager);
    }

    public void EndMinigame()
    {
        Debug.Log("Minigame Ended");
        SceneManager.LoadScene("MainMenu");
        CurrMinigameManager = null;
    }

    public void StartMinigame(string minigameManagerType)
    {
        Type type = Type.GetType(minigameManagerType, false, true);
        CurrMinigameManager = (MinigameManager) Activator.CreateInstance(type);
    }

    public void UpdateHandsRotation()
    {
        HandRotation handRotation = _activeHand.GetComponent<HandRotation>();
        if(handRotation != null)
            handRotation.RotationUpdate();
        
        CursorMovement cursorMovement = _activeHand.GetComponent<CursorMovement>();
        if(cursorMovement != null)
            cursorMovement.CursorUpdate();

    }

    public void UpdateHandsPosition()
    {
        _activeHand.GetComponent<HandMovement>().MovementUpdate();
        
    }

    public void RecalibrateControllers()
    {
        //remove current controller from ControllerCalibration.calibratedControllers

    }

    public void ResetHands()
    {
        Destroy(_activeHand);
        //display calibration screen
    }

    public void GenerateHands()
    {
        List<NeedsPlayerReference> needsPlayerReferences = FindObjectsOfType<NeedsPlayerReference>().ToList();
        
        _activeHand = Instantiate(CurrMinigameManager != null ? CurrMinigameManager.minigameHandPrefab : _cursorPrefab, transform.position, Quaternion.identity);
        
        PlayerIdentifier playerIdentifier = _activeHand.GetComponent<PlayerIdentifier>();
        playerIdentifier.playerID = _currPlayerId;
        
        if (CurrMinigameManager == null)
        {
            _activeHand.transform.SetParent(GameObject.Find("Canvas").transform);
            _activeHand.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        
        else
        {
            _activeHand.transform.position = GameObject.Find("HandSpawner").transform.position;
            foreach (var renderer in playerIdentifier.BraceletRenderers)
            {
                renderer.material = new Material(renderer.material);
                renderer.material.color = _currPlayerColor;
            }

        }

        needsPlayerReferences.ForEach(x => x.players.Add(_activeHand));

    }

    public void ChangePlayer(Color playerColor, int playerId)
    {
        _currPlayerColor = playerColor;
        _currPlayerId = playerId;
    }
}
