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
                Debug.Log("secret door is opened");
            }
            else
            {
                Debug.Log("secret door is closed, globalSecretUnlocked is false");
            }
        }
        else
        {
            Debug.LogWarning("no victroy tracker found in scene");
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
                Debug.Log("video player started playing");
            }
            Destroy(GetComponent<Collider>());
        }
    }
}
