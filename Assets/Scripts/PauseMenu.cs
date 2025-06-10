using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject modeModalUI;
    [SerializeField] private GameObject volumeToggleUI;
    [SerializeField] private Sprite volumeOnSprite;
    [SerializeField] private Sprite volumeOffSprite;
    [SerializeField] private Image volumeToggleImage;

    private bool isMuted = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        // Ensure the cursor is hidden and locked at the start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize the volume toggle image
        UpdateVolumeToggleImage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        bool isPaused = pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(!isPaused);

        if (pauseMenuUI.activeSelf)
        {
            Time.timeScale = 0f; // Pause the game
            // Show and unlock the cursor when the game is paused
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            // Hide and lock the cursor when the game is resumed
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenuUI.SetActive(false);
        // Hide and lock the cursor when the game is resumed
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleModal()
    {
        bool isModalActive = modeModalUI.activeSelf;
        modeModalUI.SetActive(!isModalActive);
    }

    public void ModeClassique()
    {
        SceneManager.LoadSceneAsync("SalleClassique");
    }

    public void ModeImmersif()
    {
        SceneManager.LoadSceneAsync("SalleImmersive");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleVolume()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateVolumeToggleImage();
    }

    private void UpdateVolumeToggleImage()
    {
        if (volumeToggleImage != null)
        {
            volumeToggleImage.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
        }
    }
}