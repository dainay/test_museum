using UnityEngine;
using UnityEngine.UI;  // Import pour Text UI classique

public class CanvasLightController : MonoBehaviour
{
    public GameObject panel;  // Panneau contenant le texte
    public Text messageText;  // Texte affiché (Text UI classique)
    public float displayDuration = 2f;  // Temps d'affichage

    void Start()
    {
        panel.SetActive(false);  // Cache le panneau au départ
    }

    // Fonction pour afficher le Canvas avec un message
    public void ShowCanvas(string message)
    {
        messageText.text = message;  // Met à jour le texte
        panel.SetActive(true);  // Affiche le panneau
        CancelInvoke(nameof(HideCanvas));  // Annule toute fermeture en attente
        Invoke(nameof(HideCanvas), displayDuration);  // Masque après X secondes
    }

    // Fonction pour cacher le Canvas
    void HideCanvas()
    {
        panel.SetActive(false);  // Cache le panneau
    }
}
