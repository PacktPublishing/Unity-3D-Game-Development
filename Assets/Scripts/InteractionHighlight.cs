using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHighlight : MonoBehaviour
{
    public float maxDistance = 10f;
    public float height = 2f;
    public LayerMask rayMask;


    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, rayMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.gameObject.GetComponent<PhysicsPuzzlePiece>())
            {
                hit.collider.gameObject.GetComponent<PhysicsPuzzlePiece>().ActivateInteractiveParticle(true);
            }
        }
    }
}
