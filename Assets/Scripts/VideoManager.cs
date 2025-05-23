using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Référence au lecteur vidéo

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Prepare(); // Prépare la vidéo avant lecture
        }
    }

    void OnMouseDown()
    {
        if (videoPlayer != null)
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause(); // Met en pause si la vidéo est en lecture
            }
            else
            {
                videoPlayer.Play(); // Démarre la vidéo si elle est en pause
            }
        }
    }
}

