using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParentKeepingPosition : MonoBehaviour
{
    [SerializeField] Transform target;
    Quaternion rotation;
    float verticalDistance;

    void Start()
    {
        verticalDistance = transform.localPosition.y;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y + verticalDistance, target.position.z);
        transform.rotation = rotation;
    }
}
