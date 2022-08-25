using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionTrigger : MonoBehaviour
{
    /// <summary>
    /// Reference to the Interact button input
    /// </summary>
    public InputActionReference interactInput;

    public GameObject interactionParticle;

    public bool disableOnInteract = true;

    public UnityEvent OnInteract;

    bool interactPressed = false;

    void Update()
    {
        interactPressed = interactInput.action.ReadValue<float>() > 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            interactionParticle.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            if (interactPressed)
            {
                interactionParticle.SetActive(false);
                OnInteract?.Invoke();

                Debug.Log($"Interacted with {gameObject.name}");

                if (disableOnInteract)
                {
                    this.enabled = false;
                    this.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            interactionParticle.SetActive(false);
        }
    }
}
