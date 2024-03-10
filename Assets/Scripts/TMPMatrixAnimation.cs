using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TMPMatrixAnimation : MonoBehaviour
{
    private TMP_Text textComponent; // TMP instance this script attached to
    public int letterLength = 100;
    public float fadeDuration = 1f; // Duration of the fade effect
    public float delayBetweenLetters = 0.05f;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    // Set transparency of character in TMP
    private void SetTMPCharacterAlpha(TMP_TextInfo textInfo, int characterIndex, byte alpha)
    {
        int materialIndex = textInfo.characterInfo[characterIndex].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[characterIndex].vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

        for (int i = 0; i < 4; i++)
        {
            vertexColors[vertexIndex + i].a = alpha;
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    // Start overall animation
    public IEnumerator StartMatrixAniamtion()
    {
        textComponent.text = "";
        for (int charIndex = 0; charIndex < letterLength; ++charIndex)
        {
            textComponent.text += (char)Random.Range(97, 113);
        }
        textComponent.ForceMeshUpdate();

        for (int charIndex = 0; charIndex < letterLength; ++charIndex)
        {
            SetTMPCharacterAlpha(textComponent.textInfo, charIndex, 0);
        }
        yield return StartCoroutine(FadeOutText(fadeDuration));
    }

    // Fade out Text
    private IEnumerator FadeOutText(float fadeOutDuration)
    {
        var textInfo = textComponent.textInfo;

        for (int charIndex = 0; charIndex < textInfo.characterCount; charIndex++)
        {
            StartCoroutine(FadeOutCharacter(charIndex, fadeOutDuration));
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    // Fade out character
    private IEnumerator FadeOutCharacter(int charIndex, float duration)
    {
        float startTime = Time.time;
        var textInfo = textComponent.textInfo;
        var charInfo = textInfo.characterInfo[charIndex];

        // Set it fully visible first
        SetTMPCharacterAlpha(textInfo, charIndex, 255);

        // Fade out character
        startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float alpha = Mathf.Lerp(255, 0, elapsed / duration);
            SetTMPCharacterAlpha(textInfo, charIndex, (byte)alpha);
            yield return null;
        }

        // Fully fade out character
        SetTMPCharacterAlpha(textInfo, charIndex, 0);
    }
}
