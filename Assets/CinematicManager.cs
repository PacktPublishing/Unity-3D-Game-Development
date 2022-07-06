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

    [SerializeField]
    private PlayableDirector tightSpaces;

    [SerializeField]
    private CinemachineVirtualCamera tightSpacesCamera;

    [SerializeField]
    private Transform tightSpacesEnter;

    [SerializeField]
    private Transform tightSpacesExit;

    [SerializeField]
    private PlayableDirector firstBlush;

    [SerializeField]
    private PlayableDirector crownMe;

    private Transform playerRoot;
    private Color endColor = new Color(0,0,0,0);

    private void Start()
    {
        // Find the player
        playerRoot = FindObjectOfType<MyvariThirdPersonMovement>().transform;
    }

    public void PlayTightSpaces()
    {
        playerRoot.localPosition = tightSpacesEnter.localPosition;
        playerRoot.localRotation = tightSpacesEnter.localRotation;

        tightSpacesCamera.Priority += 10;

        SetPlayerEnabled(false);
        tightSpaces.Play();
    }

    public void FinishTightSpaces()
    {
        playerRoot.localPosition = tightSpacesExit.localPosition;
        playerRoot.localRotation = tightSpacesExit.localRotation;

        tightSpacesCamera.Priority -= 10;
        StartCoroutine(LerpFunction(endColor, 1));
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
    }
}
