using UnityEngine;
using System.Collections.Generic;

public class GreenCheckObjects : MonoBehaviour
{
    private Light spotLight;
    private bool isObjectPlaced = false;

    private static HashSet<string> requiredObjects = new HashSet<string> { "violette", "ballon", "feuille" };
    private static HashSet<string> placedObjects = new HashSet<string>();

    public GreenCameraLookAt cameraScript; 

    void Start()
    {

        RenderSettings.ambientLight = Color.white;

        spotLight = transform.Find("SpotLight")?.GetComponent<Light>();

        if (spotLight == null)
        {
            Debug.LogWarning("⚠️ Aucune lumière 'SpotLight' trouvée !");
        }
        else
        {
            spotLight.intensity = 0;
        }

        if (cameraScript == null)
        {
            Debug.LogError("CameraLookAt n'est pas assigné dans CheckObjects !");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (spotLight == null) return;

        string objName = other.gameObject.name;

   
        if (other.gameObject.CompareTag("Player")) return;

        
        if (requiredObjects.Contains(objName))
        {
            
            if (!placedObjects.Contains(objName))
            {
                spotLight.intensity = 50;
                spotLight.color = Color.green;  
                placedObjects.Add(objName);
                Debug.Log($"{objName} ajouté ");
            }
        }
        else
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
            Debug.Log("Victoire");
            Invoke("Victory", 3f);
        }
    }

    void Victory()
    {

        RenderSettings.ambientLight = Color.white;

        if (cameraScript != null)
        {
            cameraScript.StartCameraRotation();
        }
        else
        {
            Debug.LogError("La caméra n'est pas assignée dans l'Inspector !");
        }
    }

}
