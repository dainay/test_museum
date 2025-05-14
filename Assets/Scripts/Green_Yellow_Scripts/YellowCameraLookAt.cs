using UnityEngine;

public class YellowCameraLookAt : MonoBehaviour
{
    public Transform target; // L'objet vers lequel tourner
    public float rotationSpeed = 2.0f;

    private bool shouldRotate = false;
    private Quaternion targetRotation;
    private bool allowUserControl = false;

    public GameObject objectToDrop; // L'objet à faire tomber
    public float groundY = 5.0f; // Hauteur du sol
    private bool isFalling = false;

    void Start()
    {
        // Cache la boule au début
        if (objectToDrop != null)
        {
            objectToDrop.SetActive(false);
        }
    }

    void Update()
    {
        if (allowUserControl)
            return;

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
                Debug.Log("🔻 L'objet a touché le sol, gravité désactivée !");
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

    // À appeler depuis YellowGameManager quand victoire
    public void OnYellowRoomVictory()
    {
        if (objectToDrop != null)
        {
            objectToDrop.SetActive(true); // Montre la boule
        }

        StartCameraRotation();
    }
}
