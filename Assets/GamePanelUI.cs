using UnityEngine;
using TMPro;

public class GamePanelUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public GameObject gamePanel;
    public GameObject instructionPanel;

    public void OnSubmitName()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("ًWrite your name");
            return;
        }

        PlayerInfo.PlayerName = playerName;

        gamePanel.SetActive(false);
        instructionPanel.SetActive(true);
    }
}
