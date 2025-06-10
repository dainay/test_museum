using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawnHandler : MonoBehaviour
{
    private string targetSpawnPointName = "SpawnPoint_Default";

    public void SetSpawnPointName(string name)
    {
        targetSpawnPointName = name;
        Debug.Log($"📌 Установлена новая точка появления: {targetSpawnPointName}");
    }

    void Awake()
    {
        Debug.Log("🌀 [PlayerSpawnHandler] Awake вызван");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        Debug.Log("🛑 [PlayerSpawnHandler] OnDestroy вызван — отписка от события");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"📥 [PlayerSpawnHandler] Сцена загружена: {scene.name}, запускаем MoveToSpawn()");
        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator MoveToSpawn()
    {
        Debug.Log("⏳ Ждём 1 кадр перед поиском точки входа...");
        yield return null;

        Debug.Log($"🔎 Ищем точку входа: {targetSpawnPointName}");
        GameObject spawn = GameObject.Find(targetSpawnPointName);
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            transform.rotation = spawn.transform.rotation;
            Debug.Log($"✅ Игрок перемещён в: {targetSpawnPointName}");
        }
        else
        {
            Debug.LogWarning($"⚠️ Не удалось найти точку входа: {targetSpawnPointName}");
        }
    }
}
