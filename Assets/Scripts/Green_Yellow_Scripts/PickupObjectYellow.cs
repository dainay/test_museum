using UnityEngine;

public class PickupObjectYellow : MonoBehaviour
{
    public float raycastRange = 10f; 
    public Transform cameraTransform;
    public float holdDistance = 10f; 
    public Transform sign1Transform;
    public Transform sign2Transform;
    public Transform sign3Transform;
    public Transform sign4Transform;
    public GameObject player;

    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;
    public float maxDistanceToSign = 3f;
    private bool isObjectPlacedOnSign = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic détecté!");

            if (heldObject == null)
                TryPickupObject();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            KeepObjectCentered();
            PlaceObjectOnSignIfClose();
        }

        if (isObjectPlacedOnSign)
        {
            TryPickupObjectFromSign();
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

       
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * raycastRange, Color.red, 1.5f);

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.name); 

            if (hit.collider.CompareTag("PickupYellow"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.useGravity = false;
                    heldObjectRb.isKinematic = true;

                    Debug.Log("Objet ramassé: " + heldObject.name); 
                }
                else
                {
                    Debug.LogWarning("L'objet n'a pas de Rigidbody attaché!");
                }
            }
            else
            {
                Debug.LogWarning("Raycast ne touche pas un objet avec le tag 'PickupYellow'.");
            }
        }
        else
        {
            Debug.LogWarning("Le Raycast n'a pas touché d'objet.");
        }
    }

void KeepObjectCentered()
{
    Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance;

   
    targetPosition += cameraTransform.up *-0.5f; 


    heldObject.transform.position = targetPosition;

 
    Quaternion lookRotation = Quaternion.LookRotation(cameraTransform.forward);
    heldObject.transform.rotation = lookRotation * Quaternion.Euler(-40, 180, 0);
}



    void PlaceObjectOnSignIfClose()
    {
        // Vérifie la distance avec chaque panneau pour placer l'objet
        float distanceToSign1 = Vector3.Distance(heldObject.transform.position, sign1Transform.position);
        float distanceToSign2 = Vector3.Distance(heldObject.transform.position, sign2Transform.position);
        float distanceToSign3 = Vector3.Distance(heldObject.transform.position, sign3Transform.position);
        float distanceToSign4 = Vector3.Distance(heldObject.transform.position, sign4Transform.position);

        if (distanceToSign1 <= maxDistanceToSign)
        {
            PlaceOnSign(sign1Transform);
        }
        else if (distanceToSign2 <= maxDistanceToSign)
        {
            PlaceOnSign(sign2Transform);
        }
        else if (distanceToSign3 <= maxDistanceToSign)
        {
            PlaceOnSign(sign3Transform);
        }
        else if (distanceToSign4 <= maxDistanceToSign)
        {
            PlaceOnSign(sign4Transform);
        }
    }

    void PlaceOnSign(Transform signTransform)
    {
        heldObject.transform.position = signTransform.position;
        heldObject.transform.rotation = signTransform.rotation;

        heldObjectRb.useGravity = false;
        heldObjectRb.isKinematic = true;

        isObjectPlacedOnSign = true;
        heldObject = null;
        heldObjectRb = null;
        Debug.Log("Objet placé sur le panneau: " + signTransform.name); 
    }

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;
            heldObjectRb.isKinematic = false;
        }

        Debug.Log("Objet lâché: " + heldObject.name); 
        heldObject = null;
        heldObjectRb = null;
    }

    void TryPickupObjectFromSign()
    {
        float distanceToSign1 = Vector3.Distance(cameraTransform.position, sign1Transform.position);
        float distanceToSign2 = Vector3.Distance(cameraTransform.position, sign2Transform.position);
        float distanceToSign3 = Vector3.Distance(cameraTransform.position, sign3Transform.position);
        float distanceToSign4 = Vector3.Distance(cameraTransform.position, sign4Transform.position);

        if (distanceToSign1 <= maxDistanceToSign || distanceToSign2 <= maxDistanceToSign || distanceToSign3 <= maxDistanceToSign || distanceToSign4 <= maxDistanceToSign)
        {
            RaycastHit hit;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                if (hit.collider.CompareTag("PickupYellow"))
                {
                    heldObject = hit.collider.gameObject;
                    heldObjectRb = heldObject.GetComponent<Rigidbody>();

                    if (heldObjectRb != null)
                    {
                        heldObjectRb.useGravity = false;
                        heldObjectRb.isKinematic = true;
                    }

                    isObjectPlacedOnSign = false;
                }
            }
        }
    }
}
