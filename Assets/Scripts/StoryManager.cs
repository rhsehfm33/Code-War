using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text _storyTextComponent;
    private int _storyId = 1;

    void Start()
    {
        StartCoroutine(ProceedStoryByChunk());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ProceedStoryByChunk());
    }

    IEnumerator ProceedStoryByChunk()
    {
        TMPWritingAnimation tmpWritingAnimation = _storyTextComponent.gameObject.GetComponent<TMPWritingAnimation>();
        if (tmpWritingAnimation.IsWriting)
        {
            StartCoroutine(tmpWritingAnimation.SkipWritingAnimation());
            yield break;
        }

        tmpWritingAnimation.InitializeWritingAnimation();
        yield return null;

        StringBuilder stringBuilder = new StringBuilder(" ");
        string locationX = PlayerInfoManager.Instance.locationX.ToString();
        string locationY = PlayerInfoManager.Instance.locationY.ToString();
        while (IsContinueShowing())
        {
            string textId = $"map-story.{locationX}-{locationY}.{_storyId}";
            string storyLine = LocalizationManager.Instance.GetLocalizedText(textId);
            if (storyLine == null)
            {
                break;
            }

            stringBuilder.Append(storyLine + "\n\n ");
            _storyTextComponent.text = stringBuilder.ToString();
            yield return null;

            ++_storyId;
        }

        _storyTextComponent.text = _storyTextComponent.text[..^3];
        yield return StartCoroutine(tmpWritingAnimation.StartWritingAniamtion());
    }

    bool IsContinueShowing()
    {
        return (_storyTextComponent.rectTransform.rect.height <= _storyTextComponent.fontSize * 13);
    }
}
