using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject modeModalUI;
    [SerializeField] private GameObject modePanel;
    [SerializeField] private GameObject volumePanel;
    [SerializeField] private GameObject volumeToggleUI;
    [SerializeField] private Sprite volumeOnSprite;
    [SerializeField] private Sprite volumeOffSprite;
    [SerializeField] private Image volumeToggleImage;
    [SerializeField] private TextMeshProUGUI modeText;
    [SerializeField] private Slider volumeSlider;

    [Header("Scene Transition Settings")]
    [SerializeField] private string fadeTag = "Fade";
    [SerializeField] private string hintTextTag = "Hint";
    [SerializeField] private float fadeDuration = 1f;

    public string spawnPointClassique = "SpawnPointClassique";
    public string spawnPointImmersive = "SpawnPointImmersive";

    private bool isMuted = false;
    private ModeManager.MuseumMode modeToSwitch;
    private Coroutine currentSceneChangeCoroutine = null;
    private CanvasGroup fadeCanvasGroup;
    private TextMeshProUGUI hintText;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        modeModalUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UpdateVolumeToggleImage();
        UpdateModeText();

        // Initialize fade and hint text objects
        GameObject fadeObj = GameObject.FindWithTag(fadeTag);
        if (fadeObj != null) fadeCanvasGroup = fadeObj.GetComponent<CanvasGroup>();

        GameObject hintObj = GameObject.FindWithTag(hintTextTag);
        if (hintObj != null) hintText = hintObj.GetComponent<TextMeshProUGUI>();

        if (hintText != null) hintText.text = "";
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;

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

        if (!pauseMenuUI.activeSelf)
        {
            modeModalUI.SetActive(false);
            volumeToggleUI.SetActive(true);
            modePanel.SetActive(true);
            volumePanel.SetActive(true);

            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        modeModalUI.SetActive(false);
        modePanel.SetActive(true);
        volumePanel.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleModeModal()
    {
        Debug.Log("ToggleModeModal called");
        modeToSwitch = ModeManager.Instance.CurrentMode == ModeManager.MuseumMode.Classic ? ModeManager.MuseumMode.Interactive : ModeManager.MuseumMode.Classic;
        volumeToggleUI.SetActive(false);
        modePanel.SetActive(false);
        volumePanel.SetActive(false);
        modeModalUI.SetActive(true);
    }

    public void CloseModeModal()
    {
        modeModalUI.SetActive(false);
        volumeToggleUI.SetActive(true);
        modePanel.SetActive(true);
        volumePanel.SetActive(true);
    }

    public void ConfirmModeSwitch()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        if (currentSceneChangeCoroutine != null)
        {
            StopCoroutine(currentSceneChangeCoroutine);
            Debug.Log("Previous scene change coroutine stopped.");
        }

        string targetSceneName;
        string targetSpawnPoint;

        if (modeToSwitch == ModeManager.MuseumMode.Classic)
        {
            Debug.Log("Switching to Classic mode.");
            ModeManager.Instance.SetMode(ModeManager.MuseumMode.Interactive);
            targetSceneName = "SalleClassique";
            targetSpawnPoint = spawnPointClassique;
        }
        else if (modeToSwitch == ModeManager.MuseumMode.Interactive)
        {
            Debug.Log("Switching to Immersive mode.");
            ModeManager.Instance.SetMode(ModeManager.MuseumMode.Classic);
            targetSceneName = "Main_scene";
            targetSpawnPoint = spawnPointImmersive;
        }
        else
        {
            Debug.LogError("Unrecognized mode.");
            return;
        }

        Debug.Log($"Loading scene: {targetSceneName} with spawn point: {targetSpawnPoint}");
        currentSceneChangeCoroutine = StartCoroutine(HandleSceneChange(targetSceneName, targetSpawnPoint, player));

        ResumeGame();
    }

    private IEnumerator HandleSceneChange(string sceneName, string spawnPointName, GameObject player)
    {
        yield return StartCoroutine(FadeToBlack());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        if (!newScene.IsValid() || !newScene.isLoaded)
        {
            Debug.LogError("Failed to load the new scene.");
            yield break;
        }

        // Find the spawn point in the new scene
        GameObject spawnPoint = null;
        foreach (GameObject obj in newScene.GetRootGameObjects())
        {
            if (obj.name == spawnPointName)
            {
                spawnPoint = obj;
                break;
            }
        }

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;
        }
        else
        {
            Debug.LogError("Spawn point not found in the new scene.");
        }

        // Unload the previous scene
        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene.name);

        // Set the new scene as active
        SceneManager.SetActiveScene(newScene);

        yield return StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeCanvasGroup == null) yield break;

        float time = 0f;
        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeFromBlack()
    {
        if (fadeCanvasGroup == null) yield break;

        float time = 0f;
        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }

    public void CancelModeSwitch()
    {
        CloseModeModal();
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
            volumeToggleImage.sprite = AudioListener.volume == 0f ? volumeOffSprite : volumeOnSprite;
        }
    }

    private void UpdateModeText()
    {
        if (modeText != null)
        {
            modeText.text = ModeManager.Instance.CurrentMode == ModeManager.MuseumMode.Interactive ? "Classique" : "Immersif";
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        UpdateVolumeToggleImage();
        isMuted = volume == 0f;
    }
}