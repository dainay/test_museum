using UnityEngine;
using TMPro;
using System;

public class DropdownChecker : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private int correctIndex; // правильный индекс

    [HideInInspector] public bool isCorrect = false;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(CheckAnswer);
        CheckAnswer(dropdown.value); // проверка при старте
    }

    void CheckAnswer(int index)
    {
        isCorrect = (index == correctIndex);
        Debug.Log(isCorrect + "CHECK THIS");
        GameManagerDropdown.Instance.ValidateAll();
    }
}
