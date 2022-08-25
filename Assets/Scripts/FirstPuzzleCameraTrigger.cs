using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FirstPuzzleCameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera puzzleCamera;
    

    void Awake()
    {
        FirstPuzzle.OnFirstPuzzleComplete += OnFirstPuzzleComplete;
    }

    void OnDestroy()
    {
        FirstPuzzle.OnFirstPuzzleComplete -= OnFirstPuzzleComplete;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MyvariThirdPersonMovement>() != null)
        {
            puzzleCamera.gameObject.SetActive(true);
            puzzleCamera.Priority = 99;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MyvariThirdPersonMovement>() != null)
        {
            puzzleCamera.Priority = 0;
            puzzleCamera.gameObject.SetActive(false);
        }
    }

    void OnFirstPuzzleComplete()
    {
        puzzleCamera.Priority = 0;
        puzzleCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
