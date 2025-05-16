using UnityEngine;

public class BlackDropdownMenuHandler : MonoBehaviour
{
    private BlackRaycasterManager raycasterManager;
    private Camera mainCamera;      // Основная камера игрока
    [SerializeField] private Camera dropdownCamera;  // Камера для выпадающего меню

    private CanvasGroup[] allCanvasGroups;
    private bool isDropdownActive = false;
    private GameObject playerCanvasObject;

    void Start()
    { 
        //find main camera
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Debug.Log("[Dropdown] Main camera assigned automatically: " + mainCamera.name);
        }
        else
        {
            Debug.LogWarning("[Dropdown] ⚠️ Main camera not found!");
        }

        //find raycasterManager in Player
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            raycasterManager = player.GetComponentInChildren<BlackRaycasterManager>();

            if (raycasterManager != null)
            {
                Debug.Log("🔍 RaycasterManager найден внутри Player: " + raycasterManager.name);
            }
            else
            {
                Debug.LogWarning("⚠️ Player найден, но внутри нет BlackRaycasterManager");
            }
        }
        else
        {
            Debug.LogWarning("❌ Объект с тегом Player не найден!");
        }

        //collect canvas
        GameObject[] canvasObjects = GameObject.FindGameObjectsWithTag("CanvasPainter");
        allCanvasGroups = new CanvasGroup[canvasObjects.Length];

        for (int i = 0; i < canvasObjects.Length; i++)
        {
            allCanvasGroups[i] = canvasObjects[i].GetComponent<CanvasGroup>();
        }

        playerCanvasObject = GameObject.FindWithTag("PlayerCanvas");

        if (playerCanvasObject == null)
        {
            Debug.LogWarning("[Dropdown] No PlayerCanvas found in scene");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        if (isDropdownActive && Input.GetKeyDown(KeyCode.X))
        {
            SwitchToMainCamera();
        }
    }

    void HandleClick()
    {
        GameObject hitObject = raycasterManager.GetRaycastHit();

        if (hitObject != null && hitObject.CompareTag("DropPainter"))
        {
            Debug.Log("🎨 DropPainter clicked: " + hitObject.name);

            BlackDropdownMenuHandler handler = hitObject.GetComponent<BlackDropdownMenuHandler>();

            if (handler != null)
            {
                handler.SwitchToDropdownCamera();
            }

            DeactivateAllCanvasGroups();

            if (playerCanvasObject != null)
            {
                playerCanvasObject.SetActive(false); 
                Debug.Log("[Dropdown] PlayerCanvas disabled");
            }

            // Активируем только нужный дочерний CanvasGroup
            foreach (Transform child in hitObject.transform)
            {
                if (child.CompareTag("CanvasPainter"))
                {
                    CanvasGroup cg = child.GetComponent<CanvasGroup>();
                    if (cg != null)
                    {
                        cg.interactable = true;
                        cg.blocksRaycasts = true;
                        //cg.alpha = 1f;
                    }
                }
            }
        }
    }

    void DeactivateAllCanvasGroups()
    {
        foreach (CanvasGroup cg in allCanvasGroups)
        {
            if (cg != null)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
                //cg.alpha = 0f;
            }
        }
    }

    public void SwitchToDropdownCamera()
    {
        isDropdownActive = true;

        mainCamera.gameObject.SetActive(false);
        dropdownCamera.gameObject.SetActive(true);

        EnableCursor(true);
    }

    public void SwitchToMainCamera()
    {
        isDropdownActive = false;

        mainCamera.gameObject.SetActive(true);
        dropdownCamera.gameObject.SetActive(false);

        if (playerCanvasObject != null)
        {
            playerCanvasObject.SetActive(true); 
            Debug.Log("[Dropdown] PlayerCanvas enabled");
        }

        ReactivateAllCanvasGroups();
        EnableCursor(false);
    }

    void ReactivateAllCanvasGroups()
    {
        foreach (CanvasGroup cg in allCanvasGroups)
        {
            if (cg != null)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
                //cg.alpha = 1f;
            }
        }
    }

    void EnableCursor(bool enable)
    {
        Cursor.visible = enable;
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
