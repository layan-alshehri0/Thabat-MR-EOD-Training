using UnityEngine;

public class LandmineTrigger : MonoBehaviour
{
    [Header("UI Manager")]
    public UI_Manager uiManager;   // اسحبي UI_Manager هنا من Inspector

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;   // لمنع التفعيل أكثر من مرة

        // تحقق إذا كان اللاعب أو أي جسم محدد وصل للـ landmine
        if (other.CompareTag("Player"))
        {
            triggered = true;
            if (uiManager != null)
            {
                uiManager.OnPressNext(); // تشغيل التعليمات
            }
        }
    }
}
