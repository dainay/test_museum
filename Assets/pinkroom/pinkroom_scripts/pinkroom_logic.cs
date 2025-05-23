using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class pinkroom_logic : MonoBehaviour
{
    [Header("Interaction joueur")]
    private Transform player;
    [SerializeField] private Transform targetObject;
    [SerializeField] private float activationDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Canvas")]
    [SerializeField] private Canvas codeCanvas;

    [Header("Chiffres & Contrôles")]
    [SerializeField] private TextMeshProUGUI[] digitsText;
    [SerializeField] private Button[] upButtons;
    [SerializeField] private Button[] downButtons;
    [SerializeField] private Button validateButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private int[] correctCode = new int[4] { 1, 2, 3, 4 };

    [Header("Coffre à animer")]
    [SerializeField] private Transform chestToOpen;
    [SerializeField] private Vector3 positionOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, 90f, 0f);
    [SerializeField] private float openDuration = 1f;

    private Camera mainCamera;
    private bool panelOpened = false;
    private bool chestOpened = false;
    private int[] currentCode = new int[4];

    private void Start()
    {
        mainCamera = Camera.main;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
        else
            Debug.LogError("[Start] Aucun objet avec le tag 'Player' trouvé !");


        if (codeCanvas != null)
            codeCanvas.enabled = false;
        else
            Debug.LogWarning("[Start] codeCanvas est NULL !");

        for (int i = 0; i < 4; i++)
        {
            int index = i;

            if (i < upButtons.Length && upButtons[i] != null)
                upButtons[i].onClick.AddListener(() => IncrementDigit(index));
            else
                Debug.LogWarning($"[Start] upButtons[{i}] est manquant ou null.");

            if (i < downButtons.Length && downButtons[i] != null)
                downButtons[i].onClick.AddListener(() => DecrementDigit(index));
            else
                Debug.LogWarning($"[Start] downButtons[{i}] est manquant ou null.");
        }

        if (validateButton != null)
            validateButton.onClick.AddListener(CheckCode);
        else
            Debug.LogWarning("[Start] validateButton est NULL !");

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseCodePanel);
        else
            Debug.LogWarning("[Start] closeButton est NULL !");
    }

    private void Update()
    {
        if (panelOpened)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseCodePanel();

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
                if (!Physics.Raycast(ray, out _, activationDistance, interactableLayer))
                    CloseCodePanel();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (player == null)
            {
                Debug.LogError("[Update] player est NULL !");
                return;
            }
            if (targetObject == null)
            {
                Debug.LogError("[Update] targetObject est NULL !");
                return;
            }

            float distance = Vector3.Distance(player.position, targetObject.position);
            if (distance > activationDistance)
            {
                Debug.Log("[Update] Trop loin pour interagir.");
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, activationDistance, interactableLayer))
            {
                Debug.Log($"[Update] Raycast a touché : {hit.transform.name}");
                if (hit.transform == targetObject)
                    OpenCodePanel();
            }
            else
            {
                Debug.Log("[Update] Raycast n'a rien touché.");
            }
        }
    }

    private void OpenCodePanel()
    {
        panelOpened = true;
        if (codeCanvas != null)
        {
            codeCanvas.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("[OpenCodePanel] Canvas activé");
        }
    }

    public void CloseCodePanel()
    {
        panelOpened = false;
        if (codeCanvas != null)
        {
            codeCanvas.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("[CloseCodePanel] Canvas fermé");
        }
    }

    public void IncrementDigit(int index)
    {
        Debug.Log($"[IncrementDigit] Index {index}, avant={currentCode[index]}");
        currentCode[index] = (currentCode[index] + 1) % 10;
        UpdateDisplay(index);
    }

    public void DecrementDigit(int index)
    {
        Debug.Log($"[DecrementDigit] Index {index}, avant={currentCode[index]}");
        currentCode[index] = (currentCode[index] + 9) % 10;
        UpdateDisplay(index);
    }

    private void UpdateDisplay(int index)
    {
        if (digitsText == null)
        {
            Debug.LogError("[UpdateDisplay] digitsText est NULL !");
            return;
        }
        if (index >= digitsText.Length)
        {
            Debug.LogError($"[UpdateDisplay] Index {index} hors limites du tableau digitsText (length={digitsText.Length})");
            return;
        }
        if (digitsText[index] == null)
        {
            Debug.LogError($"[UpdateDisplay] digitsText[{index}] est NULL !");
            return;
        }

        digitsText[index].text = currentCode[index].ToString();
        Debug.Log($"[UpdateDisplay] Index {index}, valeur affichée={currentCode[index]}");
    }

    public void CheckCode()
    {
        Debug.Log("[CheckCode] Début de la vérification du code...");

        for (int i = 0; i < correctCode.Length; i++)
        {
            Debug.Log($"[CheckCode] Index {i} : attendu {correctCode[i]}, actuel {currentCode[i]}");

            if (currentCode[i] != correctCode[i])
            {
                Debug.Log("[CheckCode] Code incorrect");
                return;
            }
        }

        Debug.Log("[CheckCode] Code correct !");
        CloseCodePanel();

        if (!chestOpened)
            StartCoroutine(OpenChest());
    }

    private IEnumerator OpenChest()
    {
        if (chestToOpen == null)
        {
            Debug.LogError("[OpenChest] chestToOpen est NULL !");
            yield break;
        }

        chestOpened = true;

        Vector3 startPos = chestToOpen.position;
        Vector3 endPos = startPos + positionOffset;
        Quaternion startRot = chestToOpen.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(rotationOffset);

        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            float t = elapsed / openDuration;
            chestToOpen.position = Vector3.Lerp(startPos, endPos, t);
            chestToOpen.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        chestToOpen.position = endPos;
        chestToOpen.rotation = endRot;
        Debug.Log("[OpenChest] Coffre ouvert");
    }
}
