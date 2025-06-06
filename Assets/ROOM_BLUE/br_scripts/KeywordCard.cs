using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordCard : MonoBehaviour
{
    private bool hasBeenClicked = false;
    private GameManager gameManager;

    public Material correctMaterial;
    public Material wrongMaterial;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnClicked()
    {
        if (hasBeenClicked) return;
        hasBeenClicked = true;

        Renderer meshRenderer = transform.Find("mesh1").GetComponent<Renderer>();

        bool isCorrect = gameManager != null && gameManager.IsCorrectAnswer(gameObject.name);

        meshRenderer.material = isCorrect ? correctMaterial : wrongMaterial;

        if (isCorrect)
        {
            Debug.Log("Bonne réponse !");
            gameManager.OnCorrectAnswerFound(gameObject.name);
        }
        else
        {
            Debug.Log("Mauvaise réponse !");
            gameManager.OnError();
        }
    }
}