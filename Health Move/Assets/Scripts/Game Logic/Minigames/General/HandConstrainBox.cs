using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandConstrainBox : NeedsPlayerReference
{
    public Vector3 handPushDir;

    [SerializeField] Transform min;
    [SerializeField] Transform max;

    private void Update()
    {
        if (player == null)
            return;
        Vector3 newPlayerPosition = player.transform.position;
        
        //x
        if(player.transform.position.x < min.position.x)
            newPlayerPosition.x = min.position.x;
       
        else if (player.transform.position.x > max.position.x)
            newPlayerPosition.x = max.position.x;
        
        //y
        if (player.transform.position.y < min.position.y)
            newPlayerPosition.y = min.position.y;
        
        else if (player.transform.position.y > max.position.y)
            newPlayerPosition.y = max.position.y;
       
        //z
        if (player.transform.position.z < min.position.z)
            newPlayerPosition.z = min.position.z;
        
        else if (player.transform.position.z > max.position.z)
            newPlayerPosition.z = max.position.z;

        if (player.transform.position != newPlayerPosition)
            player.transform.position = newPlayerPosition;
    }
}
