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
        
        while(checking)
        {
            yield return null;
            if(rb == null)
                yield break;
            if (rb.velocity.magnitude <= 0.2f)
            {
                rb.velocity = Vector3.zero;
                GameManager.gm.CurrMinigameManager.OnTurnEnded();
                checking = false;
                Destroy(this);
                yield break;
            }
        }
    }
}
