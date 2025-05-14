using UnityEngine;

public class GreenPickupObject : MonoBehaviour
{
    public float raycastRange = 10f; // Distance du Raycast
    public Transform cameraTransform; // Référence de la caméra
    public float holdDistance = 1f; // Distance à laquelle l'objet est tenu

    private GameObject heldObject = null; // Objet actuellement tenu
    private Rigidbody heldObjectRb = null; // Rigidbody de l'objet tenu

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche pour ramasser/lâcher
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }

        if (heldObject != null)
        {
            MoveHeldObject();
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            if (hit.collider.CompareTag("GreenPickup")) // Vérifie si l'objet a le tag "Pickup"
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.useGravity = false; // Désactive la gravité pour éviter la chute
                    heldObjectRb.isKinematic = true; // Empêche les interactions physiques
                }
            }
        }
    }

    void MoveHeldObject()
    {
        // Augmente la distance et ajuste l'offset pour être plus bas et à droite
        Vector3 offset = cameraTransform.right * 2f + cameraTransform.up * -1f;
        float farDistance = 3.5f; // L'objet sera plus loin

        // Nouvelle position de l'objet
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * farDistance + offset;

        // Appliquer la position et la rotation
        heldObject.transform.position = targetPosition;
        heldObject.transform.rotation = cameraTransform.rotation;
    }


    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true; // Réactive la gravité
            heldObjectRb.isKinematic = false; // Permet à l'objet de retomber
        }

        heldObject = null;
        heldObjectRb = null;
    }
}
