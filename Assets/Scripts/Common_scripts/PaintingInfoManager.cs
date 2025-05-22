using UnityEngine;
using System.Collections.Generic;

public class PaintingInfoManager : MonoBehaviour
{
    public static PaintingInfoManager Instance;

    private List<GameObject> infoCanvases = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        GameObject[] found = GameObject.FindGameObjectsWithTag("PaintingInfo");
        infoCanvases.AddRange(found);
        Debug.Log("📦 Сохранено PaintingInfo: " + infoCanvases.Count);
    }

    public void SetAllActive(bool state)
    {
        foreach (var canvas in infoCanvases)
        {
            Debug.Log("This painting is tring to be in: " + state);

            if (canvas != null)
                canvas.SetActive(state);
        }

    }
}
