using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCounter : MonoBehaviour
{
    [SerializeField] int maxBalls;

    List<GameObject> balls = new List<GameObject>();

    public void BallThrown(GameObject ball)
    {
        balls.Add(ball);
        if (balls.Count > maxBalls)
        {
            Destroy(balls[0]);
            balls.RemoveAt(0);
        }
    }
}
