using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class MyvariThirdPersonAimMovement : MonoBehaviour
{
    // ********** NOTE **********
    // Declaring the public and private variables on the top allows for other programmers to see
    // whats available very easily. This is a great practice and standard of C# programming.
    // ********** NOTE **********

    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotFactorPerFrame = 1.0f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;

    private InputAction moveAction;
    private InputAction lookAction;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Locomotion"];
        lookAction = playerInput.actions["Look"];

        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        // This block is checking if the character is on the ground or not
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // read in the values of the input action assigned to this script
        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        // Use the values from the inputs and put them into a vector3, leaving up blank
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        // take into account the camera's forward as this needs to be relative to the view of the camera
        move = cameraMainTransform.forward.normalized * move.z + cameraMainTransform.right.normalized * move.x;

        // zero out that y value just in case ;)
        move.y = 0.0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        HandleRotation(movement);
        HandleAnimation(movement);
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

    void HandleRotation(Vector2 movement)
    {
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotFactorPerFrame * Time.deltaTime);
        }

    }

    void HandleAnimation(Vector2 movement)
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

    void HandleOpenBookAnimation(bool isBookOpen)
    {
        animator.SetTrigger("escapeMenu");
    }
}
