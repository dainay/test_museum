using UnityEngine;

public class GreenCameraLookAt : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 2.0f;
    private bool shouldRotate = false;
    private Quaternion targetRotation;
    private bool allowUserControl = false;
    public GameObject objectToAppear; 

    void Start()
{
    objectToAppear.SetActive(false);
    if (objectToAppear != null)
    {
        objectToAppear.SetActive(false);
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
                ShowObject();
            }
        }
    }


    void AllowUserControl()
    {
        allowUserControl = true;
    }

 public void ShowObject()
{
    if (objectToAppear != null)
    {
        objectToAppear.SetActive(true);
    }
}

}
