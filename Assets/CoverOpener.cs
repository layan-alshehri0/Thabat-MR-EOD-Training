using UnityEngine;


public class CoverOpener : MonoBehaviour
{
    public GameLogic gameLogic;

    void Start()
    {
        var grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(_ => gameLogic?.StartRun());
        }
    }
}