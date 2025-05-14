using UnityEngine;

public class GreenRaycastClickHandler : MonoBehaviour
{
    public CanvasLightController canvasController;  // Référence au CanvasLightController

    public float raycastRange = 10f;  // Distance du Raycast

    private Light sceneLight;  // Lumière principale
    private float originalIntensity;  // Intensité originale de la lumière

    private Light[] objectLights1;  // Groupe Fruits
    private Light[] objectLights2;  // Groupe Animaux
    private Light[] objectLights3;  // Groupe Objets

    private bool isGroup1On = false;  // État du groupe Fruits
    private bool isGroup2On = false;  // État du groupe Animaux
    private bool isGroup3On = false;  // État du groupe Objets

    void Start()
    {
        // Trouver le CanvasLightController
        canvasController = FindObjectOfType<CanvasLightController>();

        // // Définir l'ambient light à une couleur gris foncé (RGB: 50, 50, 50)
        // RenderSettings.ambientLight = new Color(50f / 255f, 50f / 255f, 50f / 255f);

        // Trouver la lumière principale
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

        // Définition des groupes d'objets
        string[] objectNames1 = { "bougie", "banane", "feuille" };  // Groupe Fruits
        string[] objectNames2 = { "violette", "cuillere", "orange" };  // Groupe Animaux
        string[] objectNames3 = { "book", "ballon", "potion" };  // Groupe Objets

        // Initialisation des lumières avec des couleurs différentes
        objectLights1 = InitializeLights(objectNames1, Color.yellow);  // Fruits -> Jaune
        objectLights2 = InitializeLights(objectNames2, Color.blue);    // Animaux -> Bleu
        objectLights3 = InitializeLights(objectNames3, Color.green);   // Objets -> Vert
    }

    // Fonction pour initialiser les lumières des objets
    Light[] InitializeLights(string[] objectNames, Color color)
    {
        Light[] lights = new Light[objectNames.Length];

        for (int i = 0; i < objectNames.Length; i++)
        {
            GameObject obj = GameObject.Find(objectNames[i]);
            if (obj != null)
            {
                Light objLight = obj.GetComponentInChildren<Light>();  // Recherche la lumière de l'objet
                if (objLight != null)
                {
                    lights[i] = objLight;
                    objLight.intensity = 0;  // Éteindre par défaut
                    objLight.color = color;  // Appliquer la couleur
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
        // Gérer les clics de souris
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                string objectName = hit.collider.gameObject.name;

                // Si un interrupteur est cliqué, basculer les groupes
                if (objectName == "Interrupteur1")
                {
                    ToggleGroup(ref isGroup1On, objectLights1);
                }
                else if (objectName == "Interrupteur2")
                {
                    ToggleGroup(ref isGroup2On, objectLights2);
                }
                else if (objectName == "Interrupteur3")
                {
                    ToggleGroup(ref isGroup3On, objectLights3);
                }
            }
        }
    }

    // Fonction pour basculer l'état des groupes
    void ToggleGroup(ref bool groupState, Light[] objectLights)
    {
        groupState = !groupState;
        ToggleLights(objectLights, groupState);

        // Modifier l'intensité de la lumière principale
        sceneLight.intensity = (isGroup1On || isGroup2On || isGroup3On) ? 0 : originalIntensity;

        // Afficher un message sur le Canvas
        string message = sceneLight.intensity == 0 ? "💡 Lumière éteinte !" : "💡 Lumière allumée !";

        if (canvasController != null)
        {
            canvasController.ShowCanvas(message);  // Afficher le message sur le Canvas
        }

        Debug.Log(message);
    }

    // Fonction pour basculer les lumières des objets
    void ToggleLights(Light[] lights, bool state)
    {
        foreach (Light objLight in lights)
        {
            if (objLight != null)
            {
                objLight.intensity = state ? 10 : 0;  // Allumer ou éteindre la lumière
                RenderSettings.ambientLight = state ? new Color(50f / 255f, 50f / 255f, 50f / 255f) : Color.white;
            }
        }

        // Modifier l'ambient light en fonction de l'état


        Debug.Log(state ? "Lumières allumées - Ambient Light : Blanche" : "Lumières éteintes - Ambient Light : 50,50,50");
    }

}
