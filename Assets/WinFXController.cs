using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class WinFXController : MonoBehaviour
{
    [Header("Refs")]
    public Volume winVolume;           // اسحبي WinFXVolume هنا

    [Header("Timings")]
    public float fadeInTime  = 0.5f;   // دخول التأثير
    public float holdTime    = 0.8f;   // التثبيت
    public float fadeOutTime = 0.7f;   // الخروج

    Bloom bloom;
    Vignette vignette;
    ColorAdjustments colorAdj;
    ChromaticAberration ca;
    LensDistortion lens;

    void Awake()
    {
        if (winVolume)
        {
            winVolume.weight = 0f; // مقفّل افتراضيًا
            var profile = winVolume.profile;
            profile.TryGet(out bloom);
            profile.TryGet(out vignette);
            profile.TryGet(out colorAdj);
            profile.TryGet(out ca);
            profile.TryGet(out lens);
        }
    }

    public void PlayWinFX()
    {
        StopAllCoroutines();
        StartCoroutine(CoPlay());
    }

    IEnumerator CoPlay()
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeInTime)); // دخول
        yield return new WaitForSeconds(holdTime);             // تثبيت
        yield return StartCoroutine(Fade(1f, 0f, fadeOutTime));// خروج
    }

    IEnumerator Fade(float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(from, to, t / time);

            if (winVolume) winVolume.weight = k;

            // تعزيز إحساس الفوز بشكل لطيف
            if (bloom != null)
            {
                bloom.intensity.value  = Mathf.Lerp(0.0f, 1.2f, k);
                bloom.threshold.value  = Mathf.Lerp(1.2f, 0.9f, k);
            }
            if (colorAdj != null)
            {
                colorAdj.saturation.value   = Mathf.Lerp(0f, 30f, k);  // ألوان أزهى
                colorAdj.postExposure.value = Mathf.Lerp(0f, 0.8f, k); // إضاءة أعلى
                colorAdj.contrast.value     = Mathf.Lerp(0f, 10f, k);
            }
            if (vignette != null)
            {
                // فينييت خفيف جدًا عشان يبرز المركز
                vignette.intensity.value  = Mathf.Lerp(0.05f, 0.20f, k);
                vignette.smoothness.value = Mathf.Lerp(0.50f, 0.90f, k);
                vignette.color.value      = Color.white; // لمسة فاتحة
            }
            if (ca != null)
            {
                ca.intensity.value = Mathf.Lerp(0f, 0.05f, k); // كروماتيك خفيف جدًا
            }
            if (lens != null)
            {
                // انحناء بسيط للخارج يعطي "بوب" بسيط
                lens.intensity.value = Mathf.Lerp(0f, -0.05f, k);
            }

            yield return null;
        }
    }
}
