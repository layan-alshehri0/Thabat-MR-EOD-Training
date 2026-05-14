using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [Header("Run / Timer")]
    public float timeLimit = 60f;    // حد الوقت (اختياري، لو تبين انفجار تلقائي عند إنتهاء الوقت)
    public bool explodeOnTimeout = false;

    // أحداث يحتاجها بقية السكربتات
    public event Action OnTimerStart;                 // تطلبها TimerSFX
    public event Action<float> OnTick;                // لو تبين تيك كل فريم (اختياري لTimerSFX)
    public event Action<bool, float> OnDefuseEnd;     // يستخدمها LeaderboardManager

    bool running = false;
    float elapsed = 0f;

    void Update()
    {
        if (!running) return;

        elapsed += Time.deltaTime;
        OnTick?.Invoke(elapsed);

        if (explodeOnTimeout && elapsed >= timeLimit)
        {
            Explode();
        }
    }

    // يُستدعى من CoverOpener عند أول إمساك للغطاء
    public void StartRun()
    {
        if (running) return;
        running = true;
        elapsed = 0f;
        OnTimerStart?.Invoke();
    }

    // لإعادة التشغيل من البداية (يُستدعى من LeaderboardManager.PlayAgain)
    public void ResetRun()
    {
        running = false;
        elapsed = 0f;
    }

    // يعلن نجاح (تستدعيه DefuseSequence عبر UnityEvent OnSuccess)
    public void Defuse()
    {
        if (!running) return;
        running = false;
        OnDefuseEnd?.Invoke(true, elapsed);
    }

    // يعلن فشل/انفجار (تستدعيه DefuseSequence عبر UnityEvent OnFail أو بسبب المهلة)
    public void Explode()
    {
        if (!running) return;
        running = false;
        OnDefuseEnd?.Invoke(false, elapsed);
    }

    // تواقيع مطلوبة من TimerSFX/DebugHotkeys
    public bool IsRunning() => running;
    public float GetElapsed() => elapsed;
}
