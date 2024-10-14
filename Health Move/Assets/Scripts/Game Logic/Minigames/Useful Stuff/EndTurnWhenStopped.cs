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

    public IEnumerator CheckSpeed()
    {
        while(true)
        {
            yield return null;

            if (rb.velocity.magnitude <= 0)
            {
                GameManager.gm.CurrMinigameManager.OnTurnEnded();
                Destroy(this);
            }
        }
    }
}
