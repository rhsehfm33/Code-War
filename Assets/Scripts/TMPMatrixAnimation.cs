using System.Collections;
using TMPro;
using UnityEngine;

public class TMPMatrixAnimation : MonoBehaviour
{
    private TMP_Text _textComponent; // TMP instance this script attached to

    [SerializeField] private int letterLength;

    [SerializeField] private float fadeOutDuration; // Duration of the fade effect

    [SerializeField] private float delayBetweenLetters;

    void Start()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    // Start overall animation
    public IEnumerator StartMatrixAniamtion()
    {
        TMPModifier.HideText(_textComponent);

        // Initialize random text
        _textComponent.text = "";
        for (int charIndex = 0; charIndex < letterLength; ++charIndex)
        {
            _textComponent.text += (char)Random.Range(97, 113);
        }

        // Animation entry
        _textComponent.ForceMeshUpdate();
        yield return StartCoroutine(FadeOutText(fadeOutDuration));
    }

    // Fade out all characters
    private IEnumerator FadeOutText(float fadeOutDuration)
    {
        var textInfo = _textComponent.textInfo;

        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            StartCoroutine(FadeOutCharacter(charIndex, fadeOutDuration));
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    // Fade out character
    private IEnumerator FadeOutCharacter(int charIndex, float duration)
    {
        // Set it fully visible first
        TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, 255);

        // Fade out character
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float alpha = Mathf.Lerp(255, 0, elapsed / duration);
            TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, (byte)alpha);
            yield return null;
        }

        // Fully fade out character
        TMPModifier.SetTMPCharacterAlpha(_textComponent, charIndex, 0);
    }
}
