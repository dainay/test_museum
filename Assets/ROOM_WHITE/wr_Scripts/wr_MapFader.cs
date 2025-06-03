using UnityEngine;

public class MapFader : MonoBehaviour
{
    private Transform player; // No longer serialized
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private SpriteRenderer mapRenderer;

    private Color initialColor;
    private float minAlpha = 0f;
    private float maxAlpha = 0.95f;

    void Start()
    {
        // Find player by tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        
        if (playerObject == null)
        {
            Debug.LogError("Player not found. Make sure a GameObject with tag 'Player' exists.");
            enabled = false; // Disable script to prevent errors
            return;
        }
        
        player = playerObject.transform;
        initialColor = mapRenderer.color;
        initialColor.a = minAlpha;
        mapRenderer.color = initialColor;
    }

    void Update()
    {
        // Skip if player is null (e.g., not found)
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        
        float currentAlpha = Mathf.Lerp(maxAlpha, minAlpha, normalizedDistance);
        
        Color newColor = mapRenderer.color;
        newColor.a = currentAlpha;
        mapRenderer.color = newColor;
    }
}