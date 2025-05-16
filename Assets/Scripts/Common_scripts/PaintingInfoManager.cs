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
    }

    public void SetAllActive(bool state)
    {
        foreach (var canvas in infoCanvases)
        {
            if (canvas != null)
                canvas.SetActive(state);
        }

    }
}
