using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerScore : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionIdentifier player = other.GetComponent<PlayerCollisionIdentifier>();

        if (player != null)
            GameManager.gm.OnScored(player.PlayerIdentifier);
    }
}
