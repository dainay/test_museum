using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class wr_SphereController : MonoBehaviour
{
    [Header("References")]
    public Material blueMaterial;
    public Material redMaterial;
    public GameObject controlledWall;
    
    [Header("Glow Settings")]
    [SerializeField] private float maxEmissionIntensity = 5f;
    private MaterialPropertyBlock materialProps;
    private Color redEmission;
    private Color blueEmission;
    
    [Header("Interaction Settings")]
    [SerializeField] private float sphereActivationRadius = 1.5f;
    [SerializeField] private float animationDuration = 2f;

    private Renderer sphereRenderer;
    private Camera playerCamera;
    private bool isAnimating = false;
    private bool isActivated = true;

    public bool IsSphereActive() => gameObject.activeSelf;

    void Start()
    {
        if (blueMaterial == null || redMaterial == null)
        {
            Debug.LogError("Materials not assigned!", this);
            enabled = false;
            return;
        }

        playerCamera = Camera.main;
        sphereRenderer = GetComponent<Renderer>();

        materialProps = new MaterialPropertyBlock();
        redEmission = redMaterial.GetColor("_EmissionColor");
        blueEmission = blueMaterial.GetColor("_EmissionColor");

        sphereRenderer.material = blueMaterial;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        bool isInRange = distance <= sphereActivationRadius;

        // Gestion de l'émission
        if (isInRange)
        {
            float normalizedDistance = Mathf.Clamp01(distance / sphereActivationRadius);
            float intensity = (1 - normalizedDistance) * maxEmissionIntensity;
            sphereRenderer.GetPropertyBlock(materialProps);
            materialProps.SetColor("_EmissionColor", redEmission * intensity);
            sphereRenderer.SetPropertyBlock(materialProps);
        }
        else
        {
            sphereRenderer.GetPropertyBlock(materialProps);
            materialProps.SetColor("_EmissionColor", blueEmission);
            sphereRenderer.SetPropertyBlock(materialProps);
        }

        // Interaction
        if (Input.GetMouseButtonDown(0) && isInRange && isActivated)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, sphereActivationRadius))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    DeactivateSphere();
                }
            }
        }
    }

    IEnumerator AnimateWall()
    {
        if (controlledWall == null) yield break;
        
        isAnimating = true;
        isActivated = false;
        
        Vector3 startPos = controlledWall.transform.position;
        Vector3 endPos = startPos + Vector3.down * 7f;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animationDuration);
            controlledWall.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        controlledWall.SetActive(false);
        gameObject.SetActive(false);
        isAnimating = false;
    }

    void DeactivateSphere()
    {
        if (isAnimating || !isActivated) return;
        
        if (wr_GameManager.Instance != null)
        {
            wr_GameManager.Instance.IncrementCounter();
        }
        else
        {
            Debug.LogError("GameManager not found!", this);
        }

        StartCoroutine(AnimateWall());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sphereActivationRadius);
    }
}