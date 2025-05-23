using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // R�f�rence au lecteur vid�o

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Prepare(); // Pr�pare la vid�o avant lecture
        }
    }

    void OnMouseDown()
    {
        if (videoPlayer != null)
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause(); // Met en pause si la vid�o est en lecture
            }
            else
            {
                videoPlayer.Play(); // D�marre la vid�o si elle est en pause
            }
        }
    }
}

