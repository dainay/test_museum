using UnityEngine;
using System.Collections;

public class wr_FillWallController : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] private float riseHeight = 5f;
    [SerializeField] private float riseDuration = 2f;
    
    private Vector3 startPosition;
    private bool isActivated = false;

    void Awake()
    {
        startPosition = transform.position;
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        if(!isActivated)
        {
            gameObject.SetActive(true);
            StartCoroutine(RiseAnimation());
            isActivated = true;
        }
    }

    private IEnumerator RiseAnimation()
    {
        float elapsed = 0f;
        Vector3 targetPosition = startPosition + Vector3.up * riseHeight;
        gameObject.SetActive(true);

        while(elapsed < riseDuration)
        {
            transform.position = Vector3.Lerp(
                startPosition, 
                targetPosition, 
                Mathf.SmoothStep(0f, 1f, elapsed/riseDuration)
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        GetComponent<Collider>().isTrigger = false;
    }
}