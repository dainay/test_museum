using UnityEngine;
using UnityEngine.UI;

public class LayoutButtonManager : MonoBehaviour
{
    [SerializeField] private Button interviewButton;
    [SerializeField] private GameObject interviewCanvas;

    [SerializeField] private Button youtubeButton;
    [SerializeField] private string youtubeURL;

    void Start()
    {
        if (interviewButton != null)
        {
            interviewButton.onClick.AddListener(() =>
            {
                if (interviewCanvas != null)
                {
                    interviewCanvas.SetActive(true);
                    Debug.Log("✅ Interview canvas opened by local manager");
                }
            });
        }

        if (youtubeButton != null)
        {
            youtubeButton.onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(youtubeURL))
                {
                    Application.OpenURL(youtubeURL);
                    Debug.Log("🌐 Opened YouTube: " + youtubeURL);
                }
            });
        }
    }
}
