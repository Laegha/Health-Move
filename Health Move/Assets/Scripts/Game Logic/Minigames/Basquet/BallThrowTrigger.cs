using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject ball;
    [SerializeField] float speedThreshold;
    [SerializeField] float throwForce;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionIdentifier player = other.GetComponent<PlayerCollisionIdentifier>();

        if(player == null || ball == null)
            return;

        if (player.PlayerIdentifier.ControllerData.accel.magnitude < speedThreshold)
            return;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = true;
        ball.GetComponent<Collider>().isTrigger = false;
        rb.AddForce(player.PlayerIdentifier.transform.forward * throwForce, ForceMode.Impulse);
        ball.transform.parent = null;
    }
}
