using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerTarget : NeedsPlayerReference
{
    void Update()
    {
        if (player == null)
            return;

        Vector3 position = transform.position;
        position.x = player.transform.position.x;

        transform.position = position;
    }
}
