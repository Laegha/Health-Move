using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PsMoveAPI;
using System.Linq;

public class BallGenerator : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    private void OnTriggerStay(Collider other)
    {
        PlayerIdentifier player = other.transform.root.GetComponent<PlayerIdentifier>();
        if (player == null)
            return;

        ControllerData controllerData = ControllersManager.controllersManager.Controllers[player.AssignedController];
        if (controllerData.pressedButtons == (ControllerHelper.PSMoveButton.Start | ControllerHelper.PSMoveButton.Trigger) && controllerData.pressedButtons != controllerData.prevPressedButtons)
        {
            Transform ballHolder = GameObject.Find("BallHolder").transform;
            GameObject ball = Instantiate(ballPrefab, ballHolder.position, Quaternion.identity);
            ball.transform.parent = ballHolder;
            FindObjectOfType<BallThrowTrigger>().ball = ball;
        }
    }

    private void Update()
    {
        //ControllerData controllerData = ControllersManager.controllersManager.Controllers[ControllersManager.controllersManager.Controllers.Keys.ToArray()[0]];
        //ControllerHelper.PSMoveButton requiredButtons = ControllerHelper.PSMoveButton.Start | ControllerHelper.PSMoveButton.Trigger;
        //if (controllerData.pressedButtons == requiredButtons && controllerData.pressedButtons != controllerData.prevPressedButtons)
        //{
        //    Transform ballHolder = GameObject.Find("BallHolder").transform;
        //    GameObject ball = Instantiate(ballPrefab, ballHolder.position, Quaternion.identity);
        //    ball.transform.parent = ballHolder;
        //    FindObjectOfType<BallThrowTrigger>().ball = ball;
        //}
    }
}
