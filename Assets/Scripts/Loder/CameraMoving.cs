using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class CameraIntroAnimation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float startYRotation = 93f;
    [SerializeField] private float endYRotation = 270f;
    [SerializeField] private float rotationDuration = 7f;

    [Header("Movement Settings")]
    [SerializeField] private float startX = -15.5f;
    [SerializeField] private float endX = -17.5f;
    [SerializeField] private float moveDuration = 3f;

    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        StartCoroutine(PlayCameraSequence());  
    }

    private IEnumerator PlayCameraSequence()
    {

        yield return new WaitForSeconds(3f);
        // Установим начальное положение
        Vector3 startPos = transform.position;
        startPos.x = startX;
        transform.position = startPos;

        Vector3 rotation = transform.eulerAngles;
        rotation.y = startYRotation;
        transform.eulerAngles = rotation;

        // Плавный поворот
        float t = 0f;
        while (t < rotationDuration)
        {
            float yRot = Mathf.Lerp(startYRotation, endYRotation, t / rotationDuration);
            transform.eulerAngles = new Vector3(rotation.x, yRot, rotation.z);
            t += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = new Vector3(rotation.x, endYRotation, rotation.z);

        // Плавное перемещение по X
        t = 0f;
        Vector3 pos = transform.position;
        while (t < moveDuration)
        {
            float x = Mathf.Lerp(startX, endX, t / moveDuration);
            transform.position = new Vector3(x, pos.y, pos.z);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(endX, pos.y, pos.z);

        //yield return new WaitForSeconds(4f);
        // ▶️ Play video after movement
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
}
