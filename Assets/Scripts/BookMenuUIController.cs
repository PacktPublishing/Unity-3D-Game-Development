using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

/// <summary>
/// Basic UI controller for the Escape Menu
/// </summary>
public class BookMenuUIController : MonoBehaviour
{
    public Button journalResumeButton;
    public Button systemQuitButton;
    public GameObject root;
    public InputActionReference openCloseBookControl;
    public InputActionReference flipPageControl;
    public CinemachineVirtualCamera bookCam;
    public MyvariThirdPersonMovement playerMovement;
    Transform playerRoot;
    GameObject mainMenu;

    void Awake()
    {
        // Find player
        playerRoot = FindObjectOfType<MyvariThirdPersonMovement>().transform.parent;

        // Find main menu
        mainMenu = FindObjectOfType<MainMenuUIController>().gameObject;

        // Setup listeners
        journalResumeButton.onClick.AddListener(OnJournalResumeButtonPressed);
        systemQuitButton.onClick.AddListener(OnSystemQuitButtonPressed);

        openCloseBookControl.action.performed += OnOpenCloseBookButtonPressed;
        flipPageControl.action.performed += FlipPageButtonPressed;

        // Hide panel
        SetPanelEnabled(false);
    }

    void Start()
    {
        bookCam.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Remove listeners
        journalResumeButton.onClick.RemoveListener(OnJournalResumeButtonPressed);
        systemQuitButton.onClick.RemoveListener(OnSystemQuitButtonPressed);

        openCloseBookControl.action.performed -= OnOpenCloseBookButtonPressed;
    }

    void OnJournalResumeButtonPressed()
    {
        CloseBook();
    }

    void OnSystemQuitButtonPressed()
    {
        Application.Quit();
    }

    public void SetPanelEnabled(bool enable, bool cursorLock = false)
    {
        root.SetActive(enable);
        SetPlayerEnabled(!enable);

        Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLock;
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

    void OnOpenCloseBookButtonPressed(InputAction.CallbackContext context)
    {
        if (!this.enabled || mainMenu.activeInHierarchy)
        {
            return;
        }

        if (bookCam.gameObject.activeInHierarchy)
        {
            CloseBook();
        }
        else
        {
            OpenBook();
        }
    }

    void FlipPageButtonPressed(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
    }

    void OpenBook()
    {
        SetPanelEnabled(true);
        bookCam.gameObject.SetActive(true);
        bookCam.Priority = 99;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerMovement.HandleOpenBookAnim(true);
    }

    void CloseBook()
    {
        SetPanelEnabled(false);
        bookCam.Priority = 0;
        bookCam.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement.HandleOpenBookAnim(false);
    }
}
