using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class StoryLineProcessor : MonoBehaviour
{
    private TMP_Text _textComponent;

    [SerializeField]
    private float fadeInDuration;

    [SerializeField]
    private float delayBetweenLetters;

    private List<GameObject> _storyContents;

    public bool IsWriting { get; private set; }

    private void Awake()
    {
        IsWriting = false;
        _textComponent = GetComponent<TMP_Text>();
    }

    public void Start()
    {
        _textComponent.text = "";
        TMPModifier.SetTMPTextAlpha(_textComponent, 0);
    }

    public void Initialize(List<GameObject> storyContent, string storyLine)
    {
        _storyContents = storyContent;
        _textComponent.text = _storyContents.Count > 0 ? "\n\n" + storyLine : storyLine;
        _textComponent.text += " ->";
        _storyContents.Add(gameObject);
    }

    public IEnumerator EndWritingAnimation()
    {
        IsWriting = false;
        yield return null;
        TMPModifier.SetTMPTextAlpha(_textComponent, 255);
        StartCoroutine(blinkArrow(_textComponent.text.Length - 2));
        StartCoroutine(blinkArrow(_textComponent.text.Length - 1));
    }

    // Start overall animation
    public IEnumerator StartWritingAniamtion()
    {
        IsWriting = true;
        _textComponent.ForceMeshUpdate();

        // Animation entry
        yield return StartCoroutine(fadeInText(fadeInDuration));
    }

    // Fade in all characters
    private IEnumerator fadeInText(float fadeInDuration)
    {
        for (int charIndex = 0; IsWriting && charIndex < _textComponent.text.Length; charIndex++)
        {
            char ch = _textComponent.text[charIndex];
            if (ch != ' ' && ch != '\r' && ch != '\n')
            {
                StartCoroutine(fadeInCharacter(charIndex, fadeInDuration));
                yield return new WaitForSeconds(delayBetweenLetters);
            }
        }
    }

    // Fade in character
    private IEnumerator fadeInCharacter(int charIndex, float duration)
    {
        // Fade in character
        float startTime = Time.time;
        while (IsWriting && Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float alpha = Mathf.Lerp(0, 255, elapsed / duration);
            TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, (byte)alpha);
            yield return null;
        }

        // Fully fade in character
        TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, 255);

        if (IsWriting && _textComponent.text.Length - 1 == charIndex)
        {
            StartCoroutine(EndWritingAnimation());
        }
    }

    private bool isThisLastLine()
    {
        return _storyContents.IndexOf(gameObject) == _storyContents.Count - 1;
    }

    private IEnumerator blinkArrow(int charIndex)
    {
        while (isThisLastLine())
        {
            float startTime = Time.time; 
            while (isThisLastLine() && Time.time - startTime < 1.0f)
            {
                float elapsed = Time.time - startTime;
                float alpha = Mathf.Lerp(255, 0, elapsed / 1.0f);
                TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, (byte)alpha);
                yield return null;
            }

            startTime = Time.time;
            while (isThisLastLine() && Time.time - startTime < 1.0f)
            {
                float elapsed = Time.time - startTime;
                float alpha = Mathf.Lerp(0, 255, elapsed / 1.0f);
                TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, (byte)alpha);
                yield return null;
            }
        }
        if (_textComponent.text.EndsWith(" ->"))
        {
            _textComponent.text = _textComponent.text[..^3];
        }
    }
}
