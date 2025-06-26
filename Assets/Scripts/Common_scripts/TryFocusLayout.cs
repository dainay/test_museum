using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

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

    private GameObject playerCanvas;
    private GameObject[] allLayouts;
    private List<GameObject> previouslyDisabledLayouts = new List<GameObject>();
 

    void Awake()
    {
        allLayouts = GameObject.FindGameObjectsWithTag("Layout");

        layoutCamera = GetComponentInChildren<Camera>(includeInactive: true);

        GameObject mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamObj != null)
            mainCamera = mainCamObj.GetComponent<Camera>();

        videoPlayerObject = GetComponentInChildren<VideoPlayer>()?.gameObject;
        videoPlayer = videoPlayerObject?.GetComponent<VideoPlayer>();

        if (videoPlayerObject != null) videoPlayerObject.SetActive(false);
        if (staticPreviewImage != null) staticPreviewImage.enabled = true;
        if (videoDisplayImage != null) videoDisplayImage.enabled = false;
        if (layoutCamera != null) layoutCamera.gameObject.SetActive(false);
    }

    void Start()
    {
        if (playerCanvas == null)
        {
            playerCanvas = GameObject.FindWithTag("PlayerCanvas");
            if (playerCanvas == null)
                Debug.LogWarning("no player canvas found in scene");
            else
                Debug.Log("player canvas is dound " + playerCanvas.name);
        }
    }

    public void EnterLayoutMode()
    {
        previouslyDisabledLayouts.Clear();

        foreach (GameObject layout in allLayouts)
        {
            if (layout != null && layout != this.gameObject && layout.activeSelf)
            {
                previouslyDisabledLayouts.Add(layout);
                layout.SetActive(false);
                Debug.Log("layout is off " + layout.name);
            }
        }

        if (playerCanvas != null)
        {
            playerCanvas.SetActive(false);
            Debug.Log("playercanvas is off");
        }

        isInLayoutMode = true;

        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (layoutCamera != null) layoutCamera.gameObject.SetActive(true);
        if (staticPreviewImage != null) staticPreviewImage.enabled = false;
        if (videoDisplayImage != null) videoDisplayImage.enabled = true;



        // only for "SalleBlack"
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Black_scene")
        {

            Debug.Log("Yes its Black_scene " + VictoryTracker.Instance.blackRoomShow);

            if (VictoryTracker.Instance.blackRoomShow)
            {

                Debug.Log("Yes win black");

                Debug.Log("SalleBlack won, activate video players");
                if (videoPlayerObject != null)
                {
                    videoPlayerObject.SetActive(true);
                    videoPlayer?.Play();
                }

                if (videoDisplayImage != null)
                    videoDisplayImage.enabled = true;
            }
            else
            {
                Debug.Log("no no black win");
                Debug.Log("SalleBlack is not finished, hide video");
                if (videoDisplayImage != null)
                    videoDisplayImage.enabled = false;
                if (videoPlayerObject != null)
                    videoPlayerObject.SetActive(false);
            }
        }
        else
        {
            // other scenes exept "SalleBlack" dont need to check victory
            if (videoPlayerObject != null)
            {
                videoPlayerObject.SetActive(true);
                videoPlayer?.Play();
            }

            if (videoDisplayImage != null)
                videoDisplayImage.enabled = true;
        }


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Layout is on: " + gameObject.name);
    }

    public void ExitLayoutMode()
    {
        isInLayoutMode = false;

        if (layoutCamera != null) layoutCamera.gameObject.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
        if (videoPlayer != null) videoPlayer.Stop();
        if (videoPlayerObject != null) videoPlayerObject.SetActive(false);
        if (videoDisplayImage != null) videoDisplayImage.enabled = false;
        if (staticPreviewImage != null) staticPreviewImage.enabled = true;

        if (interviewCanvas != null && interviewCanvas.activeSelf)
        {
            interviewCanvas.SetActive(false);
            Debug.Log("InterviewCanvas is off");
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCanvas != null)
        {
            playerCanvas.SetActive(true);
            Debug.Log("PlayerCanvas is back on");
        }

        foreach (GameObject layout in previouslyDisabledLayouts)
        {
            if (layout != null)
            {
                layout.SetActive(true);
                Debug.Log(" layout is on: " + layout.name);
            }
        }

        previouslyDisabledLayouts.Clear();

        Debug.Log("normal view");
    }

    void Update()
    {
        if (isInLayoutMode && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ExitLayoutMode();
        }
    }
}
