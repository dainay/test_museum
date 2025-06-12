using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SimpleDoorSceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private string spawnPointName = "SpawnPoint";
    [SerializeField] private float fadeDuration = 1f;

    private TextMeshProUGUI promptText;
    private CanvasGroup fadeGroup;
    private bool playerInTrigger = false;
    private bool isLoading = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 🔍 Ищем объект с тегом "Hint" внутри игрока
            Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);
            foreach (var child in allChildren)
            {
                if (child.CompareTag("Hint"))
                {
                    promptText = child.GetComponent<TextMeshProUGUI>();
                    if (promptText != null)
                    {
                        Debug.Log("✅ Найдена подсказка внутри Player");
                        promptText.gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }

        GameObject fadeObj = GameObject.FindGameObjectWithTag("Fade");
        if (fadeObj != null)
        {
            fadeGroup = fadeObj.GetComponent<CanvasGroup>();
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
        }
    }

    void Update()
    {
        if (playerInTrigger && !isLoading && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
        {
            Debug.Log("🎬 Вход выполнен");
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
                promptText.text = "Appuyez sur E ou cliquez gauche pour entrer";
                promptText.gameObject.SetActive(true);
                Debug.Log("🟢 Подсказка показана");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;

            if (promptText != null)
            {
                promptText.gameObject.SetActive(false);
                Debug.Log("🔴 Подсказка скрыта");
            }
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isLoading = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var spawnHandler = player?.GetComponent<PlayerSpawnHandler>();
        if (spawnHandler != null)
        {
            spawnHandler.SetSpawnPointName(spawnPointName);
        }

        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene(nextSceneName);
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
