using System.Collections.Generic;
using UnityEngine;

public class VictoryTracker : MonoBehaviour
{
    public static VictoryTracker Instance;

    private Dictionary<string, bool> victories = new Dictionary<string, bool>();

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
    }

    public void SetVictory(string salleName)
    {
        victories[salleName] = true;
        Debug.Log("🏆 Victoire enregistrée pour: " + salleName);
    }

    public bool HasWon(string salleName)
    {
        bool won = victories.ContainsKey(salleName) && victories[salleName];
        Debug.Log("🔍 Vérification pour " + salleName + " : " + won);
        return won;
    }
}
