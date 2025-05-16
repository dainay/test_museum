using UnityEngine;

public class GreenCameraLookAt : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2.0f;
    private bool shouldRotate = false;

    private Quaternion targetRotation;
    private bool allowUserControl = false; 
    public GameObject objectToDrop; 
    public float fallSpeed = 5f; 

    private bool isFalling = false;

    void Update()
    {
        if (allowUserControl)
        {
            return;
        }

        if (shouldRotate && target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                shouldRotate = false;
                Debug.Log("🎥 Rotation terminée !");
                Invoke("AllowUserControl", 0.5f);
                StartObjectFall();
            }
        }
    }

    public void StartCameraRotation()
    {
        Debug.Log("🎥 La caméra commence à tourner !");
        shouldRotate = true;
    }


    void AllowUserControl()
    {
        allowUserControl = true;
        Debug.Log("🎮 Contrôle caméra rétabli !");
    }

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
            objectToDrop.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            if (objectToDrop.transform.position.y <= 0f)
            {
                objectToDrop.transform.position = new Vector3(objectToDrop.transform.position.x, 0f, objectToDrop.transform.position.z);
                isFalling = false;
                Debug.Log("🔻 L'objet a touché le sol !");
            }
        }
    }
}
