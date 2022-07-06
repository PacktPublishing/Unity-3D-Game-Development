using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaiseStairsEventTrigger : MonoBehaviour
{
    public InteractionTrigger leftTrigger;
    public InteractionTrigger rightTrigger;
    public GameObject stairsBlocker;

    public GameObject leftFire;
    public GameObject rightFire;

    public static UnityAction OnStairsRaised;

    bool leftTriggerFired = false;
    bool rightTriggerFired = false;

    bool stairsRaised = false;


    // Start is called before the first frame update
    void Awake()
    {
        leftTrigger.OnInteract.AddListener(OnLeftTriggerInteract);
        rightTrigger.OnInteract.AddListener(OnRightTriggerInteract);

        leftFire.SetActive(false);
        rightFire.SetActive(false);
    }

    void OnDestroy()
    {
        leftTrigger.OnInteract.RemoveListener(OnLeftTriggerInteract);
        rightTrigger.OnInteract.RemoveListener(OnRightTriggerInteract);
    }

    void OnLeftTriggerInteract()
    {
        leftTriggerFired = true;
        leftFire.SetActive(true);
        if (rightTriggerFired)
        {
            stairsRaised = true;
            OnStairsRaised?.Invoke();
            stairsBlocker.SetActive(false);
            Debug.Log("RAISE STAIRS HERE");
        }
    }

    void OnRightTriggerInteract()
    {
        rightTriggerFired = true;
        rightFire.SetActive(true);
        if (leftTriggerFired)
        {
            stairsRaised = true;
            OnStairsRaised?.Invoke();
            stairsBlocker.SetActive(false);
            Debug.Log("RAISE STAIRS HERE");
        }
    }
}
