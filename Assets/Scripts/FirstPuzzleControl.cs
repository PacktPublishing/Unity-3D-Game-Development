using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player trigger volume and input control for the first puzzle
/// </summary>
public class FirstPuzzleControl : MonoBehaviour
{
    /// <summary>
    /// Reference to the Interact button input
    /// </summary>
    public InputActionReference interactInput;

    /// <summary>
    /// Was the Interact button held this frame?
    /// </summary>
    bool interactHeld = false;

    /// <summary>
    /// Cached reference to the main puzzle controller
    /// </summary>
    FirstPuzzle puzzle;

    /// <summary>
    /// Red (unoccupied) color for the trigger volumes
    /// </summary>
    Color redTrigger = new Color(1f, 0f, 0f, 0.5f);

    /// <summary>
    /// Yellow (occupied) color for the trigger volumes
    /// </summary>
    Color yellowTrigger = new Color(1f, 1f, 0f, 0.5f);

    private MyvariThirdPersonMovement player;

    /// <summary>
    /// Initialization
    /// </summary>
    void Awake()
    {
        // Cache the reference to the main puzzle controller
        puzzle = FindObjectOfType<FirstPuzzle>();

        player = FindObjectOfType<MyvariThirdPersonMovement>();
    }

    /// <summary>
    /// Main update loop
    /// </summary>
    void Update()
    {
        // If the puzzle is already completed, do nothing
        if (FirstPuzzle.firstPuzzleCompleted)
        {
            return;
        }

        // Read the interact key input value
        interactHeld = interactInput.action.ReadValue<float>() > 0;
    }

    /// <summary>
    /// Called when this collider first intersects another trigger collider
    /// </summary>
    /// <param name="other">Other collider</param>
    void OnTriggerEnter(Collider other)
    {
        // If the puzzle is already completed, do nothing
        if (FirstPuzzle.firstPuzzleCompleted)
        {
            return;
        }


        // Is the other collider a trigger for the first puzzle? If so, set the
        // color to yellow to indicate the trigger is occupied
        FirstPuzzleTrigger fpt = other.GetComponent<FirstPuzzleTrigger>();
        if (fpt != null)
        {
            fpt.ActivateInteractiveParticle(true);
            fpt.SetColor(yellowTrigger);
        }
    }

    /// <summary>
    /// Called every frame this collider intersects another trigger collider
    /// </summary>
    /// <param name="other">Other collider</param>
    void OnTriggerStay(Collider other)
    {
        // If the puzzle is already completed, do nothing
        if (FirstPuzzle.firstPuzzleCompleted)
        {
            return;
        }

        FirstPuzzleTrigger fpt = other.GetComponent<FirstPuzzleTrigger>();
        fpt.ActivateInteractiveParticle(true);

        // If the Interact button was held this frame...
        if (interactHeld)
        {
            

            // Is the other collider a trigger for the first puzzle? If so, push
            // this pillar group
            
            if (fpt != null)
            {
                fpt.ActivateInteractiveParticle(false);
                puzzle.RotatePillar(fpt);
                player.HandlePushing(true);
            }
        }
        else
        {
            player.HandlePushing(false);
        }

        
    }

    /// <summary>
    /// Called when this collider stops intersecting another trigger collider
    /// </summary>
    /// <param name="other">Other collider</param>
    void OnTriggerExit(Collider other)
    {
        // If the puzzle is already completed, do nothing
        if (FirstPuzzle.firstPuzzleCompleted)
        {
            return;
        }

        // Is the other collider a trigger for the first puzzle? If so, set the
        // color to red to indicate the trigger is no longer occupied
        FirstPuzzleTrigger fpt = other.GetComponent<FirstPuzzleTrigger>();
        if (fpt != null)
        {
            fpt.ActivateInteractiveParticle(false);
            fpt.SetColor(redTrigger);
        }
    }
}
