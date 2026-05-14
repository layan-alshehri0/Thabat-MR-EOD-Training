using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class PartSound : MonoBehaviour
{
    [Header("الصوت")]
    public AudioSource pickupSound;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab); // لما اللاعب يمسك القطعة
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (pickupSound) pickupSound.Play();
    }
}
