using UnityEngine;
using UnityEngine.VFX; // لو تستخدم VFX Graph

public class EquipHandler : MonoBehaviour
{
    [Header("UI & Objects")]
    public GameObject uiPanel;       // واجهة الـUI
    public GameObject arrowObject;   // مجسم السهم

    [Header("Effects")]
    public ParticleSystem particleFx; // اختياري: لو تستخدم ParticleSystem
    public VisualEffect vfxGraph;     // اختياري: لو تستخدم VFX Graph

    public bool loop = true;          // يتحكم لو الـVFX يكرر نفسه

    public void OnEquipButton()
    {
        // إخفاء واجهة الـUI
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // إظهار السهم
        if (arrowObject != null)
            arrowObject.SetActive(true);

        // تشغيل ParticleSystem إن وجد
        if (particleFx != null)
        {
            var main = particleFx.main;
            main.loop = loop;
            particleFx.gameObject.SetActive(true);
            particleFx.Play();
        }

        // تشغيل VFX Graph إن وجد
        if (vfxGraph != null)
        {
            vfxGraph.gameObject.SetActive(true);
            vfxGraph.Play();
        }
    }
}
