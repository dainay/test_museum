using UnityEngine;

public class LayoutClickDispatcher : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hit = raycasterManager.GetRaycastHit();
            if (hit != null && hit.CompareTag("Layout"))
            {
                TryFocusLayout layout = hit.GetComponent<TryFocusLayout>();
                if (layout != null)
                {
                    layout.EnterLayoutMode();
                }
            }
        }
    }
}
