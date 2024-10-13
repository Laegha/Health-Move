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

    public Action minigameStartEvent;

    public MinigameManager CurrMinigameManager { get { return _currMinigameManager; } set { _currMinigameManager = value; } }
    public GameObject ActiveHand { get { return _activeHand; } set { _activeHand = value; } }
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
        minigameStartEvent = null;
    }

    public void StartMinigame(string minigameManagerType)
    {
        Type type = Type.GetType(minigameManagerType, false, true);
        CurrMinigameManager = (MinigameManager) Activator.CreateInstance(type);
    }

    public void UpdateHandsRotation()
    {
        HandRotation handRotation = ActiveHand.GetComponent<HandRotation>();
        if(handRotation != null)
            handRotation.RotationUpdate();
        
        CursorMovement cursorMovement = ActiveHand.GetComponent<CursorMovement>();
        if(cursorMovement != null)
            cursorMovement.CursorUpdate();

    }

    public void UpdateHandsPosition()
    {
        ActiveHand.GetComponent<HandMovement>().MovementUpdate();
        
    }

    public void RecalibrateControllers()
    {
        if(ActiveHand != null) 
            Destroy(ActiveHand);

        StopCoroutine("UpdateHandler");
        StopCoroutine("UpdateTracker");
        ControllersManager.controllersManager.EmptyCamera();

        //display controller calibration screen
        ControllerCalibration.controllerCalibration.StartCalibration();
    }

    public void ResetHands()
    {
        if(ActiveHand != null)
            Destroy(ActiveHand);
        //display position calibration screen
        FindObjectOfType<PositionCalibrationScreen>().gameObject.SetActive(true);
        
    }

    public void GenerateHands()
    {
        List<NeedsPlayerReference> needsPlayerReferences = FindObjectsOfType<NeedsPlayerReference>().ToList();
        
        ActiveHand = Instantiate(CurrMinigameManager != null ? CurrMinigameManager.minigameHandPrefab : _cursorPrefab, transform.position, Quaternion.identity);
        
        PlayerIdentifier playerIdentifier = ActiveHand.GetComponent<PlayerIdentifier>();
        playerIdentifier.playerID = _currPlayerId;
        
        if (CurrMinigameManager == null)
        {
            ActiveHand.transform.SetParent(GameObject.Find("Canvas").transform);
            ActiveHand.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        
        else
        {
            ActiveHand.transform.position = GameObject.Find("HandSpawner").transform.position;
            foreach (var renderer in playerIdentifier.BraceletRenderers)
            {
                renderer.material = new Material(renderer.material);
                renderer.material.color = _currPlayerColor;
            }

        }

        needsPlayerReferences.ForEach(x => x.players.Add(ActiveHand));
        minigameStartEvent.Invoke();

    }

    public void ChangePlayer(Color playerColor, int playerId)
    {
        _currPlayerColor = playerColor;
        _currPlayerId = playerId;
    }

    public void AddTeams(Team[] teams)
    {
        CurrMinigameManager.teams = teams;
    }
}
