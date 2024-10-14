using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnWhenStopped : MonoBehaviour
{
    Rigidbody rb;
    bool checking = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    public IEnumerator CheckSpeed()
    {
        if (checking)
            yield break;

        checking = true;
        
        while(true)
        {
            yield return null;
            if(rb == null)
                yield break;
            if (rb.velocity.magnitude <= 0.2f)
            {
                GameManager.gm.CurrMinigameManager.OnTurnEnded();
                Destroy(this);
                yield break;
            }
        }
    }
}
