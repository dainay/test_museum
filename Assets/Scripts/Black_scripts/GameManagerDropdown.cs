using UnityEngine;

public class GameManagerDropdown : MonoBehaviour
{
    public static GameManagerDropdown Instance;
    public DropdownChecker[] dropdowns;

    [SerializeField] private Animator glassAnimator;
    [SerializeField] private Camera winCamera;

    [SerializeField] private string salleName = "black";

    private Camera mainCamera;



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Debug.Log("🎥 MainCamera found at start: " + mainCamera.name);
        }
        else
        {
            Debug.LogWarning("⚠️ MainCamera not found at start!");
        }
    }


    public void ValidateAll()
    {
        foreach (var checker in dropdowns)
        {
            if (!checker.isCorrect)
            {
                return;
            }
        }
       
        BlackRoomWin();
    }

    private void BlackRoomWin()
    {
        Debug.Log("Black Room Win");

        Cursor.visible = false;
        Cursor.lockState = false ? CursorLockMode.None : CursorLockMode.Locked;

        mainCamera.gameObject.SetActive(false);
        winCamera.gameObject.SetActive(true);
       
        glassAnimator.SetTrigger("Drop");

        // ✅ Запоминаем победу
        VictoryTracker.Instance.SetVictory(salleName);

        // ✅ Активируем все Canvas с описанием
        GameObject[] infoCanvases = GameObject.FindGameObjectsWithTag("PaintingInfo");

        foreach (GameObject canvas in infoCanvases)
        {
            canvas.SetActive(true);
        }

        Debug.Log("🎨 Painting Info shown and registered for: " + salleName);
        Invoke("ReturnToMainCamera", 3.4f);

    }
    private void ReturnToMainCamera()
    {
        if (winCamera != null)
            winCamera.gameObject.SetActive(false);

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        Debug.Log("🔄 Returned to main camera after win cutscene.");
    }



}
