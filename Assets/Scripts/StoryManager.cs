using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject _storyContentPrefab;

    [SerializeField]
    private int _sentenceLimit;

    private List<GameObject> _storyContents = new List<GameObject>();

    private int _storyId = 1;

    void Start()
    {
        StartCoroutine(ProceedStoryByLine());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ProceedStoryByLine());
    }

    IEnumerator ProceedStoryByLine()
    {
        if (_storyContents.Count > 0)   // Skip writing animation if last story line is wirting
        {
            TMPWritingAnimation currentWriting = _storyContents[_storyContents.Count - 1].GetComponent<TMPWritingAnimation>();
            if (currentWriting.IsWriting)
            {
                StartCoroutine(currentWriting.EndWritingAnimation());
                yield break;
            }
            else
            {
                currentWriting.IsPassed = true;
            }
        }
        if (_storyContents.Count == _sentenceLimit) // Clear story lines
        {
            foreach (GameObject storyContent in _storyContents)
            {
                Destroy(storyContent);
            }
            _storyContents.Clear();
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

        // Instantiate story line gameObject
        GameObject newStoryContent = Instantiate(_storyContentPrefab, this.transform);
        yield return null;

        // Set story line as text and start writing animation
        TMP_Text newStoryTmpText = newStoryContent.GetComponent<TMP_Text>();
        TMPWritingAnimation newWritingAnimation = newStoryContent.GetComponent< TMPWritingAnimation>();
        newStoryTmpText.text = _storyContents.Count > 0 ? "\n\n" + storyLine : storyLine;
        _storyContents.Add(newStoryContent);
        _storyId++;
        yield return StartCoroutine(newWritingAnimation.StartWritingAniamtion());
    }
}
