using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnWhenStopped : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        if(rb.velocity.magnitude <= 0)
        {
            GameManager.gm.CurrMinigameManager.OnTurnEnded();
            Destroy(this);
        }
    }
}
