using UnityEngine;

public class YellowTablet : MonoBehaviour
{
    public string tabletName;  // Nom unique de la tablette, défini dans l'inspecteur Unity

    // Méthode pour retourner le nom de la tablette
    public string GetTabletName()
    {
        return tabletName;
    }
}
