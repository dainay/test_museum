using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SimpleDoorSceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private string spawnPointName = "SpawnPoint";
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 1f;

    private bool playerInTrigger = false;

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
        }
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (promptText != null)
                promptText.gameObject.SetActive(false);

            StartCoroutine(FadeAndLoadScene());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (promptText != null)
            {
                promptText.text = "Appuyez sur E";
                promptText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        yield return StartCoroutine(FadeToBlack());

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.Find(spawnPointName);

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
            player.transform.rotation = spawn.transform.rotation;
            Debug.Log($"🧭 Перемещён в: {spawnPointName}");
        }
        else
        {
            Debug.LogWarning("⚠️ Не найден игрок или SpawnPoint после загрузки сцены");
        }
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeGroup != null)
        {
            fadeGroup.blocksRaycasts = true;

            float time = 0f;
            while (time < fadeDuration)
            {
                fadeGroup.alpha = time / fadeDuration;
                time += Time.deltaTime;
                yield return null;
            }

            fadeGroup.alpha = 1f;
        }
    }
}
