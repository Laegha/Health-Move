using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowTest : MonoBehaviour
{
    [SerializeField] Transform directionObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //GetComponent<Rigidbody>().useGravity = true;
            transform.rotation = directionObject.transform.rotation;
            //GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);
        }
    }
}
