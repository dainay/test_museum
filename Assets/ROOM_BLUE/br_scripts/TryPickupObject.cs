// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TryPickupObject : MonoBehaviour
// {
//     private void TryPickupObject()
//     {
//     Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
//     if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, pickupLayerMask))
//     {
//         if (hit.collider.CompareTag("GreenPickup"))
//         {
//             PickUp(hit.collider.gameObject);
//         }
//     }
//     }

//     private void PickUp(GameObject objectToPickup)
//     {
//         heldObject = objectToPickup;
//         heldObjectRb = heldObject.GetComponent<Rigidbody>();
//         if (heldObjectRb != null)
//         {
//             heldObjectRb.useGravity = false;
//             heldObjectRb.isKinematic = true;
//         }
//     }
//     private void MoveHeldObject()
//     {
//         Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance + GetObjectOffset();
//         heldObject.transform.position = targetPosition;
//         heldObject.transform.rotation = cameraTransform.rotation;
//     }

//     private Vector3 GetObjectOffset()
//     {
//     return cameraTransform.right * 1f + cameraTransform.up * -0.8f;
//     }
    
//     private void DropObject()
//     {
//         if (heldObjectRb != null)
//         {
//             heldObjectRb.useGravity = true;
//             heldObjectRb.isKinematic = false;
//         }
//         heldObject = null;
//         heldObjectRb = null;
//     }
// }