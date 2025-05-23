using UnityEngine;

public class GreenCameraLookAt : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2.0f;
    private bool shouldRotate = false;

    private Quaternion targetRotation;
    private bool allowUserControl = false;

    public GameObject objectToDrop;
    public float fallSpeed = 50f;

    private bool isFalling = false;

    void Start()
    {
        GameObject playerCameraRoot = GameObject.Find("PlayerCameraRoot");
        if (playerCameraRoot != null)
        {
            target = playerCameraRoot.transform;
        }
        else
        {
            Debug.LogError("PlayerCameraRoot introuvable dans la scène !");
        }
    }

    void Update()
    {
        if (allowUserControl) return;

        if (shouldRotate && target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                shouldRotate = false;
    
                Invoke("AllowUserControl", 0.5f);
                StartObjectFall();
            }
        }
    }

    public void StartCameraRotation()
    {
    
        shouldRotate = true;
         StartObjectFall();
    }

    void AllowUserControl()
    {
        allowUserControl = true;
   
    }
void StartObjectFall()
{
    if (objectToDrop != null)
    {
        Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.velocity = new Vector3(0, -fallSpeed, 0);
        }
        isFalling = false;

    }
}


}
