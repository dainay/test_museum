using UnityEngine;

public class BlackBallInfo : MonoBehaviour
{
    [SerializeField] private string painterName;
    [SerializeField] private string salle;

    public string GetPainterName()
    {
        return painterName;
    }

    public string GetSalle()
    {
        return salle;
    }
}
