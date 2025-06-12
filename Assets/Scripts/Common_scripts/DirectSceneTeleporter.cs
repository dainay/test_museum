using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DirectSceneTeleporter : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneName;
    public string spawnPointName = "SpawnPoint";

    [Header("Fade & Hint")]
    [SerializeField] private string fadeTag = "Fade";
    [SerializeField] private string hintMessage = "Appuyez sur E ou cliquez pour entrer";
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup fadeCanvasGroup;
    private TextMeshProUGUI hintText;

    private bool playerInTrigger = false;
    public GameObject playerRef;

    private void Start()
    {
        Debug.Log("DirectSceneTeleporter Start method called.");

        GameObject fadeObj = GameObject.FindWithTag(fadeTag);
        if (fadeObj != null)
        {
            fadeCanvasGroup = fadeObj.GetComponent<CanvasGroup>();
            Debug.Log("Fade object found and CanvasGroup assigned.");
        }
        else
        {
            Debug.LogWarning("Fade object not found.");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("Player found.");
            foreach (var text in player.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (text.CompareTag("Hint"))
                {
                    hintText = text;
                    Debug.Log("Hint text component found.");
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }

        if (hintText != null) hintText.text = "";
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (playerInTrigger && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
        {
            Debug.Log("Trigger activated, starting FadeAndTeleport coroutine.");
            if (hintText != null) hintText.text = "";
            StartCoroutine(FadeAndTeleport());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            playerRef = other.gameObject;
            Debug.Log("Player entered the trigger zone.");

            if (hintText != null)
            {
                hintText.text = hintMessage;
                Debug.Log("Hint message displayed.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            playerRef = null;
            Debug.Log("Player exited the trigger zone.");

            if (hintText != null)
            {
                hintText.text = "";
                Debug.Log("Hint message cleared.");
            }
        }
    }

    public IEnumerator FadeAndTeleport()
    {
        Debug.Log("Starting FadeAndTeleport coroutine.");
        yield return StartCoroutine(FadeToBlack());
        yield return StartCoroutine(LoadAndTeleport());
        Debug.Log("FadeAndTeleport coroutine completed.");
    }

    private IEnumerator LoadAndTeleport()
    {
        Debug.Log($"Loading scene: {sceneName}");
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        while (!newScene.isLoaded)
        {
            yield return null;
        }
        Debug.Log($"Scene {sceneName} loaded.");

        GameObject spawn = null;
        foreach (GameObject obj in newScene.GetRootGameObjects())
        {
            if (obj.name == spawnPointName)
            {
                spawn = obj;
                Debug.Log($"Found spawn point: {obj.name}");
                break;
            }
        }

        if (spawn == null)
        {
            Debug.LogError($"Spawn point {spawnPointName} not found in scene {sceneName}.");
        }

        if (playerRef == null)
        {
            Debug.LogError("Player reference is null during teleportation.");
        }

        if (spawn != null && playerRef != null)
        {
            CharacterController controller = playerRef.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            Debug.Log($"Teleporting player to {spawn.transform.position}");
            playerRef.transform.position = spawn.transform.position;
            playerRef.transform.rotation = spawn.transform.rotation;

            if (controller != null) controller.enabled = true;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.SetActiveScene(newScene);
        Debug.Log($"Scene {currentScene.name} unloaded and new scene {newScene.name} activated.");
    }

    private IEnumerator FadeToBlack()
    {
        Debug.Log("Starting fade to black.");
        if (fadeCanvasGroup == null)
        {
            Debug.LogWarning("FadeCanvasGroup is null, skipping fade effect.");
            yield break;
        }

        float time = 0f;
        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
        Debug.Log("Fade to black completed.");
    }
}