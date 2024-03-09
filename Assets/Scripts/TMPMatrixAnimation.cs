using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TMPMatrixAnimation : MonoBehaviour
{
    public TMP_Text textComponent; // TMP instance this script attached to
    public int letterLength = 100;
    public float fadeDuration = 1f; // Duration of the fade effect
    public float delayBetweenLetters = 0.05f;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        StartCoroutine(StartMatrixAniamtion());
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

    // Start overall animation repeatedly
    IEnumerator StartMatrixAniamtion()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.3f));

        while (true)
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
            yield return new WaitForSeconds(Random.Range(fadeDuration, fadeDuration + 0.3f));
        }
    }

    // Fade out Text
    private IEnumerator FadeOutText(float fadeOutDuration)
    {
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            StartCoroutine(FadeOutCharacter(i, fadeOutDuration));
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    // Fade out character
    private IEnumerator FadeOutCharacter(int characterIndex, float duration)
    {
        float startTime = Time.time;
        var textInfo = textComponent.textInfo;
        var charInfo = textInfo.characterInfo[characterIndex];

        // Only run if character index is valid
        if (!charInfo.isVisible)
        {
            yield break;
        }

        // Set it fully visible first
        SetTMPCharacterAlpha(textInfo, characterIndex, 255);

        startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float elapsed = Time.time - startTime;
            float alpha = Mathf.Lerp(255, 0, elapsed / duration);
            SetTMPCharacterAlpha(textInfo, characterIndex, (byte)alpha);
            yield return null;
        }

        // Makes Character fully transparent
        SetTMPCharacterAlpha(textInfo, characterIndex, 0);
    }
}
