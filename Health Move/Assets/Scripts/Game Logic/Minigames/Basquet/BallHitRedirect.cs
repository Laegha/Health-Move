using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitRedirect : MonoBehaviour
{
    [SerializeField] Transform redirectPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("BasquetBall"))
            return;

        Rigidbody rb = collision.collider.attachedRigidbody;

        rb.velocity = Vector3.zero;
        rb.AddForce((redirectPoint.position - rb.position).normalized, ForceMode.Impulse);
    }
}
