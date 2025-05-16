using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject player = Instantiate(playerPrefab);

            // Новая стабильная стартовая позиция
            player.transform.position = new Vector3(1.41f, 1.456f, 2.7f);
            player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            DontDestroyOnLoad(player);
            Debug.Log("✅ Player instantiated and positioned.");
 
        }
        else
        {
            Debug.Log("ℹ️ Player already exists, no need to spawn again.");

            
        }
    
    }

}
