using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class BombDetectorHoldOnly : MonoBehaviour
{
    [Header("Detection")]
    public Transform bomb;             // اربطي القنبلة
    public float maxDistance = 10f;    // مدى الكشف

    [Header("Beep")]
    public AudioSource beepSource;     // لازم يكون له clip
    public float minInterval = 0.1f;   // أقصر فترة (أسرع بيب) عند أقرب مسافة
    public float maxInterval = 1.0f;   // أطول فترة (أبطأ بيب) عند أبعد مسافة
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Haptics")]
    public bool useHaptics = true;
    [Range(0f,1f)] public float maxHaptic = 1f;   // قوة الاهتزاز عند أقرب مسافة
    [Range(0f,1f)] public float minHaptic = 0.2f; // قوة الاهتزاز عند أبعد مسافة داخل المدى
    public float hapticDuration = 0.07f;          // مدة النبضة بالثواني

    // XR
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor holdingInteractor; // اليد/الراي الماسكة حاليًا
    Coroutine beepRoutine;
    bool isHeld;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited.AddListener(OnRelease);
        }

        if (beepSource == null) beepSource = GetComponent<AudioSource>();
        if (beepSource != null) beepSource.playOnAwake = false;
    }

    void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrab);
            grab.selectExited.RemoveListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        holdingInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;

        if (beepRoutine != null) StopCoroutine(beepRoutine);
        beepRoutine = StartCoroutine(BeepLoop());
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        holdingInteractor = null;

        if (beepRoutine != null) StopCoroutine(beepRoutine);
        beepRoutine = null;

        if (beepSource != null) beepSource.Stop();
    }

    IEnumerator BeepLoop()
    {
        while (isHeld)
        {
            if (bomb == null || beepSource == null || beepSource.clip == null)
            {
                yield return null;
                continue;
            }

            float d = Vector3.Distance(transform.position, bomb.position);
            if (d <= maxDistance)
            {
                // 0 عند القرب الشديد → 1 عند حدود المدى
                float t = Mathf.Clamp01(d / Mathf.Max(0.0001f, maxDistance));

                // صوت
                beepSource.volume = volume;
                beepSource.PlayOneShot(beepSource.clip);

                // هابتكس (قوة أقوى كلما قربت)
                if (useHaptics && holdingInteractor != null && holdingInteractor.xrController != null)
                {
                    float amp = Mathf.Lerp(maxHaptic, minHaptic, t); // أقرب = أقوى
                    holdingInteractor.xrController.SendHapticImpulse(amp, hapticDuration);
                }

                // الفترة بين البيبات: أقرب = أسرع
                float interval = Mathf.Lerp(minInterval, maxInterval, t);
                yield return new WaitForSeconds(interval);
            }
            else
            {
                // خارج المدى: انتظر قليل وارجع تحقق
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    // اختيارية: دائرة مدى الكشف في المشهد
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
