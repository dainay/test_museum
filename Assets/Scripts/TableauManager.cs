using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableauManager : MonoBehaviour
{
    public Renderer tableauRenderer; 
    public Texture2D tableauImage; 

    void Start()
    {
        if (tableauRenderer != null && tableauImage != null)
        {
            tableauRenderer.material.SetTexture("_BaseMap", tableauImage); 
        }
    }
}
