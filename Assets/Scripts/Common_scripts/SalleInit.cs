using UnityEngine;

public class SceneInit : MonoBehaviour
{
    [SerializeField] private string salleName;

    void Start()
    {
        GameObject[] infoCanvases = GameObject.FindGameObjectsWithTag("PaintingInfo");

        if (VictoryTracker.Instance == null)
        {
            Debug.LogWarning("⚠️ VictoryTracker not found!");
            return;
        }

        bool show = VictoryTracker.Instance.HasWon(salleName);
        Debug.Log("🎨 Scene: " + salleName + " — show painting info: " + show);

        PaintingInfoManager.Instance.SetAllActive(show);
    }
}
