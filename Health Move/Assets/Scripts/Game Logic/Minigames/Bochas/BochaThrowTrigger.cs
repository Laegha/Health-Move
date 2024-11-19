using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochaThrowTrigger : MonoBehaviour
{
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] Transform targetIfMissed;

    [HideInInspector] public Transform bocha;
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        if(playerCollisionIdentifier == null || bocha == null)
            return;

        BochasMinigameManager bochasMinigameManager = GameManager.gm.CurrMinigameManager as BochasMinigameManager;

        //apply force based on bochasMinigameManager.PlayerThrowingModes[playerCollisionIdentifier.PlayerIdentifier.Profile.name]
        Vector3 forceToApply = GetThrowDirection(other.transform, other.transform.forward) * Random.Range(.5f, 1.5f); //the random is set to add a little variation to throwing (i'm on a tight schedule, don't want to implement it properly)
        Bochin bochin = bocha.GetComponent<Bochin>();

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
        bocha = null;
    }

    Vector3 GetThrowDirection(Transform rayEmmiter, Vector3 direction)
    {
        if (Physics.Raycast(rayEmmiter.position, direction, Mathf.Infinity, raycastLayerMask))
            return direction;
        return (targetIfMissed.position - rayEmmiter.position).normalized;
    }

    
}
