using System.Collections;
using TMPro;
using UnityEngine;

public class TMPWritingAnimation : MonoBehaviour
{
    private TMP_Text _textComponent;

    [SerializeField]
    private float fadeInDuration;

    [SerializeField]
    private float delayBetweenLetters;

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

    public IEnumerator SkipWritingAnimation()
    {
        IsWriting = false;
        yield return null;
        TMPModifier.SetTMPTextAlpha(_textComponent, 255);
    }

    // Start overall animation
    public IEnumerator StartWritingAniamtion()
    {
        // Animation entry
        IsWriting = true;
        _textComponent.ForceMeshUpdate();
        yield return StartCoroutine(FadeInText(fadeInDuration));
    }

    // Fade in all characters
    private IEnumerator FadeInText(float fadeInDuration)
    {
        for (int charIndex = 0; IsWriting && charIndex < _textComponent.text.Length; charIndex++)
        {
            char ch = _textComponent.text[charIndex];
            if (ch != ' ' && ch != '\r' && ch != '\n')
            {
                StartCoroutine(FadeInCharacter(charIndex, fadeInDuration));
                yield return new WaitForSeconds(delayBetweenLetters);
            }
        }
    }

    // Fade in character
    private IEnumerator FadeInCharacter(int charIndex, float duration)
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

        if (_textComponent.text.Length - 1 == charIndex)
        {
            IsWriting = false;
        }
    }
}
