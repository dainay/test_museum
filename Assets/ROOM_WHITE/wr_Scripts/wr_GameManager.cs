using UnityEngine;
using System.Collections;
using System;

public class wr_GameManager : MonoBehaviour
{
    public static wr_GameManager Instance;

    [SerializeField] private int totalSpheres = 3;
    private int spheresActivatedCount = 0;

    [Header("Référence Sphère Finale")]
    [SerializeField] private GameObject finalSphere; // À assigner dans l'inspecteur

    [Header("Référence Caméra")]
    [SerializeField] private Camera victoryCamera; // À assigner dans l'inspecteur

    [Header("Référence Joueur")]
    private string playerTag = "Player";

    private Camera mainCamera;
    private GameObject player;
    private int _animIDIsAlive;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Désactive la sphère finale au départ
        if (finalSphere != null)
            finalSphere.SetActive(false);

        // Récupère la caméra principale
        mainCamera = Camera.main;

        // Récupère le joueur par son tag
        player = GameObject.FindGameObjectWithTag(playerTag);

        // Initialise l'ID de l'animation
        _animIDIsAlive = Animator.StringToHash("IsVictory");
    }

    public void IncrementCounter()
    {
        spheresActivatedCount++;
        Debug.Log($"Sphères activées : {spheresActivatedCount}/{totalSpheres}");

        if (spheresActivatedCount >= totalSpheres)
        {
            StartCoroutine(VictorySequence());
        }
    }

    private IEnumerator VictorySequence()
    {
        yield return new WaitForSecondsRealtime(2f);

        // Passe à la caméra de victoire
        if (victoryCamera != null)
        {
            victoryCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
        }

        // Active la sphère finale
        if (finalSphere != null)
        {
            finalSphere.SetActive(true);
        }

        // Attend 3 secondes
        yield return new WaitForSecondsRealtime(3f);

        // Reviens à la caméra principale
        if (victoryCamera != null)
        {
            victoryCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }

        // Remet le temps à l'échelle normale
        Time.timeScale = 1f;
    }
}