using System.Collections.Generic;
using UnityEngine;

public class VictoryTracker : MonoBehaviour
{
    public static VictoryTracker Instance;

    private Dictionary<string, bool> victories = new Dictionary<string, bool>();

    [SerializeField] private ScoreTracker scoreTracker;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }


        if (scoreTracker == null)
        {
            Debug.LogWarning("⚠️ ScoreTracker не привязан в инспекторе!");
        }

    }

    public void SetVictory(string salleName)
    {
        victories[salleName] = true;
        Debug.Log("🏆 Victoire enregistrée pour: " + salleName);

        if (scoreTracker != null)
        {
            scoreTracker.UpdateUI(salleName);
        }
    }

    public bool HasWon(string salleName)
    {
        bool won = victories.ContainsKey(salleName) && victories[salleName];
        Debug.Log("🔍 Vérification pour " + salleName + " : " + won);
        return won;
    }
}
