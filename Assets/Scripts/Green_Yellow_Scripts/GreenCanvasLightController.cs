using UnityEngine;
using UnityEngine.UI;

public class CanvasLightController : MonoBehaviour
{
    public GameObject panel; 
    public Text messageText;  
    public float displayDuration = 2f; 

    void Start()
    {
        panel.SetActive(false);
    }


    public void ShowCanvas(string message)
    {
        messageText.text = message;  
        panel.SetActive(true);  
        CancelInvoke(nameof(HideCanvas));  
        Invoke(nameof(HideCanvas), displayDuration); 
    }


    void HideCanvas()
    {
        panel.SetActive(false);
    }
}
