using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TimerSFX : MonoBehaviour
{
    [Header("Refs")]
    public GameLogic logic;

    [Header("Clips")]
    public AudioClip tickingLoop;     // تكتكة قصيرة تتكرر يدويًا
    public AudioClip successJingle;   // اختياري
    public AudioClip failBoom;        // اختياري

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false; // نعيد تشغيلها يدويًا
    }

    void OnEnable()
    {
        if (logic != null)
        {
            logic.OnTimerStart += HandleStart;
            logic.OnDefuseEnd  += HandleEnd;
        }
    }

    void OnDisable()
    {
        if (logic != null)
        {
            logic.OnTimerStart -= HandleStart;
            logic.OnDefuseEnd  -= HandleEnd;
        }
    }

    void HandleStart()
    {
        if (!tickingLoop) return;
        StopAllCoroutines();
        StartCoroutine(PlayTickingLoop());
    }

    System.Collections.IEnumerator PlayTickingLoop()
    {
        while (logic != null && logic.IsRunning())
        {
            src.clip = tickingLoop;
            // تسريع طفيف حسب الزمن (اختياري)
            if (logic != null) src.pitch = 1f + Mathf.Clamp01(logic.GetElapsed() / 60f) * 0.2f;
            src.Play();
            yield return new WaitForSeconds(Mathf.Max(0.01f, tickingLoop.length - 0.01f));
        }
    }

    void HandleEnd(bool success, float elapsed)
    {
        StopAllCoroutines();
        src.Stop();
        if (success && successJingle) src.PlayOneShot(successJingle);
        else if (!success && failBoom) src.PlayOneShot(failBoom);
    }
}
