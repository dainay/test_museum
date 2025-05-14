using UnityEngine;

public class GameManagerDropdown : MonoBehaviour
{
    public static GameManagerDropdown Instance;
    public DropdownChecker[] dropdowns;

    [SerializeField] private Animator glassAnimator;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera winCamera; 

    private void Awake()
    {
        Instance = this;
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
    }

    
}
