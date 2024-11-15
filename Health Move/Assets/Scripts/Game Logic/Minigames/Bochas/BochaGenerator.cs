using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BochaGenerator : MonoBehaviour
{
    [SerializeField] GameObject _bochinPrefab;
    [SerializeField] GameObject _bochaPrefab;

    BochaThrowTrigger _bochaThrowTrigger;
    
    private void Start()
    {
        _bochaThrowTrigger = FindObjectOfType<BochaThrowTrigger>();
    }
    public void GenerateBocha(string bochaToGenerate)
    {
        Transform ballHolder = GameObject.Find("BallHolder").transform;
        GameObject bocha;
        
        if(bochaToGenerate == "bochin")
            bocha = Instantiate(_bochinPrefab, ballHolder.position, Quaternion.identity);
        else
        {
            bocha = Instantiate(_bochaPrefab, ballHolder.position, Quaternion.identity);
            bocha.GetComponent<Bocha>().bochaTeam = GameManager.gm.CurrMinigameManager.currPlayerProfile.teamName;

            Renderer renderer = bocha.GetComponent<Renderer>();
            renderer.material = new Material(renderer.material);
            renderer.material.color = TeamsHandler.tm.teamsByMinigame["BochasMinigame"].Where(x => x.teamName == bochaToGenerate).ToList()[0].teamColor;
        }
        bocha.transform.parent = ballHolder;
        _bochaThrowTrigger.bocha = bocha.transform;
    }
}
