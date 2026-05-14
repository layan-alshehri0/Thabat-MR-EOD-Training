using UnityEngine;

public class HybridHandAI : MonoBehaviour
{
    public float movement = 0.5f;
    public float speed = 0.5f;
    public float stability = 1f;

    public float normalMovement;
    public float normalSpeed;
    public float normalStability;

    public string aiResult;
    public int score = 100;
    public bool isShaking;
    public int warningsCount;
    public int dangerCount;
    public int maxDangerCount = 5;
    public float averageSpeed;

    public int lastScore;
    public float lastAverageSpeed;
    public int lastWarnings;
    public int lastDangerCount;
    public string lastResult;

    public bool isCalibrated = false;
    public bool evaluationRunning = true;
    public bool missionEnded = false;

    private float speedSum = 0f;
    private int speedSamples = 0;
    private float penaltyTimer = 0f;

    void Start()
    {
        Calibrate();
    }

    void Update()
    {
        if (isCalibrated && evaluationRunning && !missionEnded)
        {
            EvaluateHand();
            CalculateAverageSpeed();
        }
    }

    public void Calibrate()
    {
        normalMovement = Mathf.Max(movement, 0.001f);
        normalSpeed = Mathf.Max(speed, 0.001f);
        normalStability = Mathf.Max(stability, 0.001f);

        isCalibrated = true;
        RestartEvaluation();
    }

    void EvaluateHand()
    {
        float stabilityRatio = stability / normalStability;
        float speedRatio = speed / normalSpeed;
        float movementRatio = movement / normalMovement;

        penaltyTimer += Time.deltaTime;

        if (stabilityRatio >= 0.9f && speedRatio <= 1.2f && movementRatio <= 1.2f)
        {
            aiResult = "Stable ";
            isShaking = false;
        }
        else if (stabilityRatio >= 0.7f && speedRatio <= 1.6f && movementRatio <= 1.6f)
        {
            aiResult = "Warning ";
            isShaking = true;
            ApplyPenalty(1, false);
        }
        else
        {
            aiResult = "Danger ";
            isShaking = true;
            ApplyPenalty(4, true);
        }

        score = Mathf.Clamp(score, 0, 100);
    }

    void ApplyPenalty(int amount, bool isDanger)
    {
        if (penaltyTimer >= 1f)
        {
            score -= amount;
            warningsCount++;

            if (isDanger)
                dangerCount++;

            penaltyTimer = 0f;
        }
    }

    void CalculateAverageSpeed()
    {
        speedSum += speed;
        speedSamples++;

        if (speedSamples > 0)
            averageSpeed = speedSum / speedSamples;
    }

    public void EndMissionSuccess(string message)
    {
        if (missionEnded) return;

        missionEnded = true;
        evaluationRunning = false;
        SaveLastAttempt("SUCCESS");

        if (BombResult.Instance != null)
        {
            BombResult.Instance.Success(
                message + "\n" +
                "Final Score: " + score + "/100\n" +
                "Average Speed: " + averageSpeed.ToString("0.00") + "\n" +
                "Warnings: " + warningsCount + "\n" +
                "Danger Count: " + dangerCount
            );
        }
    }

    public void EndMissionFail(string reason)
    {
        if (missionEnded) return;

        missionEnded = true;
        evaluationRunning = false;
        SaveLastAttempt("FAILED");

        if (BombResult.Instance != null)
        {
            BombResult.Instance.Fail(
                reason + "\n" +
                "Final Score: " + score + "/100\n" +
                "Average Speed: " + averageSpeed.ToString("0.00") + "\n" +
                "Warnings: " + warningsCount + "\n" +
                "Danger Count: " + dangerCount
            );
        }
    }

    void SaveLastAttempt(string result)
    {
        lastResult = result;
        lastScore = score;
        lastAverageSpeed = averageSpeed;
        lastWarnings = warningsCount;
        lastDangerCount = dangerCount;
    }

    public void RestartEvaluation()
    {
        missionEnded = false;
        evaluationRunning = true;

        score = 100;
        warningsCount = 0;
        dangerCount = 0;

        speedSum = 0f;
        speedSamples = 0;
        averageSpeed = 0f;
        penaltyTimer = 0f;

        aiResult = "Stable ";
        isShaking = false;
    }
}