using UnityEngine;

public class PickupObjectYellow : MonoBehaviour
{
    public float raycastRange = 10f; // Distance du Raycast
    public Transform cameraTransform; // Référence de la caméra
    public float holdDistance = 10f; // Distance à laquelle l'objet est tenu
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
            }
        }
    }

    void KeepObjectCentered()
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance + cameraTransform.up * 2f;
        heldObject.transform.position = targetPosition;

        Quaternion lookRotation = Quaternion.LookRotation(cameraTransform.forward);
        heldObject.transform.rotation = lookRotation * Quaternion.Euler(-40, 180, 0);
    }

    void PlaceObjectOnSignIfClose()
    {
        float distanceToSign1 = Vector3.Distance(heldObject.transform.position, sign1Transform.position);
        float distanceToSign2 = Vector3.Distance(heldObject.transform.position, sign2Transform.position);
        float distanceToSign3 = Vector3.Distance(heldObject.transform.position, sign3Transform.position);
        float distanceToSign4 = Vector3.Distance(heldObject.transform.position, sign4Transform.position);

        if (distanceToSign1 <= maxDistanceToSign)
        {
            PlaceOnSign(sign1Transform);
            TeleportPlayerToSign(sign1Transform);
        }
        else if (distanceToSign2 <= maxDistanceToSign)
        {
            PlaceOnSign(sign2Transform);
            TeleportPlayerToSign(sign2Transform);
        }
        else if (distanceToSign3 <= maxDistanceToSign)
        {
            PlaceOnSign(sign3Transform);
            TeleportPlayerToSign(sign3Transform);
        }
        else if (distanceToSign4 <= maxDistanceToSign)
        {
            PlaceOnSign(sign4Transform);
            TeleportPlayerToSign(sign4Transform);
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
    }

    void TeleportPlayerToSign(Transform signTransform)
    {
        if (player != null)
        {
            player.transform.position = signTransform.position + new Vector3(0, 1, 0);
        }
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

    void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;
            heldObjectRb.isKinematic = false;
        }

        heldObject = null;
        heldObjectRb = null;
    }
}
