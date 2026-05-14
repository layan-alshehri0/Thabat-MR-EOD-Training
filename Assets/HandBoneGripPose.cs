using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandBoneGripPose : MonoBehaviour
{
    [Header("Grab Object")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    [Header("Finger Targets On Cutter")]
    public Transform thumbTarget;
    public Transform indexTarget;
    public Transform middleTarget;

    [Header("Real Hand Bones / Tips")]
    public Transform thumbTip;
    public Transform indexTip;
    public Transform middleTip;

    [Header("Pose Settings")]
    public float followSpeed = 20f;
    public bool poseActive = false;

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        poseActive = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        poseActive = false;
    }

    void LateUpdate()
    {
        if (!poseActive) return;

        MoveBoneToTarget(thumbTip, thumbTarget);
        MoveBoneToTarget(indexTip, indexTarget);
        MoveBoneToTarget(middleTip, middleTarget);
    }

    void MoveBoneToTarget(Transform bone, Transform target)
    {
        if (bone == null || target == null) return;

        bone.position = Vector3.Lerp(
            bone.position,
            target.position,
            Time.deltaTime * followSpeed
        );

        bone.rotation = Quaternion.Slerp(
            bone.rotation,
            target.rotation,
            Time.deltaTime * followSpeed
        );
    }
}