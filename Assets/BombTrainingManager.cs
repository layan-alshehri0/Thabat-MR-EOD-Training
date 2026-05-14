using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class BombTrainingManager : MonoBehaviour
{
    public static BombTrainingManager Instance;

    [Header("UI")]
    public GameObject successPanel;
    public GameObject failPanel;
    public TMP_Text resultText;
    public TMP_Text timerText;
    public TMP_Text scoreText;

    [Header("Audio")]
    public AudioSource cutSound;
    public AudioSource successSound;
    public AudioSource failSound;
    public AudioSource heartbeatSound;

    [Header("Effects")]
    public GameObject explosionEffect;

    [Header("Timer")]
    public float timeLimit = 60f;
    private float currentTime;
    private bool gameEnded = false;

    [Header("Haptics")]
    public XRBaseController leftController;
    public XRBaseController rightController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentTime = timeLimit;

        if (successPanel != null) successPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
        if (explosionEffect != null) explosionEffect.SetActive(false);

        if (heartbeatSound != null)
            heartbeatSound.Play();
    }

    private void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;

        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(currentTime);

        if (currentTime <= 0)
        {
            Fail("Time is over");
        }

        // ضغط نفسي: إذا الوقت أقل من 15 ثانية، النبض يزيد
        if (heartbeatSound != null && currentTime <= 15f)
        {
            heartbeatSound.pitch = 1.5f;
        }
    }

    public void CutWire(string wireColor, bool isSafeWire)
    {
        if (gameEnded) return;

        if (cutSound != null)
            cutSound.Play();

        if (isSafeWire)
        {
            Success(wireColor);
        }
        else
        {
            Fail("Wrong wire: " + wireColor);
        }
    }

    private void Success(string wireColor)
    {
        gameEnded = true;

        if (heartbeatSound != null)
            heartbeatSound.Stop();

        if (successPanel != null)
            successPanel.SetActive(true);

        if (failPanel != null)
            failPanel.SetActive(false);

        if (resultText != null)
            resultText.text = "MISSION SUCCESS\nSafe wire cut: " + wireColor;

        float score = Mathf.Clamp((currentTime / timeLimit) * 100f, 0, 100);

        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.RoundToInt(score) + "%";

        if (successSound != null)
            successSound.Play();

        HapticPulse(0.3f, 0.2f);
    }

    private void Fail(string reason)
    {
        gameEnded = true;

        if (heartbeatSound != null)
            heartbeatSound.Stop();

        if (failPanel != null)
            failPanel.SetActive(true);

        if (successPanel != null)
            successPanel.SetActive(false);

        if (resultText != null)
            resultText.text = "MISSION FAILED\n" + reason;

        if (scoreText != null)
            scoreText.text = "Score: 0%";

        if (failSound != null)
            failSound.Play();

        if (explosionEffect != null)
            explosionEffect.SetActive(true);

        HapticPulse(0.9f, 0.6f);
    }

    private void HapticPulse(float amplitude, float duration)
    {
        if (leftController != null)
            leftController.SendHapticImpulse(amplitude, duration);

        if (rightController != null)
            rightController.SendHapticImpulse(amplitude, duration);
    }
}