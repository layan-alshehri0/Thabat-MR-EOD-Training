using UnityEngine;
using TMPro;

public class GameResultUI : MonoBehaviour
{
    public static GameResultUI Instance;

    public GameObject successPanel;
    public GameObject failPanel;

    public TMP_Text successText;
    public TMP_Text failText;

    public AudioSource successSound;
    public AudioSource failSound;

    private void Awake()
    {
        Instance = this;

        if (successPanel != null) successPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(false);
    }

    public void ShowSuccess(string wireName)
    {
        successPanel.SetActive(true);
        failPanel.SetActive(false);

        successText.text = "✅ Success! Correct wire cut: " + wireName;

        if (successSound != null) successSound.Play();
    }

    public void ShowFail(string wireName)
    {
        failPanel.SetActive(true);
        successPanel.SetActive(false);

        failText.text = "💥 Failed! Wrong wire cut: " + wireName;

        if (failSound != null) failSound.Play();
    }
}