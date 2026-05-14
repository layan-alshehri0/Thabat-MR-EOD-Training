using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMission : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}