using UnityEngine;

public class wr_GameManager : MonoBehaviour
{
    public static wr_GameManager Instance;
    
    [SerializeField] private int totalSpheres = 3;
    private int spheresActivatedCount = 0;

    [Header("Référence Sphère Finale")]
    [SerializeField] private GameObject finalSphere; // À assigner dans l'inspecteur

    void Awake()
    {
        if(Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);

        // Désactive la sphère finale au départ
        if(finalSphere != null)
            finalSphere.SetActive(false);
    }

    public void IncrementCounter()
    {
        spheresActivatedCount++;
        Debug.Log($"Sphères activées : {spheresActivatedCount}/{totalSpheres}");

        if(spheresActivatedCount >= totalSpheres)
        {
            ActivateFinalSphere();
        }
    }

    private void ActivateFinalSphere()
    {
        if(finalSphere != null)
        {
            finalSphere.SetActive(true);
            Debug.Log("Sphère finale activée !");
        }
        else
        {
            Debug.LogError("Sphère finale non assignée !");
        }
    }
}