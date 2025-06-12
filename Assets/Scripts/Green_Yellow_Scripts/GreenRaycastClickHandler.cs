using UnityEngine;
using System.Collections;

public class GreenRaycastClickHandler : MonoBehaviour
{

    public Camera mainCamera;
    public Camera managerCamera;

    public float raycastRange = 10f;

    private Light sceneLight;
    private Light[] objectLights1;
    private Light[] objectLights2;
    private Light[] objectLights3;

    private bool isGroup1On = false;
    private bool isGroup2On = false;
    private bool isGroup3On = false;

    private Coroutine blinkCoroutine1;
    private Coroutine blinkCoroutine2;
    private Coroutine blinkCoroutine3;

    public Material greenMaterial;
    public Material redMaterial;


    void Start()
    {
       
        GameObject lightObject = GameObject.Find("lightscene");
        if (lightObject != null)
        {
            sceneLight = lightObject.GetComponent<Light>();
            sceneLight.enabled = false;
        }
        else
        {
            Debug.LogWarning("Aucune lumière nommée 'lightscene' trouvée !");
        }

        RenderSettings.ambientLight = new Color(51f / 255f, 58f / 255f, 84f / 255f);

        string[] objectNames1 = { "bougie", "banane", "feuille" };
        string[] objectNames2 = { "violette", "cuillere", "orange" };
        string[] objectNames3 = { "book", "ballon", "potion" };

        objectLights1 = InitializeLights(objectNames1, new Color(0.9f, 0.4f, 0.1f));
        objectLights2 = InitializeLights(objectNames2, new Color(0f, 1f, 1f));
        objectLights3 = InitializeLights(objectNames3, Color.magenta);

          Debug.Log($"[Start] objectLights1 count: {objectLights1.Length}");
    Debug.Log($"[Start] objectLights2 count: {objectLights2.Length}");
    Debug.Log($"[Start] objectLights3 count: {objectLights3.Length}");

    foreach (Light l in objectLights1)
    {
        if (l == null)
            Debug.LogWarning("[Start] Lumière manquante dans objectLights1 !");
    }
    foreach (Light l in objectLights2)
    {
        if (l == null)
            Debug.LogWarning("[Start] Lumière manquante dans objectLights2 !");
    }
    foreach (Light l in objectLights3)
    {
        if (l == null)
            Debug.LogWarning("[Start] Lumière manquante dans objectLights3 !");
    }
    }

    Light[] InitializeLights(string[] objectNames, Color color)
{
    Light[] lights = new Light[objectNames.Length];

    for (int i = 0; i < objectNames.Length; i++)
    {
        GameObject obj = GameObject.Find(objectNames[i]);
        if (obj != null)
        {
            Debug.Log($"[Init] Objet '{objectNames[i]}' trouvé.");

            Light objLight = obj.GetComponentInChildren<Light>();
            if (objLight != null)
            {
                lights[i] = objLight;
                objLight.intensity = 0;
                objLight.color = color;

                Debug.Log($"[Init] Lumière trouvée sur '{objectNames[i]}'. Couleur définie: {color}");
            }
            else
            {
                Debug.LogWarning($"[Init WARNING] Aucune lumière trouvée dans l'objet '{objectNames[i]}' !");
            }
        }
        else
        {
            Debug.LogWarning($"[Init WARNING] Objet '{objectNames[i]}' introuvable !");
        }
    }
    return lights;
}

void SetLightsIntensity(Light[] lights, float intensity)
{
    foreach (Light l in lights)
    {
        if (l != null)
        {
            l.intensity = intensity;
            if (!l.enabled) l.enabled = true;
            Debug.Log($"[Light Check] {l.name} - Intensity: {l.intensity}, Enabled: {l.enabled}, Position: {l.transform.position}");
        }
        else
        {
            Debug.LogWarning("[Light Check WARNING] Lumière null dans le tableau !");
        }
    }
}


   public void SetObjectColor(string objectName, bool isCorrect)
{
    GameObject obj = GameObject.Find(objectName);

    if (obj != null)
    {
        Renderer rend = obj.GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            rend.material = isCorrect ? greenMaterial : redMaterial;
        }
    }
}



    void Update()
    {
        if (sceneLight != null && sceneLight.enabled)
            sceneLight.enabled = false;

        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
{

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
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
        }}
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




    public void ActivateGroupByObject(string objectName)
    {
        if (objectLights1 != null && System.Array.Exists(objectLights1, l => l != null && l.transform.parent.name == objectName))
        {
            if (!isGroup1On)
                ToggleGroup(ref isGroup1On, objectLights1, ref blinkCoroutine1);
        }
        else if (objectLights2 != null && System.Array.Exists(objectLights2, l => l != null && l.transform.parent.name == objectName))
        {
            if (!isGroup2On)
                ToggleGroup(ref isGroup2On, objectLights2, ref blinkCoroutine2);
        }
        else if (objectLights3 != null && System.Array.Exists(objectLights3, l => l != null && l.transform.parent.name == objectName))
        {
            if (!isGroup3On)
                ToggleGroup(ref isGroup3On, objectLights3, ref blinkCoroutine3);
        }
    }

   public void TurnOffLightsForObject(string objectName)
{
    Light[][] lightGroups = { objectLights1, objectLights2, objectLights3 };

    foreach (var group in lightGroups)
    {
        foreach (var light in group)
        {
            if (light != null && light.transform.parent.name == objectName)
            {
                foreach (var lightInGroup in group)
                {
                    if (lightInGroup != null)
                    {
                        lightInGroup.intensity = 0;
                        lightInGroup.enabled = false;
                    }
                }
            }
        }
    }
}


}
