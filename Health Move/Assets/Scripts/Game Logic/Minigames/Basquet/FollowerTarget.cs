using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerTarget : NeedsPlayerReference
{
    void Update()
    {
        if (players[0] == null )
            return;

        Vector3 position = transform.position;
        position.x = players[0].transform.position.x;

        transform.position = position;
    }
}
