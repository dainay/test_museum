using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TryFocusLayout : MonoBehaviour
{
    private Camera layoutCamera;
    private Camera mainCamera;

    private GameObject videoPlayerObject;
    private VideoPlayer videoPlayer;

    private RawImage staticPreviewImage;
    private RawImage videoDisplayImage;

    private bool isInLayoutMode = false;

    void Awake()
    {
        // Найдём камеру layout внутри себя
        layoutCamera = GetComponentInChildren<Camera>(includeInactive: true);

        // Главную камеру ищем по тегу
        GameObject mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamObj != null)
            mainCamera = mainCamObj.GetComponent<Camera>();

        // Найдём VideoPlayer, если он есть
        videoPlayerObject = GetComponentInChildren<VideoPlayer>()?.gameObject;
        videoPlayer = videoPlayerObject?.GetComponent<VideoPlayer>();

        // Найдём все RawImage
        RawImage[] rawImages = GetComponentsInChildren<RawImage>(includeInactive: true);
        foreach (var img in rawImages)
        {
            if (img.name.ToLower().Contains("preview"))
                staticPreviewImage = img;
            else if (img.name.ToLower().Contains("video"))
                videoDisplayImage = img;
        }

        // Начальное состояние
        if (videoPlayerObject != null) videoPlayerObject.SetActive(false);
        if (staticPreviewImage != null) staticPreviewImage.enabled = true;
        if (videoDisplayImage != null) videoDisplayImage.enabled = false;
        if (layoutCamera != null) layoutCamera.gameObject.SetActive(false);
    }

    public void EnterLayoutMode()
    {
        // Деактивировать все layout-объекты кроме текущего
        GameObject[] allLayouts = GameObject.FindGameObjectsWithTag("Layout");
        foreach (GameObject layout in allLayouts)
        {
            if (layout != this.gameObject)
            {
                layout.SetActive(false); // 💥 отключаем
            }
        }

        // 💥 Отключаем Canvas игрока
        GameObject playerCanvas = GameObject.FindWithTag("PlayerCanvas");
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // ✅ Возвращаем Canvas игрока
        GameObject playerCanvas = GameObject.FindWithTag("PlayerCanvas");
        if (playerCanvas != null)
        {
            playerCanvas.SetActive(true);
            Debug.Log("✅ PlayerCanvas reactivated");
        }

        // Вернём остальные layout обратно
        GameObject[] allLayouts = GameObject.FindGameObjectsWithTag("Layout");
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
