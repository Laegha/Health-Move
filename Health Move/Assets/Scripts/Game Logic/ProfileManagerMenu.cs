using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManagerMenu : MonoBehaviour
{
    string composingName;
    string currAddingTeam;
    [SerializeField] string menuMinigame;
    [SerializeField] TextMeshProUGUI nameDisplay;

    [SerializeField] Vector2 profileBoxCellSize;
    [SerializeField] SerializedDictionary<string, GridLayoutGroup> profilesGrids;

    [SerializeField] GameObject nameCreateMenu;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject createdProfileBoxPrefab;

    public SerializedDictionary<string, GridLayoutGroup> ProfilesGrids { get { return profilesGrids; } }

    public GameObject CreatedProfileBoxPrefab { get { return createdProfileBoxPrefab; } }

    public void AddProfile(string team)
    {
        currAddingTeam = team;

        foreach(var grid in profilesGrids)
        {
            grid.Value.transform.parent.gameObject.SetActive(false);
        }
        nameCreateMenu.SetActive(true);
        playBtn.SetActive(false);

        nameDisplay.text = composingName;
    }

    public void AddCharToName(string newChar)
    {
        composingName += newChar;
        nameDisplay.text = composingName;
    }

    public void CancelAddProfile()
    {
        composingName = "";
        CloseNameCreateMenu();
    }

    public void ConfirmProfileCreation()
    {
        ProfileManager.pm.AddProfileToTeam(composingName, currAddingTeam);

        Vector2 newBoxSize = new Vector2(profileBoxCellSize.x, profileBoxCellSize.y / profilesGrids[currAddingTeam].transform.childCount);
        profilesGrids[currAddingTeam].cellSize = newBoxSize;
        profilesGrids[currAddingTeam].transform.Find("AddPlayerBtn").GetComponent<BoxCollider>().size = newBoxSize;
        Instantiate(createdProfileBoxPrefab, profilesGrids[currAddingTeam].transform).transform.Find("ProfileName").GetComponent<TextMeshProUGUI>().text = composingName;

        CloseNameCreateMenu();
    }

    void CloseNameCreateMenu()
    {
        foreach (var grid in profilesGrids)
        {
            grid.Value.transform.parent.gameObject.SetActive(true);
        }
        nameCreateMenu.SetActive(false);
        playBtn.SetActive(true);
    }

    public void LoadMinigame() 
    {
        foreach(var team in TeamsHandler.tm.teamsByMinigame[menuMinigame])
        {
            if(!ProfileManager.pm.Profiles.Where(x => x.teamName == team.teamName).Any())
                return;
        }
        MenuManager.loadMinigame();
    }
}
