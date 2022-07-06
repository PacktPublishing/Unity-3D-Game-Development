using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerTP : MonoBehaviour
{
    [SerializeField]
    private MyvariThirdPersonMovement player;

    [SerializeField]
    private Transform firstPuzzle;
    [SerializeField]
    private Transform firstBlush;
    [SerializeField]
    private Transform preFinal;

    [SerializeField]
    private InputActionReference teleporterActions;


    private void OnEnable()
    {
        teleporterActions.action.Enable();
    }

    private void OnDisable()
    {
        teleporterActions.action.Disable();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ButtonControl firstPuzzleButton = teleporterActions.action.controls[0] as ButtonControl;
        ButtonControl firstBlushButton = teleporterActions.action.controls[1] as ButtonControl;
        ButtonControl preFinalButton = teleporterActions.action.controls[2] as ButtonControl;

        if (firstPuzzleButton.wasPressedThisFrame)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.gameObject.transform.position = firstPuzzle.position;
            player.GetComponent<CharacterController>().enabled = true;
        }

        if (firstBlushButton.wasPressedThisFrame)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.gameObject.transform.position = firstBlush.position;
            player.GetComponent<CharacterController>().enabled = true;
        }

        if (preFinalButton.wasPressedThisFrame)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.gameObject.transform.position = preFinal.position;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
