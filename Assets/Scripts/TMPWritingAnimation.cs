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


    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    // Start overall animation
    public IEnumerator StartWritingAniamtion()
    {
        // Animation entry
        _textComponent.ForceMeshUpdate();
        yield return StartCoroutine(FadeInText(fadeInDuration));
    }

    public void ClearWritingAnimation()
    {
        _textComponent.text = "";
        TMPModifier.HideText(_textComponent);
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
