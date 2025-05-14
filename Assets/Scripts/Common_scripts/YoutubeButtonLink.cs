using UnityEngine;

public class YouTubeButtonLink : MonoBehaviour
{
    [SerializeField] private string youtubeURL = "https://www.youtube.com/watch?v=ADDHERE";

    public void OpenYouTubeLink()
    {
        Application.OpenURL(youtubeURL);
    }
}
