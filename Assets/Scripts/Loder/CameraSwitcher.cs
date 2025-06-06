using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera introCamera;

    [SerializeField] private TextMeshProUGUI hintText; // Текст в Canvas игрока
    [SerializeField] private float delayBeforeHint = 4f;

    void Start()
    {
        // Убедимся, что только introCamera активна в начале
        mainCamera.enabled = false;
        introCamera.enabled = true;

        if (hintText != null)
            hintText.gameObject.SetActive(false);

        Invoke(nameof(ShowHint), delayBeforeHint);
    }

    void ShowHint()
    {
        if (hintText != null)
        {
            hintText.text = "Pour sortir, appuyez sur X";
            hintText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            introCamera.enabled = false;
            mainCamera.enabled = true;

            if (hintText != null)
                hintText.gameObject.SetActive(false);
        }
    }
}
