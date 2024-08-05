using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerScore : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PointScorer"))
            GameManager.gm.OnScored();
    }
}
