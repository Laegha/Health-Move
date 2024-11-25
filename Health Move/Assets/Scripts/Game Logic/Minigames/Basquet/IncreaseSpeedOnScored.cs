using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeedOnScored : PointScoreReciever
{
    CrowdMovementAnim _crowdMovementAnim;
    float _speedMultiplier = 2;
    float _increasedSpeedTime = 2.2f; //this is the time that lasts the goal screen animation
    void Start()
    {
        _crowdMovementAnim = GetComponent<CrowdMovementAnim>();
    }

    public override void OnScored(MinigameManager minigameManager)
    {
        base.OnScored(minigameManager);
        StartCoroutine(IncreaseSpeed());
    }

    IEnumerator IncreaseSpeed()
    {
        _crowdMovementAnim.speed *= _speedMultiplier;
        yield return new WaitForSeconds(_increasedSpeedTime);
        _crowdMovementAnim.speed /= _speedMultiplier;

    }
}
