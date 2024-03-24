using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public delegate void LanguageChangedHandler();
    public event LanguageChangedHandler OnLanguageChanged;

    public int languageIndex = 0;
    public readonly string[] languageFileNames = { "en", "kr" };
    public readonly string[] languageDescriptions = { "English", "ÇÑ±¹¾î" };

    private Dictionary<string, string> _idToLocalizedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateLocalizationText();
    }

    public void UpdateLocalizationText()
    {
        string language = languageFileNames[languageIndex];
        TextAsset localizedTextAssest = Resources.Load<TextAsset>("texts/" + language);
        if (localizedTextAssest != null)
        {
            string localizedTextJson = localizedTextAssest.text;
            _idToLocalizedText = JsonFlattener.FlattenJsonText(localizedTextJson);
        }
        OnLanguageChanged?.Invoke();

        foreach (KeyValuePair<string, string> kvp in _idToLocalizedText)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }

    public string GetLocalizedText(string textId)
    {
        return _idToLocalizedText.GetValueOrDefault(textId);
    }
}
