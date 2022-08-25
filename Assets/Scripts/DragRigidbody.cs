using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.VFX;
using Cinemachine;

// TODO: Highlight object as selectable
//       Refactor to use puzzle pieces only
public class DragRigidbody : MonoBehaviour
{
    public InputActionReference interactInput;

    public float forceAmount = 500;

    public LayerMask selectableLayer;

    public VisualEffect telekinesis;
    public Transform leftWristLoc;
    public Transform beamStart;
    public Transform beamEnd;
    public MyvariThirdPersonMovement playerMovement;

    Rigidbody selectedRigidbody;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    float selectionDistance;
    CharacterController playerController;
    Transform playerRoot;
    PhysicsPuzzlePiece ppp;

    private void Awake()
    {
        // Find player
        playerRoot = FindObjectOfType<MyvariThirdPersonMovement>().transform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();
        playerController = FindObjectOfType<CharacterController>();

    }

    void Update()
    {
        if (!targetCamera)
        {
            return;
        }

        ButtonControl control = interactInput.action.controls[0] as ButtonControl;
        if (control.wasPressedThisFrame)
        {
            //Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromScreenCenter();
            
        }

        if (control.wasReleasedThisFrame)
        {
            //Release selected Rigidbody if there any
            selectedRigidbody = null;

            SetPlayerEnabled(true);

            telekinesis.enabled = false;
            playerMovement.HandleTelekinesisAnim(false);

            
        }
    }

    void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            telekinesis.enabled = true;

            ppp = selectedRigidbody.gameObject.GetComponent<PhysicsPuzzlePiece>();
            ppp.ActivateInteractiveParticle(false);

            Vector3 positionOffset = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + positionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;

            // SetPlayerEnabled(false);
            playerController.transform.forward = (new Vector3(selectedRigidbody.transform.position.x, playerController.transform.position.y, selectedRigidbody.transform.position.z) - playerController.transform.position).normalized;
            
            playerMovement.HandleTelekinesisAnim(true);

            beamStart.position = leftWristLoc.position;
            beamEnd.position = selectedRigidbody.gameObject.transform.position;
        }
        else
        {

        }
    }

    void SetPlayerEnabled(bool enable)
    {
        //var cams = playerRoot.GetComponentsInChildren<CinemachineVirtualCamera>(true);
        //foreach (var cam in cams)
        //{
        //    cam.gameObject.SetActive(enable);
        //}
        playerRoot.GetComponentInChildren<MyvariThirdPersonMovement>().enabled = enable;
    }

    Rigidbody GetRigidbodyFromScreenCenter()
    {
        Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // TODO: LayerMask
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, selectableLayer))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hit.point);
                originalScreenTargetPosition = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, selectionDistance));
                originalRigidbodyPos = hit.collider.transform.position;
                return hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}
