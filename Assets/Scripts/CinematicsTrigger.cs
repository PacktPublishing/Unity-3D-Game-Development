using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CinematicsTrigger : MonoBehaviour
{
    [SerializeField]
    private CinematicManager cinematicManager;

    void OnTriggerEnter(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            player.GetComponent<CharacterController>().enabled = false;
            cinematicManager.PlayFirstBlush();
        }
    }
}
