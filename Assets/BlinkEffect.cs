using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlinkEffect : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;

    void Start()
    {
        if (volume != null && volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f; // تبدأ بدون رمشة
        }
    }

    public void Blink()
    {
        if (vignette != null)
        {
            StopAllCoroutines();
            StartCoroutine(BlinkRoutine());
        }
    }

    private System.Collections.IEnumerator BlinkRoutine()
    {
        // تغميق الشاشة (رمشة)
        for (float i = 0; i <= 1; i += Time.deltaTime * 4f)
        {
            vignette.intensity.value = i;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // وقت الإغلاق

        // ترجع تفتح العين
        for (float i = 1; i >= 0; i -= Time.deltaTime * 4f)
        {
            vignette.intensity.value = i;
            yield return null;
        }
    }
}