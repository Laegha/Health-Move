using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTrigger : MonoBehaviour
{
    /*[HideInInspector]*/ public GameObject ball;
    [SerializeField] float speedThreshold;
    [SerializeField] float throwForce;
    [SerializeField] Transform optimalHitPoint;
    [SerializeField] LayerMask layerMask;

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

        rb.AddForce(CalculateThrowDirection(player.transform) * throwForce, ForceMode.Impulse);

        ball.transform.parent = null;
    }

    Vector3 CalculateThrowDirection(Transform player)
    {
        if(!Physics.Raycast(player.position, player.forward, layerMask))
            return player.forward;
        
        Vector3 direction = player.position - optimalHitPoint.position;
        return direction;
    }
}
