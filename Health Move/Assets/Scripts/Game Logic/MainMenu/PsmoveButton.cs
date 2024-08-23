using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PsmoveButton : MonoBehaviour
{
    public bool isInteractable;

    [SerializeField] PsMoveAPI.ControllerHelper.PSMoveButton interactButton = PsMoveAPI.ControllerHelper.PSMoveButton.Cross;

    [SerializeField] UnityEvent onInteractedEvents;

    private void OnTriggerStay(Collider other)
    {
        if(!isInteractable)
            return;

        PlayerIdentifier playerIdentifier = other.transform.root.GetComponent<PlayerIdentifier>();
        if(playerIdentifier == null)
            return;

        if(playerIdentifier.ControllerData.pressedButtons == interactButton)
            onInteractedEvents.Invoke();
    }
}
