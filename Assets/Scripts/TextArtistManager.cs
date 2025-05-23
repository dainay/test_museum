using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextArtistManager : MonoBehaviour
{
    public TextMeshPro texteArtiste;  // R�f�rence au texte affich�
    [TextArea(2, 5)] public string nomArtiste;  // Nom modifiable dans l'Inspector
    public int maxCaracteres = 30; // Limite de caract�res

    void Start()
    {
        if (texteArtiste != null)
        {
            // V�rifie si le texte d�passe la limite et coupe le texte si n�cessaire
            if (nomArtiste.Length > maxCaracteres)
            {
                nomArtiste = nomArtiste.Substring(0, maxCaracteres);
            }

            texteArtiste.text = nomArtiste;  // Applique le texte mis � jour
        }
    }
}
