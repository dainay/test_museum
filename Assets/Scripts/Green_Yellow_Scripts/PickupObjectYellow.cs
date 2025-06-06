using UnityEngine;

public class PickupObjectYellow : MonoBehaviour
{
    public float raycastRange = 10f;
    [SerializeField] public float holdDistance = 50f;

    public Transform sign1Transform;
    public Transform sign2Transform;
    public Transform sign3Transform;
    public Transform sign4Transform;

    public float maxDistanceToSign = 3f;

    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;
    private bool isObjectPlacedOnSign = false;

    void Update()
    {
        Transform cam = GetPlayerCamera();
        if (cam == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryPickupObject(cam);
            else
                DropObject();
        }

        if (heldObject != null)
        {
            KeepObjectCentered(cam);
            PlaceObjectOnSignIfClose();
        }

        if (isObjectPlacedOnSign)
        {
            TryPickupObjectFromSign(cam);
        }
    }

    Transform GetPlayerCamera()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Camera cam = player.GetComponentInChildren<Camera>();
            if (cam != null)
                return cam.transform;
        }
        return null;
    }

    void TryPickupObject(Transform cam)
    {
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
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

    void KeepObjectCentered(Transform cam)
    {
       Vector3 targetPosition = cam.position + cam.forward * holdDistance + cam.right * 1f + cam.up * -0.4f;         

        heldObject.transform.position = targetPosition;

        Quaternion lookRotation = Quaternion.LookRotation(cam.forward);
        heldObject.transform.rotation = lookRotation * Quaternion.Euler(-40, 180, 0);
    }

    void PlaceObjectOnSignIfClose()
    {
        float d1 = Vector3.Distance(heldObject.transform.position, sign1Transform.position);
        float d2 = Vector3.Distance(heldObject.transform.position, sign2Transform.position);
        float d3 = Vector3.Distance(heldObject.transform.position, sign3Transform.position);
        float d4 = Vector3.Distance(heldObject.transform.position, sign4Transform.position);

        if (d1 <= maxDistanceToSign) PlaceOnSign(sign1Transform);
        else if (d2 <= maxDistanceToSign) PlaceOnSign(sign2Transform);
        else if (d3 <= maxDistanceToSign) PlaceOnSign(sign3Transform);
        else if (d4 <= maxDistanceToSign) PlaceOnSign(sign4Transform);
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

    void TryPickupObjectFromSign(Transform cam)
    {
        float d1 = Vector3.Distance(cam.position, sign1Transform.position);
        float d2 = Vector3.Distance(cam.position, sign2Transform.position);
        float d3 = Vector3.Distance(cam.position, sign3Transform.position);
        float d4 = Vector3.Distance(cam.position, sign4Transform.position);

        if (d1 <= maxDistanceToSign || d2 <= maxDistanceToSign || d3 <= maxDistanceToSign || d4 <= maxDistanceToSign)
        {
            Ray ray = new Ray(cam.position, cam.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
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
