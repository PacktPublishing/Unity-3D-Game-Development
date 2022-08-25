using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    [SerializeField]
    private Image ScreenFade;

    // Stairs Cinematics
    [SerializeField]
    private PlayableDirector leftStairCine;

    [SerializeField]
    private CinemachineVirtualCamera leftStairsCamera;

    [SerializeField]
    private Transform leftStairEnter;

    [SerializeField]
    private PlayableDirector rightStairCine;

    [SerializeField]
    private CinemachineVirtualCamera rightStairsCamera;

    [SerializeField]
    private Transform rightStairEnter;

    //Tight Spaces fields
    [SerializeField]
    private PlayableDirector tightSpaces;

    [SerializeField]
    private CinemachineVirtualCamera tightSpacesCamera;

    [SerializeField]
    private Transform tightSpacesEnter;

    [SerializeField]
    private Transform tightSpacesExit;

    //First Blush fields
    [SerializeField]
    private PlayableDirector firstBlush;

    [SerializeField]
    private Transform firstBlushEnter;

    [SerializeField]
    private Transform firstBlushExit;

    [SerializeField]
    private CinemachineVirtualCamera FirstBlushCamera;

    // Crown Me fields
    [SerializeField]
    private PlayableDirector crownMe;

    [SerializeField]
    private Transform crownMeEnter;

    [SerializeField]
    private Transform crownMeExit;

    [SerializeField]
    private CinemachineVirtualCamera CrownMeCamera;



    // Small Cinematics fields
    [SerializeField]
    private PlayableDirector firstTK;

    [SerializeField]
    private CrosshairManager crosshairManager;


    private Transform playerRoot;
    private Color endColor = new Color(0,0,0,0);

    private void Start()
    {
        // Find the player
        playerRoot = FindObjectOfType<MyvariThirdPersonMovement>().transform;
    }

    public void LeftStairsTrigger()
    {
        playerRoot.localPosition = leftStairEnter.localPosition;
        playerRoot.localRotation = leftStairEnter.localRotation;

        SetPlayerEnabled(false);
        leftStairCine.Play();
        //crosshairManager.CrosshairToggle(false);
    }

    public void RightStairsTrigger()
    {
        playerRoot.localPosition = rightStairEnter.localPosition;
        playerRoot.localRotation = rightStairEnter.localRotation;

        SetPlayerEnabled(false);
        rightStairCine.Play();
        //crosshairManager.CrosshairToggle(false);
    }

    public void PlayTightSpaces()
    {
        playerRoot.localPosition = tightSpacesEnter.localPosition;
        playerRoot.localRotation = tightSpacesEnter.localRotation;

        tightSpacesCamera.Priority += 10;

        SetPlayerEnabled(false);
        tightSpaces.Play();
        crosshairManager.CrosshairToggle(false);
    }

    public void FinishTightSpaces()
    {
        playerRoot.localPosition = tightSpacesExit.localPosition;
        playerRoot.localRotation = tightSpacesExit.localRotation;

        tightSpacesCamera.Priority -= 10;
        StartCoroutine(LerpFunction(endColor, 1));
        crosshairManager.CrosshairToggle(true);
        SetPlayerEnabled(true);
    }

    public void PlayFirstBlush()
    {
        playerRoot.localPosition = firstBlushEnter.localPosition;
        playerRoot.localRotation = firstBlushEnter.localRotation;

        FirstBlushCamera.Priority += 10;

        SetPlayerEnabled(false);
        crosshairManager.CrosshairToggle(false);
        firstBlush.Play();
    }

    public void FinishFirstBlush()
    {
        playerRoot.localPosition = firstBlushExit.localPosition;
        playerRoot.localRotation = firstBlushExit.localRotation;

        FirstBlushCamera.Priority -= 10;
        StartCoroutine(LerpFunction(endColor, 1));
        crosshairManager.CrosshairToggle(true);
        SetPlayerEnabled(true);
    }

    public void PlayFirstTK()
    {
        //crosshairManager.CrosshairToggle(false);
        firstTK.Play();
    }

    public void PlayCrownMe()
    {
        playerRoot.localPosition = crownMeEnter.localPosition;
        playerRoot.localRotation = crownMeEnter.localRotation;

        CrownMeCamera.Priority += 10;
        crosshairManager.CrosshairToggle(false);
        SetPlayerEnabled(false);
        crownMe.Play();
    }

    public void SetPlayerEnabled()
    {
        SetPlayerEnabled(true);
    }

    IEnumerator LerpFunction(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = ScreenFade.color;
        while (time < duration)
        {
            ScreenFade.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        ScreenFade.color = endValue;
    }

    void SetPlayerEnabled(bool enable)
    {
        var cams = playerRoot.GetComponentsInChildren<CinemachineVirtualCamera>(true);
        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(enable);
        }
        playerRoot.GetComponentInChildren<MyvariThirdPersonMovement>().enabled = enable;
        playerRoot.GetComponentInChildren<CharacterController>().enabled = enable;
    }
}
