using UnityEngine;

public class Prix : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;
    [SerializeField] private string salleName = "black";

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  
        {
            GetPrix();
        }
    }

    void GetPrix()
    {
        GameObject hitObject = raycasterManager.GetRaycastHit();
        if (hitObject != null && hitObject.CompareTag("Prix"))
        {
            VictoryTracker.Instance.SetVictory(salleName);

             
            Destroy(hitObject);
        }
    }
}
