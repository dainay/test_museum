using UnityEngine;

public class GreenCameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera greenRoomCamera;
    public KeyCode switchKey = KeyCode.P;

    private bool isGreenRoomActive = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (greenRoomCamera == null)
            greenRoomCamera = GameObject.Find("CameraLookGreenRoom")?.GetComponent<Camera>();

        if (mainCamera == null || greenRoomCamera == null)
        {
            Debug.LogError("caméra manquante");
            return;
        }

        mainCamera.enabled = true;
        greenRoomCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isGreenRoomActive = !isGreenRoomActive;

            mainCamera.enabled = !isGreenRoomActive;
            greenRoomCamera.enabled = isGreenRoomActive;
        }
    }
}
