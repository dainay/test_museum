using UnityEngine;

public class YellowGameManager : MonoBehaviour
{
    public YellowSign[] signs;
    public YellowCameraLookAt yellowCameraLookAt;

    public Camera mainCamera;
    public Camera victoryCamera;

    public GameObject victoryBall; 

    void Start()
    {
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
        if (victoryCamera != null) victoryCamera.gameObject.SetActive(false);
        if (victoryBall != null) victoryBall.SetActive(false); 
    }

    public void CheckVictory()
    {
        int correctSignsCount = 0;

        foreach (YellowSign sign in signs)
        {
            if (sign.IsCorrectlyPlaced())
            {
                correctSignsCount++;
            }
        }

        if (correctSignsCount == signs.Length)
        {
            Debug.Log("Victoire ! Toutes les tablettes sont correctement placées.");

            if (victoryBall != null) victoryBall.SetActive(true); 

            SwitchToVictoryCamera();
            Invoke(nameof(SwitchBackToMainCamera), 3f);

            if (yellowCameraLookAt != null)
            {
                yellowCameraLookAt.DropObjectNow();
            }
        }
    }

    void SwitchToVictoryCamera()
    {
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (victoryCamera != null) victoryCamera.gameObject.SetActive(true);
    }

    void SwitchBackToMainCamera()
    {
        if (victoryCamera != null) victoryCamera.gameObject.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
    }
}
