using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzle : MonoBehaviour
{
    /// <summary>
    /// TEMPORARY until we have environment animations for water/bridge
    /// </summary>
    public GameObject tempBridge;

    int numPiecesSlotted = 0;


    // Start is called before the first frame update
    void Start()
    {
        numPiecesSlotted = 0;
        PhysicsPuzzleTrigger.OnPieceSlotted += OnPieceSlotted;
    }

    void OnDestroy()
    {
        PhysicsPuzzleTrigger.OnPieceSlotted -= OnPieceSlotted;
    }

    void OnPieceSlotted(PhysicsPuzzleTrigger trigger, PhysicsPuzzlePiece piece)
    {
        if (piece.pieceType == PhysicsPuzzlePieceType.Intro)
        {
            Debug.Log("FINAL PUZZLE INTRO SOLVED. Trigger environment transition here");
            tempBridge.SetActive(true);
            //SOUND BOY
        }
        else
        {
            numPiecesSlotted += 1;
            //Puzzle Piece is Slotted
            if (numPiecesSlotted >= 3)
            {
                Debug.Log("FINAL PUZZLE SOLVED! Trigger portal event");
                //SOUND BOY
            }
        }
    }
}
