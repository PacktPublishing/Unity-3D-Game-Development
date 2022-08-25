using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPuzzlePiece : MonoBehaviour
{
    public PhysicsPuzzlePieceType pieceType;

    [SerializeField]
    private GameObject interactionParticle;

    public void ActivateInteractiveParticle(bool enable)
    {
        interactionParticle.SetActive(enable);
    }
}
