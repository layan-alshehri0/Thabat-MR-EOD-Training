using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefuseSequence : MonoBehaviour
{
    [Header("Order (IDs) - Example: cover, pin, plug")]
    public List<string> correctOrder = new List<string>();

    [Header("Optional: lock grabs when ends")]
    public List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> partsToDisable;

    [Header("Audio (اختياري)")]
    public AudioSource sfxSuccess;
    public AudioSource sfxFail;

    [Tooltip("مصدر صوت لصوت الألم (يُشغّل عند الخسارة).")]
    public AudioSource sfxHurt;                 // << جديد

    [Tooltip("لو فضّلتِ استخدام عدة مقاطع ألم عشوائية، ضعيها هنا وسيتم تشغيلها على sfxHurt.")]
    public AudioClip[] hurtClips;               // << اختياري

    [Range(0f, 1f)]
    [Tooltip("تأخير بسيط قبل تشغيل صوت الألم بعد الخسارة (ثوانٍ).")]
    public float hurtDelay = 0.1f;              // << اختياري

    [Header("Explosion (اختر واحدًا فقط)")]
    [Tooltip("لو عندك كائن انفجار موجود داخل الـScene (مثلاً BigExplosion عليه AutoExplosion) اربطيه هنا.")]
    public AutoExplosion explosionInScene;   // يشغّل الانفجار الموجود في المشهد

    [Tooltip("أو لو تبين تستنسخ انفجار كـPrefab من الـProject اربطيه هنا.")]
    public AutoExplosion explosionPrefab;    // يستنسخ Prefab جديد ويشغّله

    [Tooltip("مكان ظهور الانفجار (اختياري). لو تُرك فارغًا سيستخدم موضع هذا الكائن.")]
    public Transform explosionPoint;

    [Header("Events")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    int stepIndex = 0;
    bool ended = false;

    public void TryStep(string id)
    {
        if (ended) return;
        if (stepIndex < 0 || stepIndex >= correctOrder.Count) return;

        if (id == correctOrder[stepIndex])
        {
            stepIndex++;
            if (stepIndex >= correctOrder.Count) EndRun(true);
        }
        else
        {
            EndRun(false);
        }
    }

    void EndRun(bool success)
    {
        if (ended) return;
        ended = true;

        if (partsToDisable != null)
            foreach (var g in partsToDisable) if (g) g.enabled = false;

        if (success)
        {
            if (sfxSuccess) sfxSuccess.Play();
            OnSuccess?.Invoke();
        }
        else
        {
            if (sfxFail) sfxFail.Play();
            TriggerExplosion();

            // شغّل صوت الألم (بعد تأخير بسيط لو حبيتي)
            if (sfxHurt)
                StartCoroutine(PlayHurtAfterDelay(hurtDelay));

            OnFail?.Invoke();
        }
    }

    IEnumerator PlayHurtAfterDelay(float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        // لو عندك قائمة مقاطع، اختر واحدًا عشوائياً
        if (hurtClips != null && hurtClips.Length > 0)
        {
            var clip = hurtClips[Random.Range(0, hurtClips.Length)];
            sfxHurt.PlayOneShot(clip);
        }
        else
        {
            // أو شغّل الـAudioClip المربوط مباشرة على sfxHurt
            // (لو كان AudioClip على sfxHurt، يشتغل بـPlay؛ ولو فاضي استخدم PlayOneShot عند الحاجة)
            if (sfxHurt.clip != null) sfxHurt.Play();
        }
    }

    void TriggerExplosion()
    {
        // 1) لو فيه كائن انفجار داخل المشهد
        if (explosionInScene)
        {
            explosionInScene.Explode();
            return;
        }

        // 2) لو ما فيه Scene object، لكن عندك Prefab
        if (explosionPrefab)
        {
            Vector3 pos = explosionPoint ? explosionPoint.position : transform.position;
            Quaternion rot = explosionPoint ? explosionPoint.rotation : Quaternion.identity;
            AutoExplosion inst = Instantiate(explosionPrefab, pos, rot);
            inst.Explode();
        }
        // لو الاثنين فاضيين: ما يعمل شيء (آمن)
    }

    public void ResetRun()
    {
        ended = false;
        stepIndex = 0;
        if (partsToDisable != null)
            foreach (var g in partsToDisable) if (g) g.enabled = true;
    }
}
