using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private GameObject interviewCanvas;

    public void CloseInterview()
    {
        if (interviewCanvas != null)
        {
            interviewCanvas.SetActive(false);
            Debug.Log("❌ Interview canvas closed.");
        }
    }
}
