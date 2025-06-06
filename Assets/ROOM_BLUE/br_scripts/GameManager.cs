using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Si tu utilises TextMeshPro pour afficher le score
public class GameManager : MonoBehaviour
{
    public string[] correctAnswers = { "Pomme", "Chat", "Soualhi", "Doudou", "Chaussette" };

    private int foundCount = 0;
    private int errorCount = 0;   // <-- compteur erreurs

    public TMPro.TextMeshProUGUI scoreText;      // pour afficher "trouvés"
    public TMPro.TextMeshProUGUI errorText;      // pour afficher erreurs

    public VictoryDetector victoryDetector;


    void Start()
    {
        UpdateScoreText();
        UpdateErrorText();
    }

    public void OnCorrectAnswerFound(string answer)
    {
    foundCount++;
    UpdateScoreText();

    Debug.Log($"Trouvé : {answer} ({foundCount}/{correctAnswers.Length})");

    // Notifie le VictoryDetector
    if (victoryDetector != null)
        victoryDetector.RegisterCorrectAnswer();

    // Optionnel : garde aussi la méthode Victory() ici si tu veux d’autres effets
    if (foundCount >= correctAnswers.Length)
    {
        Victory();
    }
    }


   public void OnError()
{
    errorCount++;
    UpdateErrorText();
    Debug.Log($"Erreur n°{errorCount}");
}


    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"{foundCount} / {correctAnswers.Length} trouvés";
    }

    void UpdateErrorText()
    {
        if (errorText != null)
            errorText.text = $"Erreurs : {errorCount}";
    }

    void Victory()
    {
        Debug.Log("Félicitations ! Tu as trouvé tous les objets !");
        // Actions de victoire ici...
    }

    public bool IsCorrectAnswer(string answer)
    {
        foreach (var correct in correctAnswers)
        {
            if (correct == answer)
                return true;
        }
        return false;
    }
}