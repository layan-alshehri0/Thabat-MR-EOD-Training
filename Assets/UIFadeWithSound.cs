using UnityEngine;
using System.Collections;

public class UIFadeWithSound : MonoBehaviour
{
    public CanvasGroup canvasGroup;     // اسحب CanvasGroup
    public AudioSource uiSound;         // اسحب AudioSource (الصوت)
    public float fadeInDuration = 0.6f;
    public float fadeOutDuration = 0.25f;
    public bool playOnEnable = false;   // لو تبغى يفيد تلقائياً عند التفعيل

    void Reset() {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup) canvasGroup.alpha = 0f;
    }

    void OnEnable() {
        if (playOnEnable) ShowUI();
    }

    public void ShowUI() {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeTo(1f, fadeInDuration, playSound:true));
    }

    public void HideUI() {
        StopAllCoroutines();
        StartCoroutine(FadeTo(0f, fadeOutDuration, playSound:false));
    }

    IEnumerator FadeTo(float target, float dur, bool playSound)
    {
        if (!canvasGroup) yield break;

        // ابدأ من القيمة الحالية لكن صفّرها لو بنظهر
        if (target > canvasGroup.alpha) canvasGroup.alpha = 0f;

        if (playSound && uiSound) uiSound.Play();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float start = canvasGroup.alpha;
        float t = 0f;
        while (t < dur) {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / dur);
            // EaseOutCubic للظهور
            float e = (target > start) ? (1f - Mathf.Pow(1f - k, 3f)) : k;
            canvasGroup.alpha = Mathf.Lerp(start, target, e);
            yield return null;
        }
        canvasGroup.alpha = target;

        bool on = target >= 0.999f;
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        if (!on) gameObject.SetActive(false);
    }
}
