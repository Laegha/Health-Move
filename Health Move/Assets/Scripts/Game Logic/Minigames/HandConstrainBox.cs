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
        if (players[0] == null)
            return;
        Vector3 newPlayerPosition = players[0].transform.position;
        
        //x
        if(players[0].transform.position.x < min.position.x)
            newPlayerPosition.x = min.position.x;
       
        else if (players[0].transform.position.x > max.position.x)
            newPlayerPosition.x = max.position.x;
        
        //y
        if (players[0].transform.position.y < min.position.y)
            newPlayerPosition.y = min.position.y;
        
        else if (players[0].transform.position.y > max.position.y)
            newPlayerPosition.y = max.position.y;
       
        //z
        if (players[0].transform.position.z < min.position.z)
            newPlayerPosition.z = min.position.z;
        
        else if (players[0].transform.position.z > max.position.z)
            newPlayerPosition.z = max.position.z;

        if (players[0].transform.position != newPlayerPosition)
            players[0].transform.position = newPlayerPosition;
    }
}
