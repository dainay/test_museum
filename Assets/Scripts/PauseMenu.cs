using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject modeModalUI;
    [SerializeField] private GameObject volumeToggleUI;
    [SerializeField] private Sprite volumeOnSprite;
    [SerializeField] private Sprite volumeOffSprite;
    [SerializeField] private Image volumeToggleImage;
    [SerializeField] private TextMeshProUGUI modeText;
    [SerializeField] private Slider volumeSlider;

    private bool isMuted = false;
    private ModeManager.MuseumMode modeToSwitch;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpdateVolumeToggleImage();
        UpdateModeText();

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
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
        modeModalUI.SetActive(false);

        if (pauseMenuUI.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleModeModal()
    {
        Debug.Log("ToggleModeModal called"); // Log de débogage
        modeToSwitch = ModeManager.Instance.CurrentMode == ModeManager.MuseumMode.Classic ? ModeManager.MuseumMode.Interactive : ModeManager.MuseumMode.Classic;
        modeModalUI.SetActive(true);
    }

    public void CloseModeModal()
    {
        modeModalUI.SetActive(false);
    }

    public void ConfirmModeSwitch()
    {
        if (modeToSwitch == ModeManager.MuseumMode.Classic)
        {
            ModeManager.Instance.SetMode(ModeManager.MuseumMode.Classic);
            SceneManager.LoadSceneAsync("SalleClassique");
        }
        else if (modeToSwitch == ModeManager.MuseumMode.Interactive)
        {
            ModeManager.Instance.SetMode(ModeManager.MuseumMode.Interactive);
            SceneManager.LoadSceneAsync("Main_scene");
        }
        modeModalUI.SetActive(false);
        UpdateModeText();
    }

    public void CancelModeSwitch()
    {
        modeModalUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleVolume()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : volumeSlider.value;
        UpdateVolumeToggleImage();
    }

    private void UpdateVolumeToggleImage()
    {
        if (volumeToggleImage != null)
        {
            volumeToggleImage.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
        }
    }

    private void UpdateModeText()
    {
        if (modeText != null)
        {
            modeText.text = ModeManager.Instance.CurrentMode == ModeManager.MuseumMode.Classic ? "Immersif" : "Classique";
        }
    }

    public void SetVolume(float volume)
    {
        if (isMuted)
        {
            isMuted = false;
            UpdateVolumeToggleImage();
        }
        AudioListener.volume = volume;
    }
}