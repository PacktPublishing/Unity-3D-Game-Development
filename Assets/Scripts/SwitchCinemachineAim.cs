using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchCinemachineAim : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    private InputAction aimAction;
    private CinemachineVirtualCamera virtualCamera;
    private int priorityBoostInt = 10;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => StopAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => StopAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostInt;
    }

    private void StopAim()
    {
        virtualCamera.Priority -= priorityBoostInt;
    }
}
