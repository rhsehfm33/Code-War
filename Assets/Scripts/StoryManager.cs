using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoryManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    GameObject _storyContentPrefab;

    [SerializeField]
    GameObject _storyContentContainer;

    [SerializeField]
    private int _sentenceLimit;

    private List<GameObject> _storyContents = new List<GameObject>();

    private int _storyId = 0;

    private float _pointerDownTime;

    void Start()
    {
        StartCoroutine(ProceedStoryContent());
    }

    // When pointer is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownTime = Time.time;
    }

    // When pointer is released
    public void OnPointerUp(PointerEventData eventData)
    {
        // pressing time
        float pointerUpTime = Time.time;
        float interval = pointerUpTime - _pointerDownTime;

        // consider as click if pressing time is short
        if (interval <= 0.15f)
        {
            // Show more story line
            StartCoroutine(ProceedStoryContent());
        }
    }

    IEnumerator ProceedStoryContent()
    {
        if (_storyContents.Count > 0)   // Skip writing animation if last story line is wirting
        {
            StoryLineProcessor currentWriting = _storyContents[_storyContents.Count - 1].GetComponent<StoryLineProcessor>();
            if (currentWriting.IsWriting)
            {
                StartCoroutine(currentWriting.EndWritingAnimation());
                yield break;
            }
        }

        // Fetching stroy line
        string locationX = PlayerInfoManager.Instance.locationX.ToString();
        string locationY = PlayerInfoManager.Instance.locationY.ToString();
        string textId = $"map-story.{locationX}-{locationY}.{_storyId}";
        string storyLine = LocalizationManager.Instance.GetLocalizedText(textId);
        if (storyLine == null)
        {
            yield break;
        }

        yield return StartCoroutine(ProceedStoryLine(storyLine));
    }

    IEnumerator ProceedStoryLine(string storyLine)
    {
        // Instantiate story line gameObject
        GameObject newStoryContent = Instantiate(_storyContentPrefab, _storyContentContainer.transform);
        yield return null;

        // Set story line text
        StoryLineProcessor storyLineProcessor = newStoryContent.GetComponent<StoryLineProcessor>();
        storyLineProcessor.Initialize(_storyContents, storyLine);
        _storyId++;

        yield return new WaitForEndOfFrame();

        ScrollRect scrollRect = gameObject.GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0;
        yield return StartCoroutine(storyLineProcessor.StartWritingAniamtion());
    }
}
