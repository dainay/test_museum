using UnityEngine;

public class MapFader : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private SpriteRenderer mapRenderer;

    private Color initialColor;
    private float minAlpha = 0f;
    private float maxAlpha = 0.8f;

    void Start()
    {
        initialColor = mapRenderer.color;
        initialColor.a = minAlpha;
        mapRenderer.color = initialColor;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        
        float currentAlpha = Mathf.Lerp(maxAlpha, minAlpha, normalizedDistance);
        
        Color newColor = mapRenderer.color;
        newColor.a = currentAlpha;
        mapRenderer.color = newColor;
    }
}