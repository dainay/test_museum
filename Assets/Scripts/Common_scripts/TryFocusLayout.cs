using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TryFocusLayout : MonoBehaviour
{
    private Camera layoutCamera;
    private Camera mainCamera;

    private GameObject videoPlayerObject;
    private VideoPlayer videoPlayer;

    [SerializeField] private RawImage staticPreviewImage;
    private RawImage videoDisplayImage;

    private bool isInLayoutMode = false;

    [SerializeField] private GameObject interviewCanvas;

    public GameObject[] allLayouts;
    private GameObject playerCanvas;



    void Awake()
    {
        allLayouts = GameObject.FindGameObjectsWithTag("Layout");
        playerCanvas = GameObject.FindWithTag("PlayerCanvas");


        // Найдём камеру layout внутри себя
        layoutCamera = GetComponentInChildren<Camera>(includeInactive: true);

        // Главную камеру ищем по тегу
        GameObject mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamObj != null)
            mainCamera = mainCamObj.GetComponent<Camera>();

        // Найдём VideoPlayer, если он есть
        videoPlayerObject = GetComponentInChildren<VideoPlayer>()?.gameObject;
        videoPlayer = videoPlayerObject?.GetComponent<VideoPlayer>();


        // Начальное состояние
        if (videoPlayerObject != null) videoPlayerObject.SetActive(false);
        if (staticPreviewImage != null) staticPreviewImage.enabled = true;
        if (videoDisplayImage != null) videoDisplayImage.enabled = false;
        if (layoutCamera != null) layoutCamera.gameObject.SetActive(false);

         
    }

    public void EnterLayoutMode()
    {
        // Деактивировать все layout-объекты кроме текущего
        
        foreach (GameObject layout in allLayouts)
        {
            if (layout != this.gameObject)
            {
                layout.SetActive(false); // 💥 отключаем
            }
        }

        // 💥 Отключаем Canvas игрока
        if (playerCanvas != null)
        {
            playerCanvas.SetActive(false);
            Debug.Log("🛑 PlayerCanvas deactivated");
        }

        // Переход в layout
        isInLayoutMode = true;

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (layoutCamera != null)
            layoutCamera.gameObject.SetActive(true);

        if (staticPreviewImage != null)
            staticPreviewImage.enabled = false;

        if (videoDisplayImage != null)
            videoDisplayImage.enabled = true;

        if (videoPlayerObject != null)
        {
            videoPlayerObject.SetActive(true);
            videoPlayer?.Play();
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("📌 Layout activated: " + gameObject.name);
    }

        public void ExitLayoutMode()
    {
        isInLayoutMode = false;

        if (layoutCamera != null)
            layoutCamera.gameObject.SetActive(false);

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        if (videoPlayer != null)
            videoPlayer.Stop();

        if (videoPlayerObject != null)
            videoPlayerObject.SetActive(false);

        if (videoDisplayImage != null)
            videoDisplayImage.enabled = false;

        if (staticPreviewImage != null)
            staticPreviewImage.enabled = true;

        if (interviewCanvas != null && interviewCanvas.activeSelf)
        {
            interviewCanvas.SetActive(false);
            Debug.Log("❌ Interview canvas deactivated");
        }


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // ✅ Возвращаем Canvas игрока
        if (playerCanvas != null)
        {
            playerCanvas.SetActive(true);
            Debug.Log("✅ PlayerCanvas reactivated");
        }

        // Вернём остальные layout обратно
       
        foreach (GameObject layout in allLayouts)
        {
            if (layout != this.gameObject)
            {
                layout.SetActive(true);
            }
        }

        Debug.Log("🎮 Back to main view");
    }



void Update()
    {
        if (isInLayoutMode && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ExitLayoutMode();
        }
    }
}
