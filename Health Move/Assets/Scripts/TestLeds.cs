using PsMoveAPI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestLeds : MonoBehaviour
{
    [SerializeField] Color color;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            //ControllersManager.controllersManager.SetLeds(ControllersManager.controllersManager.Controller.Keys.ToList()[0], (int)color.r * 255, (int)color.g * 255, (int)color.b * 255);
        }
    }
}
