using UnityEngine;

public class YellowCameraLookAt : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2.0f;

    public float groundY = 5.0f;
    private bool isFalling = false;

    public GameObject objectToDrop;

    void Start()
    {
        if (objectToDrop != null)
            objectToDrop.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isFalling && objectToDrop != null)
        {
            if (objectToDrop.transform.position.y <= groundY + 0.05f)
            {
                Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }

                objectToDrop.transform.position = new Vector3(
                    objectToDrop.transform.position.x,
                    groundY,
                    objectToDrop.transform.position.z
                );

                isFalling = false;
                Debug.Log("🔻 L'objet a touché le sol !");
            }
        }
    }

    void StartObjectFall()
    {
        if (objectToDrop != null)
        {
            Rigidbody rb = objectToDrop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            isFalling = true;
            Debug.Log("🔻 L'objet commence à tomber !");
        }
    }

    public void DropObjectNow()
    {
        if (objectToDrop != null)
            objectToDrop.SetActive(true);

        StartObjectFall();
    }
}
