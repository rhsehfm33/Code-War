using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text storyTextComponent;
    private StringBuilder stringBuilder;
    private int storyId = 1;

    void Start()
    {
        stringBuilder = new StringBuilder();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ProceedStoryByChunk());
    }

    IEnumerator ProceedStoryByChunk()
    {
        TMPWritingAnimation tmpWritingAnimation = storyTextComponent.gameObject.GetComponent<TMPWritingAnimation>();

        tmpWritingAnimation.ClearWritingAnimation();
        stringBuilder.Clear();
        yield return null;

        string locationX = PlayerInfoManager.Instance.locationX.ToString();
        string locationY = PlayerInfoManager.Instance.locationY.ToString();
        while (IsContinueShowing())
        {
            string textId = $"map-story.{locationX}-{locationY}.{storyId}";
            string storyLine = LocalizationManager.Instance.GetLocalizedText(textId);
            if (storyLine == null)
            {
                break;
            }

            stringBuilder.Append(storyLine);
            storyTextComponent.text = stringBuilder.ToString();
            yield return null;

            ++storyId;
        }
        yield return StartCoroutine(tmpWritingAnimation.StartWritingAniamtion());
    }

    bool IsContinueShowing()
    {
        return (storyTextComponent.rectTransform.rect.height < 402);
    }
}
