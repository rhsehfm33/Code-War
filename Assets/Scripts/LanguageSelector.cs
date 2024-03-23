using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    private int selectedIndex = 0;
    private string[] options = { "English", "�ѱ���" };

    void Start()
    {

    }

    public void ChangeSelection(int change)
    {
        selectedIndex = (selectedIndex + change + options.Length) % options.Length;
        textComponent.text = options[selectedIndex];
    }
}
