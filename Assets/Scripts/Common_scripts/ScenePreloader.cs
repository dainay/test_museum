using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenePreloader : MonoBehaviour
{
    public string targetScene;
    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until almost done
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Now it's loaded, but not activated
        SceneLoaderHolder.sceneReady = true;
        SceneLoaderHolder.loadedScene = targetScene;
        SceneLoaderHolder.asyncOperation = asyncLoad;
    }
}
