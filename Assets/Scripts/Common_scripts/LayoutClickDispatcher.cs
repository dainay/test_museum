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
                Debug.Log("Layout clicked: " + hit);

                TryFocusLayout layout = hit.GetComponent<TryFocusLayout>();

                if (layout != null && layout.gameObject.activeInHierarchy)
                {
                    Debug.Log("we are inside the boucle and ineter into enter layout mode");
                    layout.EnterLayoutMode();
                }

            }
        }
    }
}
