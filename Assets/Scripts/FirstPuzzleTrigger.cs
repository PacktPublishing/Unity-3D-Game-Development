using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller class for a single trigger volume for the first puzzle. This trigger
/// is used to determine if the player is currently within an 'interaction zone'
/// which allows the player to push the pillars with the Interact input.
/// </summary>
public class FirstPuzzleTrigger : MonoBehaviour
{
    /// <summary>
    /// The type of this trigger volume. This designates which set of pillars this
    /// trigger belongs to.
    /// </summary>
    public FirstPuzzleTriggerType triggerType;

    /// <summary>
    /// The 'push direction' which designates whether the pillars will rotate
    /// clockwise or counter-clockwise.
    /// </summary>
    public FirstPuzzleTriggerDirection triggerDirection;

    /// <summary>
    /// Cached reference to the trigger's MeshRenderer, used to set colors for
    /// visual debugging.
    /// </summary>
    MeshRenderer meshRenderer;

    [SerializeField]
    private GameObject interactionParticle;

    /// <summary>
    /// Initialization
    /// </summary>
    void Awake()
    {
        interactionParticle.SetActive(false);
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Sets the trigger's material color.
    /// </summary>
    /// <param name="color">New color</param>
    public void SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void ActivateInteractiveParticle(bool enable)
    {
        interactionParticle.SetActive(enable);
    }
}

/// <summary>
/// Trigger volume type
/// </summary>
public enum FirstPuzzleTriggerType
{
    Outer = 0,
    Middle,
    Inner
}

/// <summary>
/// Trigger volume direction
/// </summary>
public enum FirstPuzzleTriggerDirection
{
    Clockwise = 0,
    CounterClockwise
}
