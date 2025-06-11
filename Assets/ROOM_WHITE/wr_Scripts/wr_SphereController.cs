using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class wr_SphereController : MonoBehaviour
{
    [Header("References")]
    public Material sphereMaterial;
    public GameObject controlledWall;
    public GameObject statue; // Reference to the statue

    [Header("Emission Settings")]
    [SerializeField] private Color glowEmissionColor = Color.red;
    [SerializeField] private float maxIntensityDistance = 1f;
    private float maxEmissionIntensity = 4f;
    private MaterialPropertyBlock materialProps;
    private Color baseColor;

    [Header("Interaction Settings")]
    [SerializeField] private float sphereActivationRadius = 5f;
    private float animationDuration = 2f;

    [Header("Animation Settings")]
    [SerializeField] private float minAnimationSpeed = 0.1f;
    [SerializeField] private float maxAnimationSpeed = 1f;

    private Renderer sphereRenderer;
    private Collider sphereCollider;
    private Camera playerCamera;
    private bool isAnimating = false;
    private bool isActivated = true;
    private Animator sphereAnimator; // Reference to the Animator component
    private Animator statueAnimator; // Reference to the statue's Animator component

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

        // Get the statue's Animator component
        if (statue != null)
        {
            statueAnimator = statue.GetComponent<Animator>();
        }

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
        if (isAnimating || !isActivated) return;

        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        float normalizedDistance = Mathf.Clamp01(distance / sphereActivationRadius);
        float proximityFactor = 1 - normalizedDistance; // 1 = close, 0 = far

        // Set emission intensity with adjustable peak distance
        if (distance <= sphereActivationRadius)
        {
            // Calculate intensity with adjustable peak
            float intensity = CalculateAdjustedIntensity(distance);
            SetEmissionColor(glowEmissionColor * intensity);
        }
        else
        {
            SetEmissionColor(baseColor * 0.02f);
        }

        // Set animation speed based on proximity
        if (sphereAnimator != null)
        {
            float animationSpeed = Mathf.Lerp(minAnimationSpeed, maxAnimationSpeed, proximityFactor);
            sphereAnimator.SetFloat("proximityFactor", animationSpeed);
        }

        // Interaction
        if (Input.GetMouseButtonDown(0) && distance <= sphereActivationRadius && isActivated)
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

    // New method to calculate intensity with adjustable peak
    private float CalculateAdjustedIntensity(float distance)
    {
        // Calculate the distance where intensity should start decreasing
        float peakStartDistance = Mathf.Max(0, maxIntensityDistance);

        // If we're beyond the peak distance, intensity decreases linearly to activation radius
        if (distance > peakStartDistance)
        {
            float range = sphereActivationRadius - peakStartDistance;
            float t = Mathf.Clamp01((distance - peakStartDistance) / range);
            return Mathf.Lerp(maxEmissionIntensity, 0, t);
        }
        // If we're within the peak distance, intensity decreases as we get closer to the sphere
        else
        {
            float t = Mathf.Clamp01(distance / peakStartDistance);
            return Mathf.Lerp(maxEmissionIntensity, maxEmissionIntensity * 0.5f, t);
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

        // Wait for the statue animation to complete
        if (statueAnimator != null)
        {
            yield return new WaitForSeconds(statueAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        // Wait for the sphere animation to complete
        if (sphereAnimator != null)
        {
            AnimatorStateInfo stateInfo = sphereAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }

        // Play the statue animation if the statue and its animator are not null
        if (statueAnimator != null)
        {
            statueAnimator.SetTrigger("Disappear");
        }

        // Now animate the wall
        Vector3 startPos = controlledWall.transform.position;
        Vector3 endPos = startPos + Vector3.down * 14f;
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