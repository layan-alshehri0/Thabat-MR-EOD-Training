using System.Collections;
using UnityEngine;

public class NarrationOnShow : MonoBehaviour
{
    public AudioSource narration;   // اسحبي AudioSource هنا
    public bool playEveryTime = true; // يشغل كل مرة تظهر فيها الواجهة؟
    public float delay = 0f;          // تأخير قبل التشغيل (اختياري)

    bool playedOnce;

    void OnEnable()
    {
        if (!narration) return;
        if (!playEveryTime && playedOnce) return;

        playedOnce = true;
        if (delay > 0) StartCoroutine(PlayAfterDelay());
        else narration.Play();
    }

    void OnDisable()
    {
        if (narration && narration.isPlaying) narration.Stop();
    }

    IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        narration.Play();
    }
}
