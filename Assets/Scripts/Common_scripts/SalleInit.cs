using UnityEngine;
using System.Collections;

public class SalleInit : MonoBehaviour
{
    [SerializeField] private string salleName;


    void Start()
    {
        StartCoroutine(DelayedInit());
    }
    private IEnumerator DelayedInit()
    {
        yield return null;
        Debug.Log("🕒 SceneInit запускается");

        GameObject[] infoCanvases = GameObject.FindGameObjectsWithTag("PaintingInfo");

        if (VictoryTracker.Instance == null || PaintingInfoManager.Instance == null)
        {
            Debug.LogWarning("❗ Не найдены менеджеры!");
            yield break;
        }

        bool show = VictoryTracker.Instance.HasWon(salleName);
        Debug.Log("🎨 Scene: " + salleName + " — show painting info: " + show);

        PaintingInfoManager.Instance.SetAllActive(show);

    }
}
