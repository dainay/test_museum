using UnityEngine;

public class GreenCameraLookAt : MonoBehaviour
{
    public Transform target; // 🎯 L'objet vers lequel tourner (boîte en bois)
    public float rotationSpeed = 2.0f; // 🌪️ Vitesse de rotation
    private bool shouldRotate = false;

    private Quaternion targetRotation;
    private bool allowUserControl = false; // Flag pour permettre à l'utilisateur de reprendre le contrôle
    public GameObject objectToDrop; // L'objet qui va tomber (à assigner dans l'Inspector)
    public float fallSpeed = 5f; // Vitesse de chute de l'objet

    private bool isFalling = false; // Flag pour savoir si l'objet est en train de tomber

    void Update()
    {
        // Si l'utilisateur a le contrôle, on ne fait rien
        if (allowUserControl)
        {
            return;
        }

        if (shouldRotate && target != null)
        {
            // 🔄 Calculer la rotation cible
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Garde la caméra droite (évite qu'elle regarde trop en haut/bas)
            targetRotation = Quaternion.LookRotation(direction);

            // 🎥 Rotation fluide
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // ✅ Arrêt de la rotation quand elle est proche de la cible
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                shouldRotate = false;
                Debug.Log("🎥 Rotation terminée !");

                // 🎮 Reprend le contrôle de la caméra pour l'utilisateur
                Invoke("AllowUserControl", 0.5f); // Un petit délai pour éviter une transition trop abrupte

                // Commence la chute de l'objet après la rotation
                StartObjectFall();
            }
        }
    }

    public void StartCameraRotation()
    {
        Debug.Log("🎥 La caméra commence à tourner !");
        shouldRotate = true;
    }

    // Permet à l'utilisateur de reprendre le contrôle après la rotation
    void AllowUserControl()
    {
        allowUserControl = true;
        Debug.Log("🎮 Contrôle caméra rétabli !");
    }

    // Déclenche la chute de l'objet
    void StartObjectFall()
    {
        if (objectToDrop != null)
        {
            isFalling = true;
            Debug.Log("🔻 L'objet commence à tomber !");
        }
    }

    void FixedUpdate()
    {
        if (isFalling && objectToDrop != null)
        {
            // Appliquer une chute vers le bas
            objectToDrop.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // Optionnel : Si tu veux arrêter l'objet lorsqu'il touche le sol (en utilisant une hauteur minimale)
            if (objectToDrop.transform.position.y <= 0f) // suppose que le sol est à y=0
            {
                objectToDrop.transform.position = new Vector3(objectToDrop.transform.position.x, 0f, objectToDrop.transform.position.z);
                isFalling = false;
                Debug.Log("🔻 L'objet a touché le sol !");
            }
        }
    }
}
