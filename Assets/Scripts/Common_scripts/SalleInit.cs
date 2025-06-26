using UnityEngine;
using System.Collections;

public class SalleInit : MonoBehaviour
{
    [SerializeField] private string salleName;
    [SerializeField] private GameObject prix;


    void Start()
    {
        StartCoroutine(DelayedInit());
    }
    private IEnumerator DelayedInit()
    {
        yield return null;
        Debug.Log("SceneInit started");

        GameObject[] infoCanvases = GameObject.FindGameObjectsWithTag("PaintingInfo");

        if (VictoryTracker.Instance == null || PaintingInfoManager.Instance == null)
        {
            Debug.LogWarning("no manager found in scene");
            yield break;
        }

        bool show = VictoryTracker.Instance.HasWon(salleName);
        Debug.Log("🎨 Scene: " + salleName + " — show painting info: " + show);

        PaintingInfoManager.Instance.SetAllActive(show);

        prix.SetActive(!show);

        if (prix != null)
        {
            prix.SetActive(!show);
        }
        else
        {
            Debug.LogWarning("no attached prix object in scene");
        }
    }
}
