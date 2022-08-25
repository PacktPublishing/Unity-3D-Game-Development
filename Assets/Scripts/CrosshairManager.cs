using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField]
    GameObject crosshair;

    public void CrosshairToggle(bool enable)
    {
        if (crosshair != null)
        {
            crosshair.SetActive(enable);
        }
    }
}
