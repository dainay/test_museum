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

    void Start()
    {
        GameObject hintObj = GameObject.FindGameObjectWithTag("Hint");
        GameObject fadeObj = GameObject.FindGameObjectWithTag("Fade");

        if (hintObj != null)
            promptText = hintObj.GetComponent<TextMeshProUGUI>();

        if (fadeObj != null)
            fadeGroup = fadeObj.GetComponent<CanvasGroup>();

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
        // передаём имя точки игроку
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
