using UnityEngine;
using TMPro;

public class LiveAIHUD : MonoBehaviour
{
    public HybridHandAI ai;

    public TMP_Text statusText;
    public TMP_Text scoreText;
    public TMP_Text speedText;
    public TMP_Text warningsText;

    void Update()
    {
        if (ai == null) return;

        if (statusText != null)
            statusText.text = "Status: " + ai.aiResult;

        if (scoreText != null)
            scoreText.text = "Score: " + ai.score + "/100";

        if (speedText != null)
            speedText.text = "Avg Speed: " + ai.averageSpeed.ToString("0.00");

        if (warningsText != null)
            warningsText.text = "Warnings: " + ai.warningsCount + 
                                " | Danger: " + ai.dangerCount;
    }
}