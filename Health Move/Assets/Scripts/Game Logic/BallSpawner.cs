using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    public void SpawnPrefab(Transform hands)
    {
        Instantiate(ball);
    }
}
