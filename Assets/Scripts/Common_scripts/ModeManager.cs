using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public enum MuseumMode { Classic, Interactive }

    public static ModeManager Instance;

    [SerializeField] private GameObject scoreUI;

    public MuseumMode CurrentMode { get; private set; } = MuseumMode.Classic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMode(MuseumMode mode)
    {
        CurrentMode = mode;

        if (mode == MuseumMode.Classic)
        {
            scoreUI.SetActive(false);
        }
        else if (mode == MuseumMode.Interactive)
        {
            scoreUI.SetActive(true);
        }

    }
 
}
