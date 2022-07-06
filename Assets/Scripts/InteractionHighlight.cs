using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHighlight : MonoBehaviour
{
    public float maxDistance = 10f;
    public float height = 2f;
    public LayerMask rayMask;
    GameObject highlightIndicator;
    MeshRenderer highlightIndicatorRenderer;


    void Awake()
    {
        highlightIndicator = GameObject.FindGameObjectWithTag("InteractionHighlight");
        if (highlightIndicator == null)
        {
            Debug.LogError("Highlight indicator not found in scene");
        }
        highlightIndicatorRenderer = highlightIndicator.GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, rayMask, QueryTriggerInteraction.Collide))
        {
            highlightIndicator.transform.position = hit.transform.position + new Vector3(0f, height, 0f);
            if (!highlightIndicatorRenderer.enabled)
            {
                highlightIndicatorRenderer.enabled = true;
            }
        }
        else
        {
            if (highlightIndicatorRenderer.enabled)
            {
                highlightIndicatorRenderer.enabled = false;
            }
        }
    }
}
