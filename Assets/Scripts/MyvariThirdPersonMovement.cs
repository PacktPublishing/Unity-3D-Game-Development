using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

[RequireComponent(typeof(CharacterController))]
public class MyvariThirdPersonMovement : MonoBehaviour
{
    // ********** NOTE **********
    // Declaring the public and private variables on the top allows for other programmers to see
    // whats available very easily. This is a great practice and standard of C# programming.
    // ********** NOTE **********

    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference sprintControl;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float sprintSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotFactorPerFrame = 1.0f;

    // LayerMask for raycasting against water
    [SerializeField]
    private LayerMask waterLayer;

    // How much to slow the player when standing in water
    [SerializeField]
    private float waterSlowFactor = 0.5f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private Transform cameraMainTransform;
    private bool standingInWater = false;
    private bool bookIsOpen = false;

    void Awake()
    {
        HandleWalkAnim(Vector2.zero);
    }

    // OnEnable and OnDisable have to be done per action from the input system to wake them up for use
    private void OnEnable()
    {
        movementControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        if (bookIsOpen)
        {
            return;
        }

        // This block is checking if the character is on the ground or not
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Check for water
        standingInWater = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 2f, waterLayer);

        // read in the values of the input action assigned to this script
        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        // Use the values from the inputs and put them into a vector3, leaving up blank
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        // take into account the camera's forward as this needs to be relative to the view of the camera
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;

        // zero out that y value just in case ;)
        move.y = 0.0f;

        bool sprintPressed = sprintControl.action.ReadValue<float>() > 0f;

        controller.Move(move * Time.deltaTime * (sprintPressed ? sprintSpeed : playerSpeed) * (standingInWater ? waterSlowFactor : 1f));
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        HandleRotation(movement);
        HandleWalkAnim(movement);
        HandleSprintAnim(sprintPressed);
    }

    void HandleRotation(Vector2 movement)
    {
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleWalkAnim(Vector2 movement)
    {
        bool isWalking = animator.GetBool("isWalking");

        if (movement != Vector2.zero && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }

        else if (!(movement != Vector2.zero) && isWalking)
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void HandleSprintAnim(bool sprintPressed)
    {
        animator.SetBool("isSprint", sprintPressed);
    }

    public void HandleOpenBookAnim(bool bookPressed)
    {
        animator.SetBool("isEscapeMenu", bookPressed);
    }

    public void HandleFlipPageAnim(bool flipPressed)
    {
        animator.SetBool("isFlipForward", flipPressed);
    }

    public void HandleTelekinesisAnim(bool telekinesisPressed)
    {
        animator.SetBool("isTelekinesis", telekinesisPressed);
    }

    public void HandlePushing(bool pushingPressed)
    {
        animator.SetBool("isPushing", pushingPressed);
    }
}
