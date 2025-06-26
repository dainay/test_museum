using UnityEngine;

public class GameManagerDropdown : MonoBehaviour
{
    public static GameManagerDropdown Instance;
    public DropdownChecker[] dropdowns;

    [SerializeField] private Animator glassAnimator;
    [SerializeField] private Camera winCamera;

    [SerializeField] private string salleName = "black";

    private Camera mainCamera;

    public bool blackRoomVictory = false;

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
 

        // ✅ activate all canvas with info
        PaintingInfoManager.Instance.SetAllActive(true);

        VictoryTracker.Instance.blackRoomShow = true;
        Debug.Log("✅ Победа в чёрной комнате зарегистрирована через глобальную переменную.");

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
