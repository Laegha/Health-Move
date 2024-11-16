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
    [SerializeField] GameObject createdProfileBoxPrefab;

    public GameObject defaultLayout;
    [SerializeField] GameObject nameCreateLayout;

    public SerializedDictionary<string, GridLayoutGroup> ProfilesGrids { get { return profilesGrids; } }

    public GameObject CreatedProfileBoxPrefab { get { return createdProfileBoxPrefab; } }

    public void AddProfile(string team)
    {
        currAddingTeam = team;
        composingName = "";

        defaultLayout.SetActive(false);
        nameCreateLayout.SetActive(true);

        nameDisplay.text = composingName;
    }

    public void AddCharToName(string newChar)
    {
        composingName += newChar;
        nameDisplay.text = composingName;
    }

    public void CancelAddProfile()
    {
        CloseNameCreateMenu();
    }

    public void ConfirmProfileCreation()
    {
        if (composingName == "")
            composingName = ":";
        int sameNameProfiles = ProfileManager.pm.Profiles.Where(x => x.name.Contains(composingName)).Count();
        if (sameNameProfiles > 0)
            composingName += "." + sameNameProfiles;
        ProfileManager.pm.AddProfileToTeam(composingName, currAddingTeam);

        ProfileLabel profileLabel = Instantiate(createdProfileBoxPrefab, profilesGrids[currAddingTeam].transform).GetComponent<ProfileLabel>();
        profileLabel.ProfileName = composingName;
        profileLabel.Initiate();

        Vector2 newBoxSize = new Vector2(profileBoxCellSize.x, profileBoxCellSize.y / profilesGrids[currAddingTeam].transform.childCount);
        profilesGrids[currAddingTeam].cellSize = newBoxSize;
        profilesGrids[currAddingTeam].transform.GetChild(0).GetComponent<BoxCollider>().size = newBoxSize;

        CloseNameCreateMenu();
    }

    void CloseNameCreateMenu()
    {
        composingName = "";
        defaultLayout.SetActive(true);
        nameCreateLayout.SetActive(false);
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
