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
    private GameObject playerRef;

    private void Start()
    {
        GameObject fadeObj = GameObject.FindWithTag(fadeTag);
        if (fadeObj != null) fadeCanvasGroup = fadeObj.GetComponent<CanvasGroup>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            foreach (var text in player.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (text.CompareTag("Hint"))
                {
                    hintText = text;
                    break;
                }
            }
        }

        if (hintText != null) hintText.text = "";
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (playerInTrigger && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
        {
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

            if (hintText != null)
                hintText.text = hintMessage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            playerRef = null;
            if (hintText != null) hintText.text = "";
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        yield return StartCoroutine(FadeToBlack());
        yield return StartCoroutine(LoadAndTeleport());
    }

    private IEnumerator LoadAndTeleport()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        while (!newScene.isLoaded)
            yield return null;

        GameObject spawn = null;
        foreach (GameObject obj in newScene.GetRootGameObjects())
        {
            if (obj.name == spawnPointName)
            {
                spawn = obj;
                break;
            }
        }

        if (spawn != null && playerRef != null)
        {
            CharacterController controller = playerRef.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            playerRef.transform.position = spawn.transform.position;
            playerRef.transform.rotation = spawn.transform.rotation;

            if (controller != null) controller.enabled = true;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.SetActiveScene(newScene);
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
}
