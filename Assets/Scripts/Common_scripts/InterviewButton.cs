using UnityEngine;

public class InterviewButton : MonoBehaviour
{
    [SerializeField] private GameObject interviewCanvas;  // 🎯 Укажи Canvas вручную в инспекторе

    public void ShowInterviewCanvas()
    {
        if (interviewCanvas != null)
        {
            interviewCanvas.SetActive(true);
            Debug.Log("✅ Interview canvas activated!");
        }
        else
        {
            Debug.LogWarning("❌ Interview canvas not assigned!");
        }
    }
}
