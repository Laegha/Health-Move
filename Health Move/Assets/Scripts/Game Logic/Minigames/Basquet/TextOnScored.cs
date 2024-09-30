using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnScored : PointScoreReciever
{
    [SerializeField] Animator animator;

    public override void OnScored(MinigameManager minigameManager)
    {
        base.OnScored(minigameManager);

        animator.Play("Scored");
    }
}
