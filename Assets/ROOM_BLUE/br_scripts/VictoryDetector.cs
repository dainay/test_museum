using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDetector : MonoBehaviour
{
    public int totalCorrectAnswers = 5;
    private int currentCorrectCount = 0;

    public GameObject victoryBall;
    public Camera playerCamera;
    public Camera victoryCamera;
    public float victoryDuration = 5f;

    private bool victoryTriggered = false;

    public void RegisterCorrectAnswer()
    {
        if (victoryTriggered) return;

        currentCorrectCount++;
        Debug.Log($"Bonne réponse enregistrée ({currentCorrectCount}/{totalCorrectAnswers})");

        if (currentCorrectCount >= totalCorrectAnswers)
        {
            StartCoroutine(PlayVictorySequence());
        }
    }

    private IEnumerator PlayVictorySequence()
    {
        Debug.Log("Début de la séquence de victoire !");
        victoryTriggered = true;

        // Activer la VictoryBall si elle existe
        if (victoryBall != null)
        {
            victoryBall.SetActive(true);
        }
        else
        {
            Debug.LogWarning("VictoryBall est NULL");
        }

        // Gestion des caméras
        if (victoryCamera != null && playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
            victoryCamera.gameObject.SetActive(true);

            Debug.Log("Caméra de victoire activée");

            yield return new WaitForSeconds(victoryDuration);

            victoryCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);

            Debug.Log("Retour à la caméra du joueur");
        }
        else
        {
            Debug.LogError("La caméra de victoire ou du joueur est manquante !");
        }
    }
}
