using PsMoveAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class PsmoveButton : MonoBehaviour
{
    [HideInInspector] public bool isInteractable = true;

    public UnityEvent onInteractedEvents;

    float fillAmount = 0;

    [SerializeField] float timeToInteract;

    private void OnTriggerStay(Collider other)
    {
        if(!isInteractable)
            return;

        PlayerCollisionIdentifier playerCollisionIdentifier = other.GetComponent<PlayerCollisionIdentifier>();
        
        if(playerCollisionIdentifier == null)
            return;

        fillAmount += Time.deltaTime / timeToInteract;
        other.transform.parent.Find("Elipse").GetComponent<Image>().fillAmount = fillAmount;

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
        other.transform.parent.Find("Elipse").GetComponent<Image>().fillAmount = fillAmount;
    }
}
