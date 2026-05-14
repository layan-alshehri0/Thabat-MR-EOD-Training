using UnityEngine;

public class UI_ProximityGate : MonoBehaviour
{
    [Header("What to show/hide")]
    public GameObject uiRoot;            // حطي هنا الـCanvas أو أب لكل البانيلز

    [Header("Trigger mode (recommended)")]
    public string playerTag = "Player";  // خلي XR Origin عليه هذا التاج

    [Header("Distance mode (optional)")]
    public bool useDistance = false;     // لو ما تبين كوليدر، فعّلي هذا
    public Transform player;             // حطي XR Origin أو Main Camera
    public float showRadius = 3f;        // نصف قطر الظهور بالمتر

    void Start()
    {
        if (uiRoot) uiRoot.SetActive(false); // مخفية بالبداية لين يقرب اللاعب
    }

    // وضع التريغر (زون بكوليدر)
    void OnTriggerEnter(Collider other)
    {
        if (useDistance) return;
        if (other.CompareTag(playerTag))
            SetVisible(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (useDistance) return;
        if (other.CompareTag(playerTag))
            SetVisible(false);
    }

    // وضع المسافة (بدون كوليدر)
    void Update()
    {
        if (!useDistance || !player) return;
        float sq = (player.position - transform.position).sqrMagnitude;
        SetVisible(sq <= showRadius * showRadius);
    }

    void SetVisible(bool v)
    {
        if (uiRoot && uiRoot.activeSelf != v)
            uiRoot.SetActive(v);
    }
}
