using UnityEngine;

public class YellowGameManager : MonoBehaviour
{
    public YellowSign[] signs; // Références à tous les signes
    private int correctSignsCount = 0; // Nombre de signes correctement remplis
    public YellowCameraLookAt yellowCameraLookAt; // Référence au script de la caméra de la salle jaune

    void Start()
    {
        // Initialisation
        correctSignsCount = 0;
    }

    // Appelée chaque fois qu'un signe est vérifié comme complété
    public void CheckVictory()
    {
        correctSignsCount = 0;

        foreach (YellowSign sign in signs)
        {
            if (sign.IsCorrectlyPlaced())
            {
                correctSignsCount++;
            }
        }

        // Si tous les signes sont complétés
        if (correctSignsCount == signs.Length)
        {
            // Affiche le message de victoire
            Debug.Log("Victoire ! Toutes les tablettes sont correctement placées.");

            // Appelle la méthode pour faire tourner la caméra et faire tomber l'objet
            if (yellowCameraLookAt != null)
            {
                yellowCameraLookAt.OnYellowRoomVictory(); // Déclenche la séquence de la caméra
            }
        }
    }
}
