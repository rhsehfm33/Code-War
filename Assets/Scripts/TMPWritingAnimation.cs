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

    void Start()
    {
        _textComponent = GetComponent<TMP_Text>();
        StartCoroutine(StartWritingAniamtion("Apratments are too much.\r\nI can feel the green."));
    }

    // Start overall animation
    public IEnumerator StartWritingAniamtion(string text)
    {
        TMPModifier.HideText(_textComponent);

        _textComponent.text = text;

        // Animation entry
        _textComponent.ForceMeshUpdate();
        yield return StartCoroutine(FadeInText(fadeInDuration));
    }

    // Fade in all characters
    private IEnumerator FadeInText(float fadeInDuration)
    {
        for (int charIndex = 0; charIndex < _textComponent.text.Length; charIndex++)
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
        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float alpha = Mathf.Lerp(0, 255, elapsed / duration);
            TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, (byte)alpha);
            yield return null;
        }

        // Fully fade in character
        TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, 255);
    }
}
