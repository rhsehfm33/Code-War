using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class StoryLineAnimation : MonoBehaviour
{
    private TMP_Text _textComponent;

    [SerializeField]
    private float fadeInDuration;

    [SerializeField]
    private float delayBetweenLetters;

    public bool IsWriting { get; private set; }
    public bool IsPassed;

    private void Awake()
    {
        IsPassed = false;
        IsWriting = false;
        _textComponent = GetComponent<TMP_Text>();
    }

    public void Start()
    {
        _textComponent.text = "";
        TMPModifier.SetTMPTextAlpha(_textComponent, 0);
    }

    public IEnumerator EndWritingAnimation()
    {
        IsWriting = false;
        yield return null;
        TMPModifier.SetTMPTextAlpha(_textComponent, 255);
        StartCoroutine(startBlinking(_textComponent.text.Length - 2));
        StartCoroutine(startBlinking(_textComponent.text.Length - 1));
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

    private IEnumerator startBlinking(int charIndex)
    {
        while (!IsPassed)
        {
            float startTime = Time.time; 
            while (!IsPassed && Time.time - startTime < 1.0f)
            {
                float elapsed = Time.time - startTime;
                float alpha = Mathf.Lerp(255, 0, elapsed / 1.0f);
                TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, (byte)alpha);
                yield return null;
            }

            startTime = Time.time;
            while (!IsPassed && Time.time - startTime < 1.0f)
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
