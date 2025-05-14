using UnityEngine;
public static class SceneLoaderHolder
{
    public static bool sceneReady = false;
    public static string loadedScene = "";
    public static AsyncOperation asyncOperation = null;
}
