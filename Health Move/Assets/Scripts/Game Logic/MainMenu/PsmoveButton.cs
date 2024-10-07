using PsMoveAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PsmoveButton : MonoBehaviour
{
    [HideInInspector] public bool isInteractable = true;

    [SerializeField] UnityEvent onInteractedEvents;

    float fillAmount = 0;

    [SerializeField] float speed;

    private void OnTriggerStay(Collider other)
    {
        if(!isInteractable)
            return;

        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        
        if(playerCollisionIdentifier == null)
            return;

        fillAmount += Time.deltaTime * speed;
        other.GetComponentInChildren<Image>().fillAmount = fillAmount;

        if(fillAmount >= 1)
        {
            onInteractedEvents.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInteractable)
            return;

        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();

        if (playerCollisionIdentifier == null)
            return;

        fillAmount = 0;
        other.GetComponentInChildren<Image>().fillAmount = fillAmount;
    }
}
