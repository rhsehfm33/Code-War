using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TMPModifier
{
    // Set transparency of character in TMP
    public static void SetTMPCharacterAlpha(TMP_Text textComponent, int characterIndex, byte alpha)
    {
        TMP_TextInfo textInfo = textComponent.textInfo;
        int materialIndex = textInfo.characterInfo[characterIndex].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[characterIndex].vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

        for (int i = 0; i < 4; i++)
        {
            vertexColors[vertexIndex + i].a = alpha;
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    // Hide text in TMP_text
    public static void HideText(TMP_Text textComponent)
    {
        Color currentColor = textComponent.color;
        currentColor.a = 0f;
        textComponent.color = currentColor;
    }
}
