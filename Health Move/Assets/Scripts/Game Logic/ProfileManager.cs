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
    [SerializeField] SerializedDictionary<string, GameObject> _profileManagerMenusByMinigame;

    List<Profile> _profiles = new List<Profile>();

    ProfileManagerMenu _activeMenu;

    public SerializedDictionary<string, GameObject> ProfileManagerMenusByMinigame { get { return _profileManagerMenusByMinigame;} }
    public List<Profile> Profiles { get { return _profiles; } }
    
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
    }

    public void CreateMenu(string loadingMinigame)
    {
        _activeMenu = Instantiate(ProfileManagerMenusByMinigame[loadingMinigame], GameObject.Find("Canvas").transform).GetComponent<ProfileManagerMenu>();
    }

    public void AddProfileToTeam(string name, string teamName)
    {
        _profiles.Add(new Profile(name, teamName));
    }

    public string GetProfileTextFromKey(string key)
    {
        if (key[0] == ':')
            return "";
        else
        {
            int cutPosition = key.IndexOf(".");
            if (cutPosition > 0)
                return key.Substring(0, cutPosition);
            else
                return key;

        }
    }
}
