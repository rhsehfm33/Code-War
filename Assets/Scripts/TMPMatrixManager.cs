using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPMatrixManager : MonoBehaviour
{
    public Transform canvasTransform;
    public GameObject matrixTMP;
    public int matrixTMPCount;
    public float intervalRange;

    void Start()
    {
        for (int i = 0; i < matrixTMPCount; ++i)
        {
            StartCoroutine(StartBackgroundMatrixAnimation());
        }
    }

    IEnumerator StartBackgroundMatrixAnimation()
    {
        GameObject tmpMatrixInstance = Instantiate(matrixTMP, canvasTransform);

        yield return null;

        tmpMatrixInstance.transform.SetSiblingIndex(1);
        int parentWidth = (int)transform.parent.GetComponent<RectTransform>().rect.width;
        RectTransform rectTransform = tmpMatrixInstance.GetComponent<RectTransform>();
        TMPMatrixAnimation tmpMatrixAnimation = tmpMatrixInstance.GetComponent<TMPMatrixAnimation>();
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, intervalRange));
            rectTransform.anchoredPosition = new Vector2((int)Random.Range(-17, 18) * 30, 0);
            rectTransform.pivot = new Vector2((float)0.5, 1);
            yield return StartCoroutine(tmpMatrixAnimation.StartMatrixAniamtion());
        }
    }
}
