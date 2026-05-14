using UnityEngine;


public class ShowUIOnTeleport : MonoBehaviour
{
    public GameObject uiPanel;        // اسحبي الـ Panel هنا
    private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor anchor;

    void Awake()
    {
        anchor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor>();
        if (anchor != null)
            anchor.teleporting.AddListener(OnTeleported);
    }

    // التوقيع الصحيح للحدث
    void OnTeleported(UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportingEventArgs args)
    {
        if (uiPanel != null)
            uiPanel.SetActive(true);
    }
}
