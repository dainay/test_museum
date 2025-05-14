using UnityEngine;
using TMPro; 

public class BlackBasket : MonoBehaviour
{
    [SerializeField] private TMP_Text painterText;
    [SerializeField] private TMP_Text salleText;
    private const string defaultText = "Système de détection du peintre";  
    private int ballCount = 0;  

    private void Start()
    {
        painterText.text = defaultText; 
    }

    private void OnTriggerEnter(Collider other)
    {
        BlackBallInfo ball = other.GetComponent<BlackBallInfo>();

        if (ball != null)
        {
            painterText.text = "C'est " + ball.GetPainterName();
            salleText.text = "(salle " + ball.GetSalle() + ")";
            ballCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BlackBallInfo ball = other.GetComponent<BlackBallInfo>();

        if (ball != null)
        {
            ballCount--;
            if (ballCount <= 0)  
            {
                painterText.text = defaultText;
                salleText.text = "";
            }
        }
    }
}
