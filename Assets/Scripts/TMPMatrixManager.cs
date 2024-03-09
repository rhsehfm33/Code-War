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

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, intervalRange));
            RectTransform rectTransform = tmpMatrixInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2((int)Random.Range(-17, 18) * 30, 1080);
            TMPMatrixAnimation tmpMatrixAnimation = tmpMatrixInstance.GetComponent<TMPMatrixAnimation>();
            yield return StartCoroutine(tmpMatrixAnimation.StartMatrixAniamtion());
        }
    }
}
