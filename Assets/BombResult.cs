using UnityEngine;
using TMPro;

public class BombResult : MonoBehaviour
{
    public static BombResult Instance;

    public GameObject successPanel;
    public GameObject failPanel;
    public GameObject hud;

    public TMP_Text resultText;
    public BombTimer timer;

    public bool gameEnded = false;

    void Awake()
    {
        Instance = this;
        gameEnded = false;

        if (successPanel != null)
            successPanel.SetActive(false);

        if (failPanel != null)
            failPanel.SetActive(false);
    }

    public void Success(string message)
    {
        if (gameEnded) return;
        gameEnded = true;

        if (timer != null)
            timer.StopTimer();

        if (hud != null)
            hud.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(true);

        if (failPanel != null)
            failPanel.SetActive(false);

        if (resultText != null)
            resultText.text =
                "TRAINING COMPLETED\n" +
                "MISSION SUCCESSFUL\n\n" +
                message;
    }

    public void Fail(string reason)
    {
        if (gameEnded) return;
        gameEnded = true;

        if (timer != null)
            timer.StopTimer();

        if (hud != null)
            hud.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(false);

        if (failPanel != null)
            failPanel.SetActive(true);

        if (resultText != null)
            resultText.text =
                "TRAINING FAILED\n\n" +
                reason;
    }
}