using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParentWithoutChangingRelativePosition : MonoBehaviour
{
    Vector3 localPosition;

    void Start()
    {
        localPosition = transform.localPosition;
    }
    void Update()
    {
        transform.localPosition = localPosition;
    }
}
