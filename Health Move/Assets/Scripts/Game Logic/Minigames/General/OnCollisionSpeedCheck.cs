using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSpeedCheck : MonoBehaviour
{
    [SerializeField] string _collisionTag;
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag(_collisionTag))
            return;
        EndTurnWhenStopped endTurnWhenStopped = collision.collider.GetComponent<EndTurnWhenStopped>();

        StartCoroutine(endTurnWhenStopped.CheckSpeed());
    }
}
