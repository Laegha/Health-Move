using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandConstrainBox : MonoBehaviour
{
    public Vector3 handPushDir;

    [SerializeField] Transform min;
    [SerializeField] Transform max;

    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        print("Collided with ");
        Bounds thisBounds = GetComponent<Collider>().bounds;
        Vector3 newHandPosition = other.transform.position;

        if(handPushDir == Vector3.right)
        {
            float otherLenght = other.bounds.center.x - other.bounds.min.x;
            newHandPosition.x = thisBounds.max.x + otherLenght;
        }
        else if(handPushDir == Vector3.left)
        {
            float otherLenght = other.bounds.center.x - other.bounds.min.x;
            newHandPosition.x = thisBounds.min.x - otherLenght;
        }
        else if (handPushDir == Vector3.up)
        {
            float otherLenght = other.bounds.center.y - other.bounds.min.y;
            newHandPosition.y = thisBounds.max.y + otherLenght;
        }
        else if (handPushDir == Vector3.down)
        {
            float otherLenght = other.bounds.center.y - other.bounds.min.y;
            newHandPosition.y = thisBounds.min.y - otherLenght;
        }
        else if (handPushDir == Vector3.forward)
        {
            float otherLenght = other.bounds.center.z - other.bounds.min.z;
            newHandPosition.z = thisBounds.max.z + otherLenght;
        }
        else if (handPushDir == Vector3.back)
        {
            float otherLenght = other.bounds.center.z - other.bounds.min.z;
            newHandPosition.z = thisBounds.min.z - otherLenght;
        }
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //other.transform.position = newHandPosition;
    }

    private void Update()
    {
        Transform player = FindObjectOfType<PlayerIdentifier>().transform;
        Vector3 newPlayerPosition = player.position;
        
        //x
        if(player.position.x < min.position.x)
            newPlayerPosition.x = min.position.x;
       
        else if (player.position.x > max.position.x)
            newPlayerPosition.x = max.position.x;
        
        //y
        if (player.position.y < min.position.y)
            newPlayerPosition.y = min.position.y;
        
        else if (player.position.y > max.position.y)
            newPlayerPosition.y = max.position.y;
       
        //z
        if (player.position.z < min.position.z)
            newPlayerPosition.z = min.position.z;
        
        else if (player.position.z > max.position.z)
            newPlayerPosition.z = max.position.z;

        if(player.position != newPlayerPosition)
            player.position = newPlayerPosition;
    }
}
