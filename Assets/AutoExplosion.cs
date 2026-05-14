using System.Collections;
using UnityEngine;
using UnityEngine.XR;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class AutoExplosion : MonoBehaviour
{
    [Header("Refs")]
    public ParticleSystem explosionVFX;
    public AudioSource   explosionSFX;

    [Header("Options")]
    public bool explodeOnStart = false;         // فعّليه لو تبين الانفجار بمجرد تفعيل الكائن
    [Range(0f, 1f)] public float xrAmplitude = 1f;

    bool done;

    void Awake()
    {
        // محاولات تلقائية لجلب المراجع إن تُركت فاضية
        if (!explosionVFX) explosionVFX = GetComponent<ParticleSystem>();
        if (!explosionSFX) explosionSFX = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (explodeOnStart)
            Explode();
    }

    /// <summary>تفجير فوري (ينفّذ مرة واحدة فقط)</summary>
    public void Explode()
    {
        if (done) return;
        done = true;

        if (explosionVFX) explosionVFX.Play();
        if (explosionSFX) explosionSFX.Play();

        float d = GetDurationPublic();

        PulseXR(XRNode.LeftHand,  xrAmplitude, d);
        PulseXR(XRNode.RightHand, xrAmplitude, d);

    #if ENABLE_INPUT_SYSTEM
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(xrAmplitude, xrAmplitude);
            StartCoroutine(StopGamepadAfter(d));
        }
    #endif
    }

    /// <summary>تفجير بعد تأخير بالثواني</summary>
    public void ExplodeWithDelay(float delay)
    {
        if (done) return;
        StartCoroutine(CoExplode(delay));
    }
    IEnumerator CoExplode(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    /// <summary>مدة الانفجار (الأطول بين الـVFX والـSFX). لا تقل عن 0.5s.</summary>
    public float GetDurationPublic()
    {
        float t = 0f;
        if (explosionVFX)
        {
            var m = explosionVFX.main;
            t = Mathf.Max(t, m.duration);
        }
        if (explosionSFX && explosionSFX.clip)
            t = Mathf.Max(t, explosionSFX.clip.length);

        if (t <= 0f) t = 0.5f;
        return t;
    }

    void PulseXR(XRNode node, float amp, float dur)
    {
        var dev = InputDevices.GetDeviceAtXRNode(node);
        if (!dev.isValid) return;

        if (dev.TryGetHapticCapabilities(out var caps) && caps.supportsImpulse)
        {
            // channel = 0 لمعظم الكنترولرز
            dev.SendHapticImpulse(0u, Mathf.Clamp01(amp), Mathf.Max(0f, dur));
        }
    }

#if ENABLE_INPUT_SYSTEM
    IEnumerator StopGamepadAfter(float s)
    {
        yield return new WaitForSeconds(s);
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
#endif
}