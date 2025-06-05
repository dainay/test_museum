using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public Text scoreText;
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

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddPoint()
    {
        currentScore++;
        UpdateScoreUI();
    }

    public void RemovePoint()
{
    currentScore = Mathf.Max(0, currentScore - 1);
    UpdateScoreUI();
}


    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + currentScore.ToString()+ "/3";
        }
    }
}
