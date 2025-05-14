using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TryFocusLayout : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;

    [SerializeField] private Camera mainCamera;       // Камера Starter Assets
    [SerializeField] private Camera layoutCamera;     // Вторая камера на layout

    private bool isInLayoutMode = false;

    [SerializeField] private GameObject videoPlayerObject;      // объект с VideoPlayer
    [SerializeField] private RawImage staticPreviewImage;        // RawImage с картинкой
    [SerializeField] private RawImage videoDisplayImage;         // RawImage с видео RenderTexture

    private VideoPlayer videoPlayer;

    [SerializeField] private GameObject interviewCanvas; // Canvas with the interview (assign in Inspector)


    void Start()
    {
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();

        // Начальное состояние
        videoPlayerObject.SetActive(false);
        staticPreviewImage.enabled = true;
        videoDisplayImage.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hit = raycasterManager.GetRaycastHit();
            if (hit != null && hit.CompareTag("Layout"))
            {
                EnterLayoutMode();
            }
        }

        if (isInLayoutMode && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ExitLayoutMode();
        }
    }

    void EnterLayoutMode()
    {
        staticPreviewImage.enabled = false;
        videoDisplayImage.enabled = true;

        videoPlayerObject.SetActive(true);
        videoPlayer.Play();

        layoutCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);

        ShowCursor();
        isInLayoutMode = true;

        Debug.Log("🖼 Layout camera activated");
    }

    void ExitLayoutMode()
    {
        videoPlayer.Stop();
        videoPlayerObject.SetActive(false);

        videoDisplayImage.enabled = false;
        staticPreviewImage.enabled = true;

        layoutCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        if (interviewCanvas != null && interviewCanvas.activeSelf)
        {
            interviewCanvas.SetActive(false);
            Debug.Log("❌ Interview canvas auto-closed on layout exit.");
        }

        HideCursor();
        isInLayoutMode = false;



        Debug.Log("🎮 Back to main camera");
    }

    void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
