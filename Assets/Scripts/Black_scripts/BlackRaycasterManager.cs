using UnityEngine;

public class BlackRaycasterManager : MonoBehaviour
{
    private Camera playerCamera; // Assign Main Camera in Inspector
    [SerializeField] private float maxDistance = 3.0f; // Raycast range

    void Start()
    {
       
        
            playerCamera = Camera.main;

            if (playerCamera != null)
            {
                Debug.Log("🎯 Assigned camera automatically: " + playerCamera.name);
            }
            else
            {
                Debug.LogWarning("⚠️ No MainCamera found for BlackRaycasterManager!");
            }
        
    }
    public GameObject GetRaycastHit()
    {
        if (playerCamera == null)
        {
            Debug.LogError("❌ RaycasterManager: Camera is not assigned!");
            return null;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Debug visualization in Scene view
        //Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.yellow, 2f);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            //Debug.Log("✅ Raycast hit: " + hit.transform.name);
            return hit.transform.gameObject; // Return the hit object
        }

        //Debug.Log("❌ Raycast found nothing!");
        return null;
    }
}
