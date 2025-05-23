using UnityEngine;

public class GreenPickupObject : MonoBehaviour
{
    private float raycastRange = 10f;
    private float holdDistance = 2f;

    private Transform cameraTransform; 
    private GameObject heldObject;
    private Rigidbody heldObjectRb;

 void Awake()
{
    cameraTransform = GameObject.Find("MainCamera")?.transform;

    if (cameraTransform == null)
    {
        Debug.LogError("Pas de mainCamera");
    }
}

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
            MoveHeldObject();
    }

   private void TryPickupObject()
{
    Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
    if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
    {
        if (hit.collider.CompareTag("GreenPickup"))
        {
            PickUp(hit.collider.gameObject);
        }
    }
}


    private void PickUp(GameObject objectToPickup)
    {
        heldObject = objectToPickup;
        heldObjectRb = heldObject.GetComponent<Rigidbody>();
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = false;
            heldObjectRb.isKinematic = true;
        }
    }
    private void MoveHeldObject()
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance + GetObjectOffset();
        heldObject.transform.position = targetPosition;
        heldObject.transform.rotation = cameraTransform.rotation;
    }

    private Vector3 GetObjectOffset()
    {
        return cameraTransform.right * 1.5f + cameraTransform.up * -1f;
    }

    private void DropObject()
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
