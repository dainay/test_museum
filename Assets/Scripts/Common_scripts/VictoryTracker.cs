using System.Collections.Generic;
using UnityEngine;

public class VictoryTracker : MonoBehaviour
{
    public static VictoryTracker Instance;

    private Dictionary<string, bool> victories = new Dictionary<string, bool>();

    [SerializeField] private ScoreTracker scoreTracker;

    [SerializeField] private List<string> allSalles;   

    [SerializeField] public bool globalSecretUnlocked = false;

    public bool blackRoomShow = false;


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

        CheckForAllVictories();
    }

    public bool HasWon(string salleName)
    {
        bool won = victories.ContainsKey(salleName) && victories[salleName];
        Debug.Log("🔍 Vérification pour " + salleName + " : " + won);
        return won;
    }

    private void CheckForAllVictories()
    {
        foreach (string salle in allSalles)
        {
            if (!victories.ContainsKey(salle) || !victories[salle])
            {
                Debug.Log("⛔ Не все salles завершены. Секретная комната закрыта.");
                return;
            }
        }

        globalSecretUnlocked = true;
        Debug.Log("🎉secret room is OPEN"); 
    }

}
