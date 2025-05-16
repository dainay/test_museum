using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneTeleporter : MonoBehaviour
{
    [Header("Scene Settings")]
    public string spawnPointName = "SpawnPoint";

    [Header("Fade & Hint (by Tag)")]
    [SerializeField] private string fadeTag = "Fade";
    [SerializeField] private string hintTextTag = "Hint";
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup fadeCanvasGroup;
    private TextMeshProUGUI hintText;

    private bool playerInTrigger = false;
    private GameObject playerRef;

    private void Start()
    {
        // Находим нужные элементы по тегам
        GameObject fadeObj = GameObject.FindWithTag(fadeTag);
        if (fadeObj != null) fadeCanvasGroup = fadeObj.GetComponent<CanvasGroup>();

        GameObject hintObj = GameObject.FindWithTag(hintTextTag);
        if (hintObj != null) hintText = hintObj.GetComponent<TextMeshProUGUI>();

        // Инициализация
        if (hintText != null) hintText.text = "";
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (playerInTrigger && SceneLoaderHolder.sceneReady && Input.GetKeyDown(KeyCode.E))
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

            if (SceneLoaderHolder.sceneReady && hintText != null)
            {
                hintText.text = "Appuyez sur E pour entrer";
            }
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
        yield return StartCoroutine(TeleportToScene(playerRef));
    }

    private IEnumerator TeleportToScene(GameObject player)
    {
        SceneLoaderHolder.asyncOperation.allowSceneActivation = true;

        Scene loadedScene;
        do
        {
            loadedScene = SceneManager.GetSceneByName(SceneLoaderHolder.loadedScene);
            yield return null;
        } while (!loadedScene.isLoaded);

        GameObject spawn = null;
        foreach (GameObject obj in loadedScene.GetRootGameObjects())
        {
            if (obj.name == spawnPointName)
            {
                spawn = obj;
                break;
            }
        }

        if (spawn != null)
        {
            player.transform.position = spawn.transform.position;
            player.transform.rotation = spawn.transform.rotation;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene.name);
        SceneManager.SetActiveScene(loadedScene);

        SceneLoaderHolder.sceneReady = false;
        SceneLoaderHolder.loadedScene = "";
        SceneLoaderHolder.asyncOperation = null;
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
