using UnityEngine;
using TMPro;

public class BombTimer : MonoBehaviour
{
    public float timeLeft = 60f;
    public TMP_Text timerText;

    private bool running = false;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            running = false;

            AdaptiveTrainingManager adaptive =
                FindObjectOfType<AdaptiveTrainingManager>();

            if (adaptive != null)
            {
                adaptive.RegisterTimeFail();
            }
            else
            {
                BombResult.Instance.Fail("Time Over");
            }
        }

        UpdateUI();
    }

    public void StartTimer()
    {
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }

    void UpdateUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text =
            minutes.ToString("00") + ":" +
            seconds.ToString("00");
    }
}