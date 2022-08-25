using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransitionVolume : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    void OnTriggerEnter(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
            cam.Priority += 100;
        }
    }

    void OnTriggerExit(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            cam.Priority -= 100;
        }
    }
}
