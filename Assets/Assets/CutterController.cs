using UnityEngine;

public class CutterController : MonoBehaviour
{
    public Transform leftJaw;
    public Transform rightJaw;

    public float closeAmount = 6f;
    public float smoothSpeed = 8f;

    public bool IsClosed { get; private set; }

    private Quaternion leftStartRotation;
    private Quaternion rightStartRotation;

    private float currentValue = 0f;

    void Start()
    {
        if (leftJaw != null)
            leftStartRotation = leftJaw.localRotation;

        if (rightJaw != null)
            rightStartRotation = rightJaw.localRotation;
    }

    void Update()
    {
        float trigger =
            OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);

        trigger = Mathf.Clamp01(trigger);

        currentValue = Mathf.Lerp(
            currentValue,
            trigger,
            Time.deltaTime * smoothSpeed
        );

        IsClosed = currentValue > 0.8f;

        if (leftJaw != null)
        {
            leftJaw.localRotation =
                leftStartRotation *
                Quaternion.Euler(0, 0, currentValue * closeAmount);
        }

        if (rightJaw != null)
        {
            rightJaw.localRotation =
                rightStartRotation *
                Quaternion.Euler(0, 0, -currentValue * closeAmount);
        }
    }
}