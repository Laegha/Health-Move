using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTest : MonoBehaviour
{
    [SerializeField] Transform directionObject;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            rb.useGravity = true;
            rb.AddForce(directionObject.up * 10, ForceMode.Impulse);
            transform.parent = null;
        }
    }
}
