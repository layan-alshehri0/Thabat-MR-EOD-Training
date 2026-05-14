using UnityEngine;
using UnityEngine.UI;

public class WearCheck : MonoBehaviour
{
    public Toggle toggle;   // اربط التوجّل حق الـ UI (Helmet / Backpack / Walkie)
    public string itemTag;  // اكتب هنا اسم الـ Tag (مثلاً "Helmet")

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(itemTag))
        {
            toggle.isOn = true; // يطلع الصح
        }
    }
}