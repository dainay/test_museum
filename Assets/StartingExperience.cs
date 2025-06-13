using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoLauncher : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Main_scene";
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(LaunchScene);
        else
            Debug.LogWarning("⛔ Кнопка Start не привязана!");

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        else
            Debug.LogWarning("⛔ Кнопка Quit не привязана!");
    }

    private void LaunchScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void QuitGame()
    {
        Debug.Log("❌ Выход из игры...");
        Application.Quit();
    }
}
