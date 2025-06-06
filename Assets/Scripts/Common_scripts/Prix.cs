using UnityEngine;

public class Prix : MonoBehaviour
{
    private BlackRaycasterManager raycasterManager;
    [SerializeField] private string salleName = "black";


    void Start()
    {
        GameObject raycasterObj = GameObject.FindWithTag("Raycaster");
        if (raycasterObj != null)
        {
            raycasterManager = raycasterObj.GetComponent<BlackRaycasterManager>();
        }

        if (raycasterManager == null)
        {
            Debug.LogError("⚠️ RaycasterManager не найден на объекте с тегом 'Raycaster' !");
        }
    }

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
