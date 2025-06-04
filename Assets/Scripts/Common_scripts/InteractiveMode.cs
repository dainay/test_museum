using UnityEngine;

public class InteractiveMode : MonoBehaviour
{ 
    void Start()
    {
        ModeManager.Instance.SetMode(ModeManager.MuseumMode.Interactive);
    }

   
}
