using UnityEngine;
using System.Collections;

public class GreenRaycastClickHandler : MonoBehaviour
{
    public CanvasLightController canvasController;

    public float raycastRange = 10f; 

    private Light sceneLight; 
    private float originalIntensity; 

    private Light[] objectLights1; 
    private Light[] objectLights2; 
    private Light[] objectLights3;

    private bool isGroup1On = false;
    private bool isGroup2On = false;
    private bool isGroup3On = false;

    private Coroutine blinkCoroutine1;
    private Coroutine blinkCoroutine2;
    private Coroutine blinkCoroutine3;

    void Start()
    {
        canvasController = FindObjectOfType<CanvasLightController>();
        GameObject lightObject = GameObject.Find("lightscene");
        if (lightObject != null)
        {
            sceneLight = lightObject.GetComponent<Light>();
            originalIntensity = sceneLight.intensity;
        }
        else
        {
            Debug.LogWarning("Aucune lumière nommée 'lightscene' trouvée !");
        }
        string[] objectNames1 = { "bougie", "banane", "feuille" }; 
        string[] objectNames2 = { "violette", "cuillere", "orange" }; 
        string[] objectNames3 = { "book", "ballon", "potion" }; 

        objectLights1 = InitializeLights(objectNames1, Color.yellow);
        objectLights2 = InitializeLights(objectNames2, Color.blue); 
        objectLights3 = InitializeLights(objectNames3, Color.green);
    }

    Light[] InitializeLights(string[] objectNames, Color color)
    {
        Light[] lights = new Light[objectNames.Length];

        for (int i = 0; i < objectNames.Length; i++)
        {
            GameObject obj = GameObject.Find(objectNames[i]);
            if (obj != null)
            {
                Light objLight = obj.GetComponentInChildren<Light>(); 
                if (objLight != null)
                {
                    lights[i] = objLight;
                    objLight.intensity = 0;
                    objLight.color = color;
                }
                else
                {
                    Debug.LogWarning($"Aucune lumière trouvée dans '{objectNames[i]}' !");
                }
            }
            else
            {
                Debug.LogWarning($"Aucun objet nommé '{objectNames[i]}' trouvé !");
            }
        }
        return lights;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                string objectName = hit.collider.gameObject.name;
                if (objectName == "Interrupteur1")
                {
                    ToggleGroup(ref isGroup1On, objectLights1, ref blinkCoroutine1);
                }
                else if (objectName == "Interrupteur2")
                {
                    ToggleGroup(ref isGroup2On, objectLights2, ref blinkCoroutine2);
                }
                else if (objectName == "Interrupteur3")
                {
                    ToggleGroup(ref isGroup3On, objectLights3, ref blinkCoroutine3);
                }
            }
        }
    }

    void ToggleGroup(ref bool groupState, Light[] objectLights, ref Coroutine blinkCoroutine)
    {
        groupState = !groupState;

        if (groupState)
        {
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(BlinkLights(objectLights));
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            SetLightsIntensity(objectLights, 0);
        }

        bool anyGroupOn = isGroup1On || isGroup2On || isGroup3On;

        if (sceneLight != null)
        {
            if (anyGroupOn)
            {
                sceneLight.enabled = false;
            }
            else
            {
                sceneLight.enabled = true;
                sceneLight.intensity = originalIntensity;
            }
        }

        RenderSettings.ambientLight = anyGroupOn
    ? new Color(68f / 255f, 70f / 255f, 82f / 255f)
    : new Color(174f / 255f, 176f / 255f, 202f / 255f);

        string message = !sceneLight.enabled ? "Lumière éteinte" : "Lumière allumée";

        if (canvasController != null)
        {
            canvasController.ShowCanvas(message);
        }

        Debug.Log(message);
    }

    IEnumerator BlinkLights(Light[] lights)
    {
        float duration = 1.5f;
        float maxIntensity = 20f;
        float minIntensity = 0.5f;

        while (true)
        {

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float intensity = Mathf.Lerp(minIntensity, maxIntensity, t / duration);
                SetLightsIntensity(lights, intensity);
                yield return null;
            }
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float intensity = Mathf.Lerp(maxIntensity, minIntensity, t / duration);
                SetLightsIntensity(lights, intensity);
                yield return null;
            }
        }
    }

    void SetLightsIntensity(Light[] lights, float intensity)
    {
        foreach (Light l in lights)
        {
            if (l != null)
                l.intensity = intensity;
        }
    }
}
