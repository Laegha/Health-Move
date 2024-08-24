using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject ball;
    [SerializeField] float speedThreshold;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionIdentifier player = other.GetComponent<PlayerCollisionIdentifier>();

        if(player == null || ball == null)
            return;

        if (player.PlayerIdentifier.ControllerData.accel.magnitude < speedThreshold)
            return;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(other.transform.up * 10, ForceMode.Impulse);
        ball.transform.parent = null;
    }
}
