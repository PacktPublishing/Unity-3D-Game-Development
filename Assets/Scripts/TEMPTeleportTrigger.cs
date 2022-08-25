using UnityEngine;

/// <summary>
/// TEMPORARY until we get animations for crawling through the collapsed tunnel
/// </summary>
public class TEMPTeleportTrigger : MonoBehaviour
{
    public Transform teleportTargetPos;

    void OnTriggerEnter(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            Debug.Log("TELEPORTING PLAYER");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = teleportTargetPos.position;
            player.transform.rotation = teleportTargetPos.rotation;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
