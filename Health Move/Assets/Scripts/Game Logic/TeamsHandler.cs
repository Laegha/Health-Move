using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsHandler : MonoBehaviour
{
    static TeamsHandler instance;

    public TeamsHandler tm { get { return instance; } }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializedDictionary("Minigame", "Teams")]
    [SerializeField] SerializedDictionary<string, Team[]> _teamsByMinigame;
}
