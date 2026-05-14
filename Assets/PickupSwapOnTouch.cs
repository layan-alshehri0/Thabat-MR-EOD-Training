using UnityEngine;
using UnityEngine.UI;

public class PickupSwapOnTouch : MonoBehaviour
{
    [Header("What to show when picked (under XR Rig)")]
    public GameObject equippedObject;     // النسخة المتركّبة على الظهر/الرأس (مطفّية بالبداية)

    [Header("Checklist UI (optional)")]
    public GameObject checkmark;          // صورة ✅
    public Toggle checklistToggle;        // Toggle ✅
    public AudioSource pickSfx;           // صوت (اختياري)
    public GameObject pickVfx;            // مؤثر (اختياري)

    [Header("Touch filter")]
    public string handTag = "Hand";       // تاج اليد

    bool done;

    void OnTriggerEnter(Collider other)
    {
        if (done) return;
        if (!other.CompareTag(handTag)) return;

        done = true;

        // 1) أخفي الغرض اللي على الجدار
        gameObject.SetActive(false);

        // 2) أظهر النسخة المجهزة على XR Rig
        if (equippedObject) equippedObject.SetActive(true);

        // 3) UI
        if (checkmark)       checkmark.SetActive(true);
        if (checklistToggle) checklistToggle.isOn = true;

        // 4) صوت/مؤثر
        if (pickSfx) pickSfx.Play();
        if (pickVfx) Instantiate(pickVfx, transform.position, transform.rotation);
    }
}
