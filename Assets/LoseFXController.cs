using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LoseFXController : MonoBehaviour
{
    [Header("Refs")]
    public Volume loseVolume;          // اسحبي LoseFXVolume هنا

    [Header("Timings")]
    public float fadeInTime = 0.6f;    // وقت دخول التأثير
    public float holdTime   = 1.0f;    // كم يمسك
    public float fadeOutTime= 0.6f;    // وقت الخروج

    Vignette vignette;
    ColorAdjustments colorAdj;
    ChromaticAberration ca;

    void Awake()
    {
        if (loseVolume)
        {
            loseVolume.weight = 0f; // مقفّل افتراضيًا
            var profile = loseVolume.profile;
            profile.TryGet(out vignette);
            profile.TryGet(out colorAdj);
            profile.TryGet(out ca);
        }
    }

    public void PlayLoseFX()
    {
        StopAllCoroutines();
        StartCoroutine(CoPlay());
    }

    IEnumerator CoPlay()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeInTime)); // دخول
        yield return new WaitForSeconds(holdTime);             // تثبيت
        yield return StartCoroutine(Fade(1f, 0f, fadeOutTime));// خروج (لو تبين)
    }

    IEnumerator Fade(float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(from, to, t / time);

            if (loseVolume) loseVolume.weight = k;

            // قيم إضافية تُشدّ بالتوازي (اختياري لكنها تعطي إحساس أقوى)
            if (vignette != null)
            {
                vignette.intensity.value  = Mathf.Lerp(0.10f, 0.55f, k);
                vignette.smoothness.value = Mathf.Lerp(0.50f, 0.95f, k);
            }
            if (colorAdj != null)
            {
                colorAdj.saturation.value   = Mathf.Lerp(0f, -60f, k);
                colorAdj.postExposure.value = Mathf.Lerp(0f, -0.8f, k);
            }
            if (ca != null)
            {
                ca.intensity.value = Mathf.Lerp(0f, 0.25f, k);
            }

            yield return null;
        }
    }
}
