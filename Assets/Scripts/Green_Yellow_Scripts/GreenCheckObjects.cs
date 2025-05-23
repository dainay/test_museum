using UnityEngine;
using System.Collections.Generic;

public class GreenCheckObjects : MonoBehaviour
{
    private Light spotLight;
    private bool isObjectPlaced = false;

    private static HashSet<string> requiredObjects = new HashSet<string> { "violette", "ballon", "feuille" };
    private static HashSet<string> placedObjects = new HashSet<string>();

    public GreenCameraLookAt cameraScript;

    public Camera mainCamera;
    public Camera victoryCamera;

    void Start()
{
    RenderSettings.ambientLight = Color.white;

    if (mainCamera == null)
        mainCamera = GameObject.Find("MainCamera")?.GetComponent<Camera>();

    if (victoryCamera == null)
        victoryCamera = GameObject.Find("VictoryCamera")?.GetComponent<Camera>();

    if (cameraScript == null)
        cameraScript = FindObjectOfType<GreenCameraLookAt>();

    if (mainCamera == null || victoryCamera == null)
    {
        Debug.LogError(" Les caméras ne sont pas correctement assignées !");
    }

    spotLight = transform.Find("SpotLight")?.GetComponent<Light>();

    if (spotLight == null)
        Debug.LogWarning("⚠️ Aucune lumière 'SpotLight' trouvée !");
    else
        spotLight.intensity = 0;
}


    void OnTriggerStay(Collider other)
    {
        if (spotLight == null || other.gameObject.CompareTag("Player")) return;

        string objName = other.gameObject.name;

        if (requiredObjects.Contains(objName) && !placedObjects.Contains(objName))
        {
            spotLight.intensity = 50;
            spotLight.color = Color.green;
            placedObjects.Add(objName);
            Debug.Log($"{objName} ajouté");
        }
        else if (!requiredObjects.Contains(objName))
        {
            spotLight.intensity = 100;
            spotLight.color = Color.red;
            isObjectPlaced = true;
        }

        CheckWinCondition();
    }

    void OnTriggerExit(Collider other)
    {
        if (spotLight == null) return;

        string objName = other.gameObject.name;

        if (isObjectPlaced)
        {
            isObjectPlaced = false;
            spotLight.intensity = 0;
        }

        placedObjects.Remove(objName);
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (placedObjects.SetEquals(requiredObjects))
        {
            Invoke("Victory", 3f);
        }
    }
void Victory()
{
    RenderSettings.ambientLight = Color.white;

    if (mainCamera != null && victoryCamera != null)
    {
        mainCamera.enabled = false;
        victoryCamera.enabled = true;
        Invoke("ReturnToMainCamera", 3f);
    }
    else
    {
        Debug.LogError("Les caméras ne sont pas correctement assignées !");
    }

    if (cameraScript != null)
    {
        cameraScript.StartCameraRotation();
    }
}

void ReturnToMainCamera()
{
    if (mainCamera != null && victoryCamera != null)
    {
        victoryCamera.enabled = false;
        mainCamera.enabled = true;
    }
}

}
