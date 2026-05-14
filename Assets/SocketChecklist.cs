using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketChecklist : MonoBehaviour
{
    [Header("Sockets")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor helmetSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor backpackSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor walkieSocket;

    [Header("UI Toggles")]
    public Toggle helmetCheck;
    public Toggle backpackCheck;
    public Toggle walkieCheck;

    private void OnEnable()
    {
        // Register socket events
        helmetSocket.selectEntered.AddListener(OnHelmetPlaced);
        backpackSocket.selectEntered.AddListener(OnBackpackPlaced);
        walkieSocket.selectEntered.AddListener(OnWalkiePlaced);
    }

    private void OnDisable()
    {
        // Unregister socket events
        helmetSocket.selectEntered.RemoveListener(OnHelmetPlaced);
        backpackSocket.selectEntered.RemoveListener(OnBackpackPlaced);
        walkieSocket.selectEntered.RemoveListener(OnWalkiePlaced);
    }

    private void OnHelmetPlaced(SelectEnterEventArgs args)
    {
        helmetCheck.isOn = true;   // check the box
    }

    private void OnBackpackPlaced(SelectEnterEventArgs args)
    {
        backpackCheck.isOn = true;
    }

    private void OnWalkiePlaced(SelectEnterEventArgs args)
    {
        walkieCheck.isOn = true;
    }
}
