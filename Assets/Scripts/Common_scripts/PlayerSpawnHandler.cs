using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawnHandler : MonoBehaviour
{
    private string targetSpawnPointName = "SpawnPoint_Default";

    public void SetSpawnPointName(string name)
    {
        targetSpawnPointName = name;
        Debug.Log($"new spawn point {targetSpawnPointName}");
    }

    void Awake()
    {
        Debug.Log("[PlayerSpawnHandler] Awake called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        Debug.Log("[PlayerSpawnHandler] OnDestroy called");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PlayerSpawnHandler] scene loaded {scene.name}, start MoveToSpawn()");
        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator MoveToSpawn()
    {
        Debug.Log("wait 1 frame before moving to spawn point");
        yield return null;

        Debug.Log($"loor for entrance point {targetSpawnPointName}");
        GameObject spawn = GameObject.Find(targetSpawnPointName);
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            transform.rotation = spawn.transform.rotation;
            Debug.Log($"player is moved to  {targetSpawnPointName}");
        }
        else
        {
            Debug.LogWarning($"entrance point is not found {targetSpawnPointName}");
        }
    }
}
