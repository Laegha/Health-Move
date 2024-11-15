using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochaThrowTrigger : MonoBehaviour
{
    [SerializeField] float arrimadorForceMultiplier;
    [SerializeField] float bochadorForceMultiplier;

    [HideInInspector] public Transform bocha;
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        if(playerCollisionIdentifier == null || bocha == null)
            return;

        BochasMinigameManager bochasMinigameManager = GameManager.gm.CurrMinigameManager as BochasMinigameManager;

        //apply force based on bochasMinigameManager.PlayerThrowingModes[playerCollisionIdentifier.PlayerIdentifier.Profile.name]
        BochasMinigameManager.BochasThrowingMode bochaThrowingMode = bochasMinigameManager.PlayerThrowingModes[playerCollisionIdentifier.PlayerIdentifier.Profile.name];
        Vector3 forceToApply = other.transform.forward * Random.Range(.5f, 1.5f); //the random is set to add a little variation to throwing (i'm on a tight schedule, don't want to implement it properly)
        Bochin bochin = bocha.GetComponent<Bochin>();

        if (bochaThrowingMode == BochasMinigameManager.BochasThrowingMode.Arrimador || bochin != null)
            forceToApply *= arrimadorForceMultiplier;
        else
            forceToApply *= bochadorForceMultiplier;

        //push bocha
        Rigidbody rb = bocha.GetComponent<Rigidbody>();
        rb.AddForce(forceToApply, ForceMode.Impulse);
        rb.useGravity = true;
        bocha.GetComponent<Collider>().isTrigger = false;
        bocha.transform.parent = null;

        if (bochin == null)
        {
            bochasMinigameManager.ThrownBocha(bocha);
        }
        StartCoroutine(ThrownBocha(bocha.GetComponent<EndTurnWhenStopped>()));
        bocha = null;
    }

    IEnumerator ThrownBocha(EndTurnWhenStopped endTurnWhenStopped)
    {
        yield return new WaitForEndOfFrame();
        
        StartCoroutine(endTurnWhenStopped.CheckSpeed());
    }
}
