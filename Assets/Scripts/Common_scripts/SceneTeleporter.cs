using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTeleporter : MonoBehaviour
{
    public string spawnPointName = "SpawnPoint";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SceneLoaderHolder.sceneReady)
        {
            StartCoroutine(TeleportToScene(other.gameObject));
        }
    }

    private IEnumerator TeleportToScene(GameObject player)
    {
        // Activate the scene (was loaded but paused)
        SceneLoaderHolder.asyncOperation.allowSceneActivation = true;

        // Wait until fully activated
        while (!SceneManager.GetSceneByName(SceneLoaderHolder.loadedScene).isLoaded)
        {
            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByName(SceneLoaderHolder.loadedScene);

        // Move player to new scene's spawn point
        foreach (GameObject obj in loadedScene.GetRootGameObjects())
        {
            if (obj.name == spawnPointName)
            {
                player.transform.position = obj.transform.position;
                player.transform.rotation = obj.transform.rotation;
                break;
            }
        }

        // Unload previous scene
        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene.name);

        SceneManager.SetActiveScene(loadedScene);

        // Clean state
        SceneLoaderHolder.sceneReady = false;
        SceneLoaderHolder.loadedScene = "";
        SceneLoaderHolder.asyncOperation = null;
    }
}
