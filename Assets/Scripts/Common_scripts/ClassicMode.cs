using UnityEngine;

public class ClassicMode : MonoBehaviour
{ 
    void Start()
    {
        ModeManager.Instance.SetMode(ModeManager.MuseumMode.Classic);
    }

   
}
