using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeStarter : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        GameObject fadeObject = GameObject.Find("Fade");

        if (fadeObject != null)
        {
            CanvasGroup group = fadeObject.GetComponent<CanvasGroup>();
            if (group != null)
            {
                Debug.Log("[FadeStarter] Found Fade, starting fade out");
                StartCoroutine(FadeFromBlack(group));
            }
            else
            {
                Debug.LogWarning("[FadeStarter] Fade object has no CanvasGroup");
            }
        }
        else
        {
            Debug.LogWarning("[FadeStarter] Fade object not found");
        }
    }

    private IEnumerator FadeFromBlack(CanvasGroup group)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            group.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        group.alpha = 0f;
    }
}
