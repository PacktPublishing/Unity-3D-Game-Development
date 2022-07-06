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

    public Color idleColor = new Color(1f, 0f, 0f, 0.5f);
    public Color occupiedColor = new Color(1f, 1f, 0f, 0.5f);
    public Color interactColor = new Color(0f, 1f, 0f, 0.5f);

    public bool disableOnInteract = true;

    public UnityEvent OnInteract;

    bool interactPressed = false;

    Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        interactPressed = interactInput.action.ReadValue<float>() > 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            mat.SetColor("_BaseColor", occupiedColor);
        }
    }

    void OnTriggerStay(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            if (interactPressed)
            {
                mat.SetColor("_BaseColor", interactColor);
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
            mat.SetColor("_BaseColor", idleColor);
        }
    }
}
