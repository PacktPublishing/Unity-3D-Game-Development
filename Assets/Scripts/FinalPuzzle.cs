using UnityEngine;

public class FinalPuzzle : MonoBehaviour
{
    [SerializeField]
    CinematicManager cinematicManager;

    [SerializeField]
    Animator introPuzleAnchorAnimator;

    [SerializeField]
    Animator introPuzzlePieceAnimator;

    int numPiecesSlotted = 0;

    public AudioSource introToFinalSFX;
    public AudioSource partialFinalSFX;
    public AudioSource FullFinalSFX;

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
            cinematicManager.PlayFirstTK();
            introPuzzlePieceAnimator.SetBool("isActive", true);
            introPuzleAnchorAnimator.SetBool("isActive", true);
            //SOUND
            introToFinalSFX.Play();
        }
        else
        {
            numPiecesSlotted += 1;
            //Puzzle Piece is Slotted

            partialFinalSFX.Play();

            if (numPiecesSlotted >= 3)
            {
                Debug.Log("FINAL PUZZLE SOLVED! Trigger portal event");
                cinematicManager.PlayCrownMe();
                //SOUND
                FullFinalSFX.Play();
            }
        }
    }
}
