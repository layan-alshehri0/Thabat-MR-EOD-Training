using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CoverOpener_StartTimer : MonoBehaviour
{
    public LeaderboardManager manager;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    bool started;

    void Awake() { grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(); }

    void OnEnable()  { if (grab) grab.selectEntered.AddListener(OnGrab); }
    void OnDisable() { if (grab) grab.selectEntered.RemoveListener(OnGrab); }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (started) return;
        started = true;
        if (manager) manager.StartRun();
    }
}
