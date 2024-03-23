using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private Dictionary<string, string> idToSingleText;
    private Dictionary<string, List<String>> idToStoryTexts;

    public static LocalizationManager Instance { get; private set; }

    public enum Language
    {
        en,
        kr
    }

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
        LoadLocalizationText(Language.en);
    }

    public void LoadLocalizationText(Language language)
    {
        TextAsset localizedTextAssest = Resources.Load<TextAsset>("texts/" + language.ToString());
        if (localizedTextAssest != null)
        {
            string localizedTextJson = localizedTextAssest.text;
            idToSingleText = JsonFlattener.FlattenJsonText(localizedTextJson);
            idToStoryTexts = JsonFlattener.FlattenJsonTextArray(localizedTextJson);
        }

        foreach (KeyValuePair<string, string> kvp in idToSingleText)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }

        foreach (KeyValuePair<string, List<string>> kvp in idToStoryTexts)
        {
            int index = 0;
            foreach (string value in kvp.Value)
            {
                Debug.Log($"Key: {kvp.Key}, Index: {index} Value: {value}");
                ++index;
            }
        }
    }
}
