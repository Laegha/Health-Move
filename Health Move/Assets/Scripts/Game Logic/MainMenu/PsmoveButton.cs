using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PsmoveButton : MonoBehaviour
{
    [HideInInspector] public bool isInteractable = true;

    [SerializeField] PsMoveAPI.ControllerHelper.PSMoveButton interactButton = PsMoveAPI.ControllerHelper.PSMoveButton.Cross;

    [SerializeField] UnityEvent onInteractedEvents;

    private void OnTriggerStay(Collider other)
    {
        if(!isInteractable)
            return;

            print("Triggered Button");
        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        if(playerCollisionIdentifier == null)
            return;

        if(playerCollisionIdentifier.PlayerIdentifier.ControllerData.pressedButtons == (interactButton | PsMoveAPI.ControllerHelper.PSMoveButton.Trigger))
        {
            onInteractedEvents.Invoke();
        }
    }
}
