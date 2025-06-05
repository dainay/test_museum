using UnityEngine;
using System.Collections.Generic;

public class GreenCheckObjects : MonoBehaviour
{
    private Light spotLight;
    private bool isObjectPlaced = false;

    public Material defaultMaterial;
    public GreenRaycastClickHandler raycastClickHandler;

    private static HashSet<string> requiredObjects = new HashSet<string> { "violette", "ballon", "feuille" };
    private static HashSet<string> placedObjects = new HashSet<string>();

    public GreenCameraLookAt cameraScript;
    public CanvasLightController canvasController;

    public Camera mainCamera;
    public Camera victoryCamera;

    public Material greenMaterial;
    public Material redMaterial;

    private Dictionary<string, string> objectToTarget = new Dictionary<string, string>
    {
        { "ballon", "Cylindre1" },
        { "potion", "Cylindre1" },
        { "book", "Cylindre1" },
        { "violette", "Cylindre2" },
        { "cuillere", "Cylindre2" },
        { "orange", "Cylindre2" },
        { "bougie", "Cylindre3" },
        { "banane", "Cylindre3" },
        { "feuille", "Cylindre3" }
    };

    void Start()
    {
        raycastClickHandler = FindObjectOfType<GreenRaycastClickHandler>();
        canvasController = FindObjectOfType<CanvasLightController>();
        RenderSettings.ambientLight = Color.white;

        if (mainCamera == null)
            mainCamera = GameObject.Find("MainCamera")?.GetComponent<Camera>();

        if (victoryCamera == null)
            victoryCamera = GameObject.Find("VictoryCamera")?.GetComponent<Camera>();

        if (cameraScript == null)
            cameraScript = FindObjectOfType<GreenCameraLookAt>();

        if (mainCamera == null || victoryCamera == null)
            Debug.LogError("Les caméras ne sont pas correctement assignées !");

        spotLight = transform.Find("SpotLight")?.GetComponent<Light>();
        if (spotLight != null)
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
            raycastClickHandler?.TurnOffLightsForObject(objName); 

            SetTargetObjectMaterial(objName, true);
            canvasController?.ShowCanvas("Bravo, vous avez trouvé l'objet");
            ScoreManager.Instance?.AddPoint();

            CheckWinCondition();
        }
        else if (!requiredObjects.Contains(objName))
        {
            SetTargetObjectMaterial(objName, false);
            canvasController?.ShowCanvas("Ce n'est pas un objet attendu !");
            spotLight.intensity = 50;
            spotLight.color = Color.red;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (spotLight == null) return;

        string objName = other.gameObject.name;

        if (objectToTarget.TryGetValue(objName, out string targetName))
        {
            ApplyMaterialToTarget(targetName, defaultMaterial);
        }

        spotLight.intensity = 0;
        spotLight.color = Color.white;

        if (requiredObjects.Contains(objName) && placedObjects.Contains(objName))
        {
            placedObjects.Remove(objName);
            ScoreManager.Instance?.RemovePoint();
        }

        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (placedObjects.SetEquals(requiredObjects))
        {
            Invoke("Victory", 0f);
        }
    }

    void Victory()
    {
        RenderSettings.ambientLight = new Color(68f / 255f, 70f / 255f, 82f / 255f);

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
            cameraScript.ShowObject();
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

    private void ApplyMaterialToTarget(string targetName, Material material)
    {
        GameObject target = GameObject.Find(targetName);

        if (target != null)
        {
            Renderer rend = target.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material = material;
            }
            else
            {
                Debug.LogWarning($"Pas de Renderer trouvé dans l'objet cible {targetName}");
            }
        }
        else
        {
            Debug.LogWarning($"Objet cible {targetName} introuvable !");
        }
    }

    private void SetTargetObjectMaterial(string placedObjectName, bool correct)
    {
        if (objectToTarget.TryGetValue(placedObjectName, out string targetName))
        {
            ApplyMaterialToTarget(targetName, correct ? greenMaterial : redMaterial);
        }
        else
        {
            Debug.LogWarning($"Aucun cylindre mappé pour l'objet {placedObjectName}");
        }
    }
}
