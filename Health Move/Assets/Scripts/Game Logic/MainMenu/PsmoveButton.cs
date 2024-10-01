using PsMoveAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PsmoveButton : MonoBehaviour
{
    [HideInInspector] public bool isInteractable = true;

    [SerializeField] UnityEvent onInteractedEvents;

    private void OnTriggerStay(Collider other)
    {
        if(!isInteractable)
            return;

        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        
        if(playerCollisionIdentifier == null)
            return;

        if(playerCollisionIdentifier.PlayerIdentifier.ControllerData.pressedButtons != ((ControllerHelper.PSMoveButton.Up - ControllerHelper.PSMoveButton.Up) | ControllerHelper.PSMoveButton.Trigger))
        {
            onInteractedEvents.Invoke();
        }
    }
}
