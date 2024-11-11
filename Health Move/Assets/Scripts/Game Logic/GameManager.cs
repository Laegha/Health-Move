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

    Color _currPlayerColor = Color.blue;

    Action generatedHands = delegate { };

    public MinigameManager CurrMinigameManager { get { return _currMinigameManager; } set { _currMinigameManager = value; } }
    public GameObject ActiveHand { get { return _activeHand; } set { _activeHand = value; } }
    public Color CurrPlayerColor { get { return _currPlayerColor; } set { _currPlayerColor = value; } }
    public Action GeneratedHands {  get { return generatedHands; } set { generatedHands = value; } }

    private void Start()
    {
        SceneManager.sceneLoaded += MinigameStart;
    }

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
        Type type = Type.GetType(minigameManagerType, true, true);
        CurrMinigameManager = (MinigameManager) Activator.CreateInstance(type);
        print(CurrMinigameManager);
    }

    public void UpdateHandsRotation()
    {
        if (ActiveHand == null)
            return;

        HandRotation handRotation = ActiveHand.GetComponent<HandRotation>();
        if(handRotation != null)
            handRotation.RotationUpdate();
        
        CursorMovement cursorMovement = ActiveHand.GetComponent<CursorMovement>();
        if(cursorMovement != null)
            cursorMovement.CursorUpdate();

    }

    public void UpdateHandsPosition()
    {
        if(ActiveHand == null) 
            return;
        
        HandMovement handMovement = ActiveHand.GetComponent<HandMovement>();
        if (handMovement == null)
            return;

        handMovement.MovementUpdate();
        
    }

    public IEnumerator KillControllerTracking(Action onComplete)
    {
        if(ActiveHand != null) 
            Destroy(ActiveHand);

        StartCoroutine(ControllersManager.controllersManager.KillTracking());

        while (!ControllersManager.controllersManager.Tracking) { print("Waiting for killtracking"); yield return null; }

        //display controller calibration screen
        onComplete?.Invoke();
    }

    public void GetProfileSensitivity(Profile profile, Action callback)
    {
        SensitivityCalibrationScreen sensitivityCalibrationScreen = FindObjectOfType<SensitivityCalibrationScreen>();
        sensitivityCalibrationScreen.SetProfileSensitivity(profile, callback);
    }

    public void ResetHands()
    {
        if(ActiveHand != null)
            Destroy(ActiveHand);
        
        //display position calibration screen
        PositionCalibrationScreen positionCalibrationScreen = FindObjectOfType<PositionCalibrationScreen>();
        StartCoroutine(positionCalibrationScreen.WaitForInput());
    }

    public void GenerateHands()
    {
        List<NeedsPlayerReference> needsPlayerReferences = FindObjectsOfType<NeedsPlayerReference>().ToList();
        
        ActiveHand = Instantiate(CurrMinigameManager != null ? CurrMinigameManager.minigameHandPrefab : _cursorPrefab, transform.position, Quaternion.identity);
        
        
        
        if (CurrMinigameManager == null)
        {
            ActiveHand.transform.SetParent(GameObject.Find("Canvas").transform);
            ActiveHand.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        
        else
        {
            PlayerIdentifier playerIdentifier = ActiveHand.GetComponent<PlayerIdentifier>();
            playerIdentifier.playerTeam = CurrMinigameManager.currPlayerProfile.teamName;
            playerIdentifier.Profile = CurrMinigameManager.currPlayerProfile;

            ActiveHand.transform.position = GameObject.Find("HandSpawner").transform.position;
            foreach (var renderer in playerIdentifier.BraceletRenderers)
            {
                renderer.material = new Material(renderer.material);
                renderer.material.color = _currPlayerColor;
            }

        }

        needsPlayerReferences.ForEach(x => x.player = ActiveHand);

        generatedHands?.Invoke();
    }

    public void ChangePlayer(Color playerColor)
    {
        _currPlayerColor = playerColor;
    }

    public void RoutineRunner(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public Transform FindInChildren(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            // Búsqueda recursiva en los hijos del hijo
            Transform result = FindInChildren(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    void MinigameStart(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (_currMinigameManager == null)
            return;

        _currMinigameManager.Start();
    }
}
