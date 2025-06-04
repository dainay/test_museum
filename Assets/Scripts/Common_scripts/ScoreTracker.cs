using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private Image pink;
    [SerializeField] private Image blue;
    [SerializeField] private Image black;
    [SerializeField] private Image green;
    [SerializeField] private Image yellow;
    [SerializeField] private Image white;

    private Dictionary<string, Image> colorToImage;

    private void Awake()
    {
        colorToImage = new Dictionary<string, Image>()
        {
            { "pink", pink },
            { "blue", blue },
            { "black", black },
            { "green", green },
            { "yellow", yellow },
            { "white", white }
        };

        foreach (var image in colorToImage.Values)
        {
            SetAlpha(image, false);
        }
    }
    public void UpdateUI(string salleName)
    {
        if (colorToImage.TryGetValue(salleName, out Image image))
        {
            bool won = VictoryTracker.Instance.HasWon(salleName);
            SetAlpha(image, won);
        }
        else
        {
            Debug.LogWarning($"🎨 Aucune image trouvée pour la salle '{salleName}'");
        }
    }

    private void SetAlpha(Image image, bool isActive)
    {
        Color c = image.color;
        c.a = isActive ? 1f : 0f;
        image.color = c;
    }
}
