using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPLanguageApplier : MonoBehaviour
{
    [SerializeField]
    private string _textId;

    void Start()
    {
        LocalizationManager.Instance.OnLanguageChanged += updateLocalizedText;
    }
    private void updateLocalizedText()
    {
        GetComponent<TMP_Text>().text = LocalizationManager.Instance.GetLocalizedText(_textId);
    }
}
