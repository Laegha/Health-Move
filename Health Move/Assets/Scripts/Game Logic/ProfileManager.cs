using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.SceneManagement;


public class ProfileManager : MonoBehaviour
{
    static ProfileManager instance;

    public static ProfileManager pm { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [SerializedDictionary("Minigame", "Teams")]
    [SerializeField] SerializedDictionary<string, Team[]> _teamsByMinigame;

    Dictionary<string, Profile> _profiles = new Dictionary<string, Profile>();
    
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == "MainMenu")
        {
            _profiles.Clear();
        }
        //Display UI
    }

    public void AddProfileToTeam(string name, string teamName)
    {
        _profiles.Add(name, new Profile(name, teamName));
        //Show on UI
    }
}
