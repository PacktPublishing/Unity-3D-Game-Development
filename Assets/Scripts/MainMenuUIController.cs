using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;

/// <summary>
/// Basic UI controller for the world-space Main Menu
/// </summary>
public class MainMenuUIController : MonoBehaviour
{
    public Button startGameButton;
    public Button quitButton;

    [SerializeField]
    private CinemachineVirtualCamera MainMenuCam;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private CrosshairManager crosshairManager;

    private Transform playerRoot;

    void Awake()
    {
        // Find the player
        playerRoot = FindObjectOfType<MyvariThirdPersonMovement>().transform.parent;

        animator.SetBool("isOpening", true);

        // Disable player input while menu is visible
        SetPlayerEnabled(false);

        // Setup listeners
        startGameButton.onClick.AddListener(OnStartGameButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    void OnDestroy()
    {
        // Remove listeners
        startGameButton.onClick.RemoveListener(OnStartGameButtonPressed);
        quitButton.onClick.RemoveListener(OnQuitButtonPressed);
    }

    void OnStartGameButtonPressed()
    {
        SetPlayerEnabled(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.gameObject.SetActive(false);
        MainMenuCam.Priority -= 10;

        crosshairManager.CrosshairToggle(true);

        animator.SetBool("isOpening", false);
    }

    void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    void SetPlayerEnabled(bool enable)
    {
        var cams = playerRoot.GetComponentsInChildren<CinemachineVirtualCamera>(true);
        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(enable);
        }
        playerRoot.GetComponentInChildren<MyvariThirdPersonMovement>().enabled = enable;
    }
}
