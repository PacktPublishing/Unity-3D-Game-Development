using System.Collections;
using UnityEngine;

public class OpenLastDoors : MonoBehaviour
{
    [SerializeField]
    Transform leftDoor, rightDoor;

    public Vector3 endPosValue = new Vector3(2.0f, 0.0f, 0.0f);
    public float timeToFinish = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        MyvariThirdPersonMovement player = other.GetComponent<MyvariThirdPersonMovement>();
        if (player != null)
        {
            StartCoroutine(LerpFunction(endPosValue, timeToFinish));
        }
    }

    IEnumerator LerpFunction(Vector3 endValue, float duration)
    {
        float time = 0;
        Vector3 startValueLD = leftDoor.localPosition;
        Vector3 startValueRD = rightDoor.localPosition;

        Vector3 rightDoorEndValue = new Vector3(endValue.x * -1, 0, 0);

        while (time < duration)
        {
            leftDoor.localPosition = Vector3.Lerp(startValueLD, endValue, time / duration);
            rightDoor.localPosition = Vector3.Lerp(startValueRD, rightDoorEndValue, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
        leftDoor.localPosition = endValue;
        rightDoor.localPosition = rightDoorEndValue;
    }
}
