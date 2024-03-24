using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text _textComponent;

    private void Start()
    {
        UpdateLanguageSelection();
    }

    private void UpdateLanguageSelection()
    {
        _textComponent.text = LocalizationManager.Instance.languageDescriptions[LocalizationManager.Instance.languageIndex];
    }

    public void ChangeSelection(int change)
    {
        int languageIndex = LocalizationManager.Instance.languageIndex;
        int languageSize = LocalizationManager.Instance.languageFileNames.Length;
        languageIndex = (languageIndex + change + languageSize) % languageSize;
        LocalizationManager.Instance.languageIndex = languageIndex;
        LocalizationManager.Instance.UpdateLocalizationText();
        UpdateLanguageSelection();
    }
}
