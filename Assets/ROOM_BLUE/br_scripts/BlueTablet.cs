using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTablet : MonoBehaviour
{
    public string tabletName;  // Nom unique de la tablette, défini dans l'inspecteur Unity

    public string GetTabletName()
    {
        return tabletName;
    }
}
