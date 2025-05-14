using UnityEngine;

public class BlackDropdownMenuHandler : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;
    [SerializeField] private Camera mainCamera;      // Основная камера игрока
    [SerializeField] private Camera dropdownCamera;  // Камера для выпадающего меню

    private CanvasGroup[] allCanvasGroups;
    private bool isDropdownActive = false;

    void Start()
    {
        GameObject[] canvasObjects = GameObject.FindGameObjectsWithTag("CanvasPainter");
        allCanvasGroups = new CanvasGroup[canvasObjects.Length];

        for (int i = 0; i < canvasObjects.Length; i++)
        {
            allCanvasGroups[i] = canvasObjects[i].GetComponent<CanvasGroup>();
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
