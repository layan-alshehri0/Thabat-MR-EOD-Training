using UnityEngine;
using TMPro;

public class MissionStartController : MonoBehaviour
{
    public static bool MissionStarted = false;

    public BombTimer bombTimer;
    public GameObject startPanel;
    public GameObject hud;
    public TMP_Text instructionText;

    void Start()
    {
        MissionStarted = false;

        if (hud != null)
            hud.SetActive(false);
    }

    public void StartMission()
    {
        MissionStarted = true;

        if (startPanel != null)
            startPanel.SetActive(false);

        if (hud != null)
            hud.SetActive(true);

        if (instructionText != null)
            instructionText.text = "Mission Active";

        if (bombTimer != null)
            bombTimer.StartTimer();

        Debug.Log("MISSION STARTED");
    }
}