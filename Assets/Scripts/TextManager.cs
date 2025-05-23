using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TextMeshPro texte;  // Référence au texte 3D
    [TextArea(2, 5)] public string nouveauTexte;  // Texte modifiable dans l’Inspector
    public int maxCaracteres = 100; // Nombre max de caractères autorisés

    void Start()
    {
        if (texte != null)
        {
            // Vérifie si le texte dépasse la limite et coupe le texte
            if (nouveauTexte.Length > maxCaracteres)
            {
                nouveauTexte = nouveauTexte.Substring(0, maxCaracteres);
            }

            texte.text = nouveauTexte; // Applique le texte mis à jour
        }
    }
}
