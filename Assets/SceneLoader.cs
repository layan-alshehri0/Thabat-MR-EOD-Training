using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // تحميل مشهد باسم
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // تحميل مشهد برقم
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // إعادة تحميل المشهد الحالي
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // خروج من اللعبة
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}