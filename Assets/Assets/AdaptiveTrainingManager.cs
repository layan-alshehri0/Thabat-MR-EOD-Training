using UnityEngine;
using TMPro;
using System.Collections;

public class AdaptiveTrainingManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject instructionPanel;
    public TMP_Text instructionText;
    public GameObject hudPanel;
    public GameObject successPanel;
    public GameObject trainingFailPanel;
    public TMP_Text trainingFailText;

    [Header("Bomb Levels")]
    public GameObject bombLevel1;
    public GameObject bombLevel2;
    public GameObject bombLevel3;

    [Header("Place Bomb In Front")]
    public Transform playerCamera;
    public float bombDistance = 1.0f;
    public float bombHeightOffset = -0.25f;

    [Header("Buttons")]
    public GameObject continueButton;
    public GameObject restartButton;

    [Header("UI")]
    public TMP_Text levelText;
    public TMP_Text finalReportText;

    [Header("Final Report")]
    private float[] levelTimes = new float[3];
    private int[] levelScores = new int[3];
    private int[] levelWarnings = new int[3];
    private int[] levelDanger = new int[3];

    [Header("Timer")]
    public BombTimer timerUI;
    public float instructionDuration = 3f;
    public float level1Time = 60f;
    public float level2Time = 40f;
    public float level3Time = 25f;

    [Header("AI")]
    public HybridHandAI handAI;
    public float shakyStabilityThreshold = 0.5f;

    [Header("Effects")]
    public Light stressLight;
    public AudioSource heartbeatSound;
    public AudioSource warningSound;

    public int currentLevel = 1;
    public int maxLevel = 3;

    void Start()
    {
        HideAll();

        if (startPanel != null)
            startPanel.SetActive(true);

        if (finalReportText != null)
            finalReportText.text = "";
    }

    void HideAll()
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (instructionPanel != null) instructionPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (successPanel != null) successPanel.SetActive(false);
        if (trainingFailPanel != null) trainingFailPanel.SetActive(false);

        if (continueButton != null) continueButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);

        if (bombLevel1 != null) bombLevel1.SetActive(false);
        if (bombLevel2 != null) bombLevel2.SetActive(false);
        if (bombLevel3 != null) bombLevel3.SetActive(false);

        if (heartbeatSound != null) heartbeatSound.Stop();
        if (warningSound != null) warningSound.Stop();
    }

    public void BeginLevel()
    {
        HideAll();

        if (finalReportText != null)
            finalReportText.text = "";

        StartCoroutine(ShowInstructionThenStart());
    }

    IEnumerator ShowInstructionThenStart()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(true);

        if (instructionText != null)
        {
            if (currentLevel == 1)
                instructionText.text =
                    "Level 1 - Training\nIdentify the safe wire.\nFocus on hand stability.";

            else if (currentLevel == 2)
                instructionText.text =
                    "Level 2 - Pressure\nThe safe wire is harder to identify.\nStay calm.";

            else if (currentLevel == 3)
                instructionText.text =
                    "Level 3 - Critical\nThe safe wire is hidden among similar wires.\nHigh precision required.";
        }

        yield return new WaitForSeconds(instructionDuration);

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        StartCurrentLevel();
    }

    void StartCurrentLevel()
    {
        if (hudPanel != null)
            hudPanel.SetActive(true);

        if (bombLevel1 != null)
            bombLevel1.SetActive(currentLevel == 1);

        if (bombLevel2 != null)
            bombLevel2.SetActive(currentLevel == 2);

        if (bombLevel3 != null)
            bombLevel3.SetActive(currentLevel == 3);

        GameObject activeBomb = null;

        if (currentLevel == 1) activeBomb = bombLevel1;
        if (currentLevel == 2) activeBomb = bombLevel2;
        if (currentLevel == 3) activeBomb = bombLevel3;

        PlaceBombInFront(activeBomb);

        if (handAI != null)
            handAI.RestartEvaluation();

        if (currentLevel == 1)
        {
            if (timerUI != null)
                timerUI.timeLeft = level1Time;

            if (levelText != null)
                levelText.text = "Level 1 - Training";

            if (stressLight != null)
            {
                stressLight.color = Color.white;
                stressLight.intensity = 0.5f;
            }

            if (heartbeatSound != null)
                heartbeatSound.Stop();

            if (warningSound != null)
                warningSound.Stop();
        }
        else if (currentLevel == 2)
        {
            if (timerUI != null)
                timerUI.timeLeft = level2Time;

            if (levelText != null)
                levelText.text = "Level 2 - Pressure";

            if (stressLight != null)
            {
                stressLight.color = Color.yellow;
                stressLight.intensity = 2.5f;
            }

            if (heartbeatSound != null)
                heartbeatSound.Play();

            if (warningSound != null)
                warningSound.Play();
        }
        else if (currentLevel == 3)
        {
            if (timerUI != null)
                timerUI.timeLeft = level3Time;

            if (levelText != null)
                levelText.text = "Level 3 - Critical";

            if (stressLight != null)
            {
                stressLight.color = Color.red;
                stressLight.intensity = 5f;
            }

            if (heartbeatSound != null)
                heartbeatSound.Play();

            if (warningSound != null)
                warningSound.Play();
        }

        if (timerUI != null)
            timerUI.StartTimer();
    }

    void PlaceBombInFront(GameObject bomb)
    {
        if (bomb == null || playerCamera == null)
            return;

        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        bomb.transform.position =
            playerCamera.position +
            forward * bombDistance +
            Vector3.up * bombHeightOffset;

        bomb.transform.rotation =
            Quaternion.LookRotation(forward);
    }

    public void RegisterCorrectCut()
    {
        SaveLevelPerformance();

        StopLevel();

        if (successPanel != null)
            successPanel.SetActive(true);

        if (currentLevel < maxLevel)
        {
            currentLevel++;

            if (continueButton != null)
                continueButton.SetActive(true);

            if (restartButton != null)
                restartButton.SetActive(false);

            if (finalReportText != null)
            {
                finalReportText.text =
                    "LEVEL PASSED\n\nPress Next Level to continue.";
            }
        }
        else
        {
            if (continueButton != null)
                continueButton.SetActive(false);

            if (restartButton != null)
                restartButton.SetActive(true);

            ShowFinalReport();
        }
    }

    public void RegisterWrongCut()
    {
        string reason;

        if (handAI != null &&
            handAI.stability < shakyStabilityThreshold)
        {
            reason =
                "Level " + currentLevel + " Failed\n" +
                "Reason: High hand shaking detected.\n" +
                "Recommendation: More stability training is required.";
        }
        else
        {
            reason =
                "Level " + currentLevel + " Failed\n" +
                "Reason: Wrong wire selected.\n" +
                "Recommendation: More accuracy training is required.";
        }

        ShowFail(reason);
    }

    public void RegisterTimeFail()
    {
        ShowFail(
            "Level " + currentLevel + " Failed\n" +
            "Reason: Time over.\n" +
            "Recommendation: Improve speed under pressure."
        );
    }

    void ShowFail(string message)
    {
        StopLevel();

        if (trainingFailPanel != null)
            trainingFailPanel.SetActive(true);

        if (trainingFailText != null)
            trainingFailText.text = message;
    }

    void StopLevel()
    {
        if (timerUI != null)
            timerUI.StopTimer();

        if (hudPanel != null)
            hudPanel.SetActive(false);

        if (heartbeatSound != null)
            heartbeatSound.Stop();

        if (warningSound != null)
            warningSound.Stop();

        if (handAI != null)
        {
            handAI.evaluationRunning = false;
            handAI.missionEnded = true;
        }
    }

    void SaveLevelPerformance()
    {
        int index = currentLevel - 1;

        if (index < 0 || index >= 3)
            return;

        float levelDuration = 0f;

        if (currentLevel == 1)
            levelDuration = level1Time;

        else if (currentLevel == 2)
            levelDuration = level2Time;

        else if (currentLevel == 3)
            levelDuration = level3Time;

        if (timerUI != null)
            levelTimes[index] =
                levelDuration - timerUI.timeLeft;

        if (handAI != null)
        {
            levelScores[index] = handAI.score;
            levelWarnings[index] = handAI.warningsCount;
            levelDanger[index] = handAI.dangerCount;
        }
    }

    void ShowFinalReport()
    {
        if (finalReportText == null)
            return;

        float totalTime =
            levelTimes[0] +
            levelTimes[1] +
            levelTimes[2];

        int averageScore =
            Mathf.RoundToInt(
                (levelScores[0] +
                 levelScores[1] +
                 levelScores[2]) / 3f);

        int totalWarnings =
            levelWarnings[0] +
            levelWarnings[1] +
            levelWarnings[2];

        int totalDanger =
            levelDanger[0] +
            levelDanger[1] +
            levelDanger[2];

        finalReportText.text =
            "TRAINING COMPLETED\n" +
            "MISSION SUCCESSFUL\n\n" +

            "Level 1:\n" +
            "Time: " + levelTimes[0].ToString("0.00") + "s\n" +
            "Score: " + levelScores[0] + "/100\n" +
            "Warnings: " + levelWarnings[0] + "\n" +
            "Danger: " + levelDanger[0] + "\n\n" +

            "Level 2:\n" +
            "Time: " + levelTimes[1].ToString("0.00") + "s\n" +
            "Score: " + levelScores[1] + "/100\n" +
            "Warnings: " + levelWarnings[1] + "\n" +
            "Danger: " + levelDanger[1] + "\n\n" +

            "Level 3:\n" +
            "Time: " + levelTimes[2].ToString("0.00") + "s\n" +
            "Score: " + levelScores[2] + "/100\n" +
            "Warnings: " + levelWarnings[2] + "\n" +
            "Danger: " + levelDanger[2] + "\n\n" +

            "Final Summary:\n" +
            "Total Time: " + totalTime.ToString("0.00") + "s\n" +
            "Average Score: " + averageScore + "/100\n" +
            "Total Warnings: " + totalWarnings + "\n" +
            "Total Danger: " + totalDanger;
    }
}