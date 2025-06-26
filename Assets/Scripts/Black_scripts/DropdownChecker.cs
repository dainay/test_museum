using UnityEngine;
using TMPro;
using System;

public class DropdownChecker : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private int correctIndex; 

    [HideInInspector] public bool isCorrect = false;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(CheckAnswer);
        CheckAnswer(dropdown.value); 
    }

    void CheckAnswer(int index)
    {
        isCorrect = (index == correctIndex);
        Debug.Log(isCorrect + "CHECK THIS");
        GameManagerDropdown.Instance.ValidateAll();
    }
}
