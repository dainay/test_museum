using UnityEngine;
using UnityEngine.Video;

public class SecretDoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform door;

    [SerializeField] private VideoPlayer videoPlayer;
    void Start()
    {
        GameObject trackerObj = GameObject.FindWithTag("VictoryTracker");

        if (trackerObj != null)
        {
            VictoryTracker tracker = trackerObj.GetComponent<VictoryTracker>();

            if (tracker != null && tracker.globalSecretUnlocked)
            {
                door.localEulerAngles = new Vector3(0f, 0f, 95f); 
                Debug.Log("🚪 Секретная дверь открыта!");
            }
            else
            {
                Debug.Log("🔒 Секретная дверь остаётся закрытой.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Объект с тегом 'VictoryTracker' не найден!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DOOR COLLIDER CROSSEd");

        if (other.CompareTag("Player")) 
        {
            if (videoPlayer != null && !videoPlayer.isPlaying)
            {
                videoPlayer.Play();
                Debug.Log("▶️ Видео запущено при входе в зону двери.");
            }
        }
    }
}
