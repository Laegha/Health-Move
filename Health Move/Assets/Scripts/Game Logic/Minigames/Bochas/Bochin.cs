using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bochin : MonoBehaviour
{
    public Transform justThrownBocha;
    [SerializeField] float pushForce;

    void Update()
    {
        if(justThrownBocha == null)
            return;
        Vector3 pushDirection = (justThrownBocha.position - transform.position).normalized;
        justThrownBocha.GetComponent<Rigidbody>().AddForce(new Vector3(pushDirection.x, 0, pushDirection.z) * Time.deltaTime * pushForce, ForceMode.Acceleration);
    }
}
