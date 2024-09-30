using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject ball;

    [SerializeField] float speedThreshold;
    [SerializeField] float throwForce;
    [SerializeField] Transform optimalHitPoint;

    [SerializeField] Transform playerFollowerTarget;
    [SerializeField] LayerMask raycastIncludeLayerMask;

    BallCounter ballCounter;
    
    private void Start()
    {
        ballCounter = FindObjectOfType<BallCounter>();    
    }
    
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

        ballCounter.BallThrown(ball);
        ball = null;
    }

    Vector3 CalculateThrowDirection(Transform player)
    {
        Vector3 throwDirection = playerFollowerTarget.position - player.position;
        if (!Physics.Raycast(player.position, throwDirection, ~raycastIncludeLayerMask))
        {
            print("Tiro acertado");
            return throwDirection;
        }
        
        Vector3 accurateDirection = optimalHitPoint.position - player.position;
        return accurateDirection;
    }
}
