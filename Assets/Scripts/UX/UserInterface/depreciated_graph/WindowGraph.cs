using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class WindowGraph : MonoBehaviour
{
    [SerializeField]
    private Sprite spriteDot = null;
    [SerializeField]
    private RectTransform graphContainer = null;

    private void Awake()
    {
        List<float> intList = new List<float>()
        {
            10, 100, 500, 100, 40, 20 ,30, 10, 5, 21
        };
        ShowGraph(intList);
    }

    private GameObject CreateDot(Vector2 anchoredPos)
    {
        //generating sprite
        GameObject obj = new GameObject("dot", typeof(Image));
        obj.transform.SetParent(graphContainer, false);
        obj.GetComponent<Image>().sprite = spriteDot;

        //sprite position
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(40, 40);
        //lower left corner
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return obj;
    }

    public float FindMaxValue(List<float> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Empty list");
        }

        float maxFloat = float.MinValue;
        foreach (float type in list)
        {
            if (type > maxFloat)
            {
                maxFloat = type;
            }
        }
        return maxFloat;
    }

    private void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        float yMax = FindMaxValue(valueList);
        float xStep = graphWidth / valueList.Count;

        GameObject lastDot = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = i * xStep + xStep / 2;
            float yPos = (valueList[i] / yMax) * graphHeight;
            GameObject dot = CreateDot(new Vector2(xPos, yPos));

            if (lastDot != null)
            {
                Vector2 dotPosA = dot.GetComponent<RectTransform>().anchoredPosition;
                Vector2 dotPosB = lastDot.GetComponent<RectTransform>().anchoredPosition;
                CreateDotConnection(dotPosA: dotPosA, dotPosB: dotPosB);
            }

            lastDot = dot;
        }
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject obj = new GameObject("dotConnection", typeof(Image));
        obj.transform.SetParent(graphContainer, false);
        //color
        obj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);

        //pos
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = dotPosA;
        //dir
        Vector2 direction = (dotPosA - dotPosB).normalized;
        float distance = Vector2.Distance(dotPosA, dotPosB);

        //rectTransform.

        //lower left corner
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        //size
        rectTransform.sizeDelta = new Vector2(100f, 3f);

    }
}
