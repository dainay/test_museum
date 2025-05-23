using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextArtistManager : MonoBehaviour
{
    public TextMeshPro texteArtiste;  // Référence au texte affiché
    [TextArea(2, 5)] public string nomArtiste;  // Nom modifiable dans l'Inspector
    public int maxCaracteres = 30; // Limite de caractères

    void Start()
    {
        if (texteArtiste != null)
        {
            // Vérifie si le texte dépasse la limite et coupe le texte si nécessaire
            if (nomArtiste.Length > maxCaracteres)
            {
                nomArtiste = nomArtiste.Substring(0, maxCaracteres);
            }

            texteArtiste.text = nomArtiste;  // Applique le texte mis à jour
        }
    }
}
