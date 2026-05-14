using UnityEngine;

public class HideTeleportAnchor : MonoBehaviour
{
    public GameObject anchorObject; // اسحبي هنا المجسم اللي تبغينه يختفي

    public void HideAnchor()
    {
        if (anchorObject != null)
            anchorObject.SetActive(false);  // يخفيه بالكامل
    }
}
