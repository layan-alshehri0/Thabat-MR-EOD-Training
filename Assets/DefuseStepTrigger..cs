using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class DefuseStepTrigger : MonoBehaviour
{
    public string stepId = "cover";        // عرفي اسم الخطوة (نفس اللي بالقائمة)
    public DefuseSequence sequence;        // اسحبيه من المشهد
    bool fired = false;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
    }

    void OnDestroy()
    {
        if (grab != null) grab.selectEntered.RemoveListener(OnGrabbed);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (fired || sequence == null) return;
        fired = true;                     // ما نكرّر الحدث
        sequence.TryStep(stepId);
    }
}