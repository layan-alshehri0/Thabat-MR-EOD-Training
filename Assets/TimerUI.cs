using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;   // اسحبي TimerText هنا
    float elapsed;
    bool running;
    bool hiddenAtStart;

    public void ResetTimer(bool startHidden = true)
    {
        elapsed = 0f;
        running = true;
        hiddenAtStart = startHidden;
        if (timerText) timerText.gameObject.SetActive(!startHidden);
        if (timerText && !startHidden) timerText.text = "0.00s";
    }

    void Update()
    {
        if (!running) return;
        elapsed += Time.deltaTime;
        if (timerText && !hiddenAtStart)
            timerText.text = $"{elapsed:0.00}s";
    }

    public void ShowFinalTime()
    {
        running = false;
        hiddenAtStart = false;
        if (timerText) {
            timerText.gameObject.SetActive(true);
            timerText.text = $"{elapsed:0.00}s";
        }
    }

    public float GetTime() => elapsed;
}
