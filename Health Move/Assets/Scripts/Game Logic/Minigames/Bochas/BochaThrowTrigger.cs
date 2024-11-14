using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochaThrowTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        if(playerCollisionIdentifier == null)
            return;

        BochasMinigameManager bochasMinigameManager = GameManager.gm.CurrMinigameManager as BochasMinigameManager;
        bochasMinigameManager.ThrownBocha(other.transform);

        //apply force based on bochasMinigameManager.PlayerThrowingModes[playerCollisionIdentifier.PlayerIdentifier.Profile.name]
    }
}
