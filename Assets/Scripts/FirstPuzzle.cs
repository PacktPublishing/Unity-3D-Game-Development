using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.Playables;

/// <summary>
/// Primary controller class for the first puzzle
/// </summary>
public class FirstPuzzle : MonoBehaviour
{
    /// <summary>
    /// Outer pillar group
    /// </summary>
    public Transform outerPillars;

    /// <summary>
    /// Middle pillar group
    /// </summary>
    public Transform middlePillars;

    /// <summary>
    /// Inner pillar group
    /// </summary>
    public Transform innerPillars;

    /// <summary>
    /// Center spire
    /// </summary>
    public Transform centerSpire;

    /// <summary>
    /// The correct rotation value for the outer pillar group, in degrees
    /// </summary>
    public float correctRotationOuter = 120f;

    /// <summary>
    /// The correct rotation value for the middle pillar group, in degrees
    /// </summary>
    public float correctRotationMiddle = 240f;

    /// <summary>
    /// The correct rotation value for the inner pillar group, in degrees
    /// </summary>
    public float correctRotationInner = 75f;

    /// <summary>
    /// The pillar group rotations will be considered correct if they are within this
    /// given threshold, in degrees
    /// </summary>
    public float correctThreshold = 5f;

    /// <summary>
    /// How fast the player rotates the pillar group, in degrees per second
    /// </summary>
    public float pushSpeed = 10f;

    /// <summary>
    /// Has the first puzzle been completed?
    /// </summary>
    public static bool firstPuzzleCompleted = false;

    public static UnityAction OnFirstPuzzleComplete;

    /// <summary>
    /// Collection of all the puzzle trigger volumes
    /// </summary>
    FirstPuzzleTrigger[] triggers;

    /// <summary>
    /// Cached reference to the player
    /// </summary>
    CharacterController playerController;

    MyvariThirdPersonMovement player;

    /// <summary>
    /// Is the outer pillar group aligned?
    /// </summary>
    bool outerAligned = false;

    /// <summary>
    /// Is the middle pillar group aligned?
    /// </summary>
    bool middleAligned = false;

    /// <summary>
    /// Is the inner pillar group aligned?
    /// </summary>
    bool innerAligned = false;

    /// <summary>
    /// Have we displayed the 'outer pillar group aligned' message? This prevents
    /// the message from repeatedly printing to the console
    /// </summary>
    bool displayedOuterAligned = false;

    /// <summary>
    /// Have we displayed the 'middle pillar group aligned' message? This prevents
    /// the message from repeatedly printing to the console
    /// </summary>
    bool displayedMiddleAligned = false;

    /// Have we displayed the 'inner pillar group aligned' message? This prevents
    /// the message from repeatedly printing to the console
    bool displayedInnerAligned = false;

    /// <summary>
    /// Have we displayed the 'victory' message? This prevents the message from 
    /// repeatedly printing to the console
    /// </summary>
    bool displayedVictory = false;

    /// <summary>
    /// Outer pillar group starting rotation at moment of victory, used for lerping
    /// </summary>
    Vector3 outerStartVictory;

    /// <summary>
    /// Middle pillar group starting rotation at moment of victory, used for lerping
    /// </summary>
    Vector3 middleStartVictory;

    /// <summary>
    /// Inner pillar group starting rotation at moment of victory, used for lerping
    /// </summary>
    Vector3 innerStartVictory;

    /// <summary>
    /// Outer pillar group victory ending rotation, used for lerping
    /// </summary>
    Vector3 outerEndVictory = new Vector3(0f, 0, 0f);

    /// <summary>
    /// Middle pillar group victory ending rotation, used for lerping
    /// </summary>
    Vector3 middleEndVictory = new Vector3(0f, 0f, 0f);

    /// <summary>
    /// Inner pillar group victory ending rotation, used for lerping
    /// </summary>
    Vector3 innerEndVictory = new Vector3(0f, 0f, 0f);

    /// <summary>
    /// Color used for the trigger debug volumes to indicate victory
    /// </summary>
    Color victoryColor = new Color(0f, 1f, 0f, 0.5f);

    /// <summary>
    /// Has the victory lerp reached its full duration?
    /// </summary>
    bool victoryLerpComplete = false;

    /// <summary>
    /// What time the victory lerp started
    /// </summary>
    float victoryStartTime;

    /// <summary>
    /// Desired victory lerp duration in seconds
    /// </summary>
    float victoryDuration = 2f;

    /// <summary>
    /// Starting height of the center spire
    /// </summary>
    float centerSpireStartHeight = -0.4f;

    /// <summary>
    /// Final height of the center spire after victory
    /// </summary>
    float centerSpireVictoryHeight = 0.3f;

    List<Material> innerPillarMaterials = new();
    List<Material> middlePillarMaterials = new();
    List<Material> outerPillarMaterials = new();

    public AudioSource puzzleCompleteSFX;

    [SerializeField]
    private PlayableDirector endOfPuzzleCinematic;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        // Cache references to the trigger volumes and the player
        triggers = GetComponentsInChildren<FirstPuzzleTrigger>();
        playerController = FindObjectOfType<CharacterController>();
        player = FindObjectOfType<MyvariThirdPersonMovement>();

        // Random starting positions
        outerPillars.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);
        middlePillars.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);
        innerPillars.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);

        // Assigning Material
        foreach (Transform pillar in innerPillars)
        {
            innerPillarMaterials.Add(pillar.gameObject.GetComponent<MeshRenderer>().materials[1]);
        }

        foreach (Transform pillar in middlePillars)
        {
            middlePillarMaterials.Add(pillar.gameObject.GetComponent<MeshRenderer>().materials[1]);
        }

        foreach (Transform pillar in outerPillars)
        {
            outerPillarMaterials.Add(pillar.gameObject.GetComponent<MeshRenderer>().materials[1]);
        }

        // Starting center spire position
        centerSpire.position = new Vector3(centerSpire.position.x, centerSpireStartHeight, centerSpire.position.z);
    }

    /// <summary>
    /// Main update loop
    /// </summary>
    void Update()
    {
        // Every frame we need to check for the victory condition
        if (!firstPuzzleCompleted)
        {
            firstPuzzleCompleted = CheckForVictory();
        }

        if (firstPuzzleCompleted && !displayedVictory)
        {
            DisplayVictory();
            displayedVictory = true;
        }

        if (displayedVictory && !victoryLerpComplete)
        {
            player.HandlePushing(false);
            //PerformVictoryLerp();
        }

        PrintDebug();
    }

    /// <summary>
    /// Checks the pillar group rotations to see if they are all within the allowed
    /// threshold, 
    /// </summary>
    /// <returns>True if victory condition is met, false otherwise</returns>
    bool CheckForVictory()
    {
        // Check pillar group alignment compared to the puzzle solution
        outerAligned = CheckAlignment(outerPillars, outerPillarMaterials, correctRotationOuter);
        middleAligned = CheckAlignment(middlePillars, middlePillarMaterials, correctRotationMiddle);
        innerAligned = CheckAlignment(innerPillars, innerPillarMaterials, correctRotationInner);


        // Everything is aligned so display the victory
        if (outerAligned && middleAligned && innerAligned)
        {
            puzzleCompleteSFX.Play();
            Debug.Log("Finished it up");
            // Play cinematic door opening
            endOfPuzzleCinematic.Play();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the rotation of a given pillar group is within the allowed threshold
    /// </summary>
    /// <param name="pillarGroup">Pillar group to check</param>
    /// <param name="correctRotation">Correct rotation value</param>
    /// <returns>True if within threshold, false otherwise</returns>
    bool CheckAlignment(Transform pillarGroup, List<Material> pillarMaterial, float correctRotation)
    {
        // Check how close the rotation is and change material emissive boost
        float StartValue = 1f;
        float EndValue = 15f;

        float EmissiveValue = Mathf.Lerp(EndValue, StartValue, Mathf.InverseLerp(0f, 180f, Mathf.Abs(UnityEditor.TransformUtils.GetInspectorRotation(pillarGroup.transform).y)));

        foreach (Material material in pillarMaterial)
        {
            material.SetFloat("_Emis_Boost", EmissiveValue);
        }

        // If the absolute difference between the pillar rotation and the correct rotation
        // is within the allowed threshold, this pillar group is at or near the correct
        // position
        if (Mathf.Abs(UnityEditor.TransformUtils.GetInspectorRotation(pillarGroup.transform).y) < correctThreshold)
        {
            foreach (Material material in pillarMaterial)
            {
                material.SetFloat("_Emis_Boost", 40);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Displays the victory message, stores starting victory values, and disables triggers
    /// </summary>
    void DisplayVictory()
    {
        Debug.Log("FIRST PUZZLE SOLVED");

        // Store the starting lerp values
        victoryStartTime = Time.time;
        outerStartVictory = UnityEditor.TransformUtils.GetInspectorRotation(outerPillars.transform);
        middleStartVictory = UnityEditor.TransformUtils.GetInspectorRotation(middlePillars.transform);
        innerStartVictory = UnityEditor.TransformUtils.GetInspectorRotation(innerPillars.transform);

        // Set the trigger debug color and disable all future interactions
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].SetColor(victoryColor);
            triggers[i].enabled = false;
        }
    }

    /// <summary>
    /// Performs the victory lerp which moves the pillar groups to their final solution
    /// positions and raises the center spire
    /// </summary>
    void PerformVictoryLerp()
    {
        // Normalize the lerp value to be '0 to 1' instead of '0 to duration'
        float lerpVal = (Time.time - victoryStartTime) / victoryDuration;

        // Check for lerp completion and clamp the value to 1
        if (lerpVal >= 1f)
        {
            lerpVal = 1f;
            victoryLerpComplete = true;

            // Notify others that the puzzle has been solved
            OnFirstPuzzleComplete?.Invoke();
        }

        // Lerp the pillar rotations from their starting positions to the final true 
        // solution positions
        outerPillars.eulerAngles = Vector3.Lerp(outerStartVictory, outerEndVictory, lerpVal);
        middlePillars.eulerAngles = Vector3.Lerp(middleStartVictory, middleEndVictory, lerpVal);
        innerPillars.eulerAngles = Vector3.Lerp(innerStartVictory, innerEndVictory, lerpVal);

        // Lerp the center spire height
        float spireHeight = Mathf.Lerp(centerSpireStartHeight, centerSpireVictoryHeight, lerpVal);
        centerSpire.position = new Vector3(centerSpire.position.x, spireHeight, centerSpire.position.z);
    }

    /// <summary>
    /// Prints some debug information to the console
    /// </summary>
    void PrintDebug()
    {
        // If the outer pillar group is aligned and we have not reported that yet, display
        // a message

        if (outerAligned && !displayedOuterAligned)
        {
            displayedOuterAligned = true;
            Debug.Log("Outer pillars are aligned");
            FirstPuzzleTrigger[] triggers = outerPillars.GetComponentsInChildren<FirstPuzzleTrigger>();
            foreach (FirstPuzzleTrigger trigger in triggers)
            {
                trigger.SetColor(victoryColor);
            }
        }
        else if (!outerAligned)
        {
            displayedOuterAligned = false;
        }

        // If the middle pillar group is aligned and we have not reported that yet, display
        // a message
        if (middleAligned && !displayedMiddleAligned)
        {
            displayedMiddleAligned = true;
            Debug.Log("Middle pillars are aligned");
            FirstPuzzleTrigger[] triggers = middlePillars.GetComponentsInChildren<FirstPuzzleTrigger>();
            foreach (FirstPuzzleTrigger trigger in triggers)
            {
                trigger.SetColor(victoryColor);
            }
        }
        else if (!middleAligned)
        {
            displayedMiddleAligned = false;
        }

        // If the inner pillar group is aligned and we have not reported that yet, display
        // a message
        if (innerAligned && !displayedInnerAligned)
        {
            displayedInnerAligned = true;
            Debug.Log("Inner pillars are aligned");
            FirstPuzzleTrigger[] triggers = innerPillars.GetComponentsInChildren<FirstPuzzleTrigger>();
            foreach (FirstPuzzleTrigger trigger in triggers)
            {
                trigger.SetColor(victoryColor);
            }
        }
        else if (!innerAligned)
        {
            displayedInnerAligned = false;
        }
    }

    /// <summary>
    /// Rotates a given pillar group based on which trigger volume is occupied by the player
    /// </summary>
    /// <param name="trigger">Trigger volume occupied by player</param>
    public void RotatePillar(FirstPuzzleTrigger trigger)
    {
        // Rotate pillars
        float rot = (trigger.triggerDirection == FirstPuzzleTriggerDirection.Clockwise ? pushSpeed : -pushSpeed) * Time.deltaTime;
        trigger.transform.parent.parent.Rotate(Vector3.up, rot);

        // Keep player locked in trigger volume, facing the pillar. We need to disable the CharacterController here
        // when setting a new position, otherwise it will overwrite the new position with the player's current position
        playerController.enabled = false;
        float origY = playerController.transform.position.y;
        playerController.transform.position = new Vector3(trigger.transform.position.x, origY, trigger.transform.position.z);
        playerController.transform.forward = trigger.transform.forward;
        playerController.enabled = true;
    }
}
