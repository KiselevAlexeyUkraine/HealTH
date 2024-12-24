using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    public int CurrentScene { get => SceneManager.GetActiveScene().buildIndex; }

    //[SerializeField] private GameObject[] cameras;

    private void Awake()
    {
        instance = this;
    }

    // Метод для перехода на сцену по индексу
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }



    // Метод для перехода на следующую сцену
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Проверяем, что следующая сцена существует
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Это последняя сцена в сборке.");
        }
    }

    // Метод для перехода на предыдущую сцену
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;

        // Проверяем, что предыдущая сцена существует
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            Debug.Log("Это первая сцена в сборке.");
        }
    }

    // Метод для перезапуска текущей сцены
    public void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PauseManager.instance.Play();
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Останавливаем игру в редакторе
#endif
    }
}