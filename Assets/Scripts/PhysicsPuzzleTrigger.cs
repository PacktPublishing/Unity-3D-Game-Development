using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsPuzzleTrigger : MonoBehaviour
{
    public PhysicsPuzzlePieceType correctPieceType;

    public float tweenDuration = 2f;

    public static UnityAction<PhysicsPuzzleTrigger, PhysicsPuzzlePiece> OnPieceSlotted;

    Transform tweenPiece;
    Vector3 tweenStartPos;
    Vector3 tweenStartRot;
    float tweenStart;

    void OnTriggerEnter(Collider other)
    {
        // Make sure the colliding object is a puzzle piece of the correct type
        PhysicsPuzzlePiece piece = other.GetComponent<PhysicsPuzzlePiece>();
        if (piece != null && (piece.pieceType == correctPieceType || correctPieceType == PhysicsPuzzlePieceType.Any))
        {
            Debug.Log("PIECE SLOTTED");

            // Disable rigidbody, gravity, and collider. Zero all velocity
            piece.GetComponent<Rigidbody>().isKinematic = true;
            piece.GetComponent<Rigidbody>().useGravity = false;
            piece.GetComponent<Rigidbody>().velocity = Vector3.zero;
            piece.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            piece.GetComponent<Collider>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // Setup tween references and starting values
            tweenPiece = other.transform;
            tweenStartPos = other.transform.position;
            tweenStartRot = other.transform.eulerAngles;
            tweenStart = Time.time;
            StartCoroutine(TransitionTween());
        }
    }

    IEnumerator TransitionTween()
    {
        Debug.Log("STARTING PIECE TWEEN");

        // TODO: Temp
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0f, 1f, 0f, 0.5f));

        // Main tween loop
        while (Time.time - tweenStart < tweenDuration)
        {
            float delta = (Time.time - tweenStart) / tweenDuration;
            tweenPiece.position = Vector3.Lerp(tweenStartPos, transform.position, delta);
            tweenPiece.eulerAngles = Vector3.Slerp(tweenStartRot, transform.eulerAngles, delta);
            yield return null;
        }

        // Tween complete
        tweenPiece.position = transform.position;
        tweenPiece.eulerAngles = transform.eulerAngles;
        OnPieceSlotted?.Invoke(this, tweenPiece.GetComponent<PhysicsPuzzlePiece>());

        Debug.Log("PIECE TWEEN COMPLETE");

    }
}


public enum PhysicsPuzzlePieceType
{
    First = 0,
    Second,
    Third,
    Intro,
    Any
}