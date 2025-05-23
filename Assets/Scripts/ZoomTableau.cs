using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTableau : MonoBehaviour
{

  private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool isZoomed = false;
    private Camera mainCamera;

    void Start()
    {
        originalPosition = transform.position; // Sauvegarde la position originale du tableau
        originalScale = transform.localScale; // Sauvegarde la taille originale
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (!isZoomed)
        {
            // Agrandir et centrer le tableau sur l'écran
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2;
            transform.localScale *= 2;
        }
        else
        {
            // Revenir à la taille et position initiale
            transform.position = originalPosition;
            transform.localScale = originalScale;
        }
        
        isZoomed = !isZoomed; // Inverser l'état du zoom
    }
}
