using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    public Camera playerCamera; 
    public float rayDistance = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                KeywordCard card = hit.collider.GetComponent<KeywordCard>();
                if (card != null)
                {
                    card.OnClicked();
                }
            }
        }
    }
}