using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class wr_SphereController : MonoBehaviour
{
    [Header("References")]
    public Material sphereMaterial;
    public GameObject controlledWall;
    
    [Header("Emission Settings")]
    [SerializeField] private Color glowEmissionColor = Color.red;
    private float maxEmissionIntensity = 2f;
    private MaterialPropertyBlock materialProps;
    private Color baseColor;
    
    [Header("Interaction Settings")]
    [SerializeField] private float sphereActivationRadius = 5f;
    private float animationDuration = 2f;

    private Renderer sphereRenderer;
    private Collider sphereCollider;
    private Camera playerCamera;
    private bool isAnimating = false;
    private bool isActivated = true;
    private Animator sphereAnimator; // Reference to the Animator component

    public bool IsSphereActive() => gameObject.activeSelf;

    void Start()
    {
        if (sphereMaterial == null)
        {
            Debug.LogError("Sphere material not assigned!", this);
            enabled = false;
            return;
        }

        playerCamera = Camera.main;
        sphereRenderer = GetComponent<Renderer>();
        sphereCollider = GetComponent<Collider>();
        sphereAnimator = GetComponent<Animator>(); // Get the Animator component

        materialProps = new MaterialPropertyBlock();
        sphereRenderer.material = sphereMaterial;
        
        // Get the material's base color and apply low intensity
        baseColor = sphereMaterial.GetColor("_Color");
        SetEmissionColor(baseColor * 0.02f); // Default low intensity
        
        // Initialize animator state
        if (sphereAnimator != null)
        {
            sphereAnimator.SetBool("isActive", false);
        }
    }

    void Update()
    {
        // Skip update if we're animating or not activated
        if (isAnimating || !isActivated) return;
        
        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        bool isInRange = distance <= sphereActivationRadius;

        // Emission intensity based on proximity
        if (isInRange)
        {
            float normalizedDistance = Mathf.Clamp01(distance / sphereActivationRadius);
            float intensity = (1 - normalizedDistance) * maxEmissionIntensity;
            SetEmissionColor(glowEmissionColor * intensity);
        }
        else
        {
            // Set to base color with low intensity (0.02)
            SetEmissionColor(baseColor * 0.02f);
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

    void SetEmissionColor(Color color)
    {
        if (sphereRenderer == null) return;
        
        sphereRenderer.GetPropertyBlock(materialProps);
        materialProps.SetColor("_EmissionColor", color);
        sphereRenderer.SetPropertyBlock(materialProps);
    }

    IEnumerator AnimateWall()
    {
        if (controlledWall == null) yield break;
        
        isAnimating = true;
        isActivated = false;
        
        // Trigger the disappearing animation
        if (sphereAnimator != null)
        {
            sphereAnimator.SetBool("isActive", true);
        }
        
        // Wait for the animation to complete
        // We'll wait a short time to ensure the animation starts
        yield return new WaitForSeconds(0.1f);
        
        // Wait until the animation is playing
        if (sphereAnimator != null)
        {
            // Get the current state information
            AnimatorStateInfo stateInfo = sphereAnimator.GetCurrentAnimatorStateInfo(0);
            
            // Wait for the animation to finish
            yield return new WaitForSeconds(stateInfo.length);
        }
        
        // Now animate the wall
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
        gameObject.SetActive(false); // Deactivate sphere after animation completes
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