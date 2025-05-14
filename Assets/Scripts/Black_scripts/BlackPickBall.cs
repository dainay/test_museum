using UnityEngine;

public class BlackPickBall : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;
    [SerializeField] private Transform hand; // Assign hand transform in Inspector
    private GameObject currentBall;
    private bool isHoldingBall = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            if (isHoldingBall)
                Drop();
            else
                PickUp();
        }
    }

    void PickUp()
    {
        if (hand == null)
        {
            Debug.LogError("❌ PickBall: Hand Transform is not assigned!");
            return;
        }

        GameObject hitObject = raycasterManager.GetRaycastHit();
        if (hitObject != null && hitObject.CompareTag("Ball"))
        {
            Debug.Log("🎯 Picked up ball: " + hitObject.name);

            currentBall = hitObject;
            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();

            ballRb.isKinematic = true;
            ballRb.useGravity = false;

            currentBall.transform.SetParent(hand);
            currentBall.transform.localPosition = Vector3.zero;
            isHoldingBall = true;
        }
    }

    void Drop()
    {
        if (currentBall)
        {
            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
            }

            currentBall.transform.SetParent(null);
            currentBall = null;
            isHoldingBall = false;
        }
    }
}
