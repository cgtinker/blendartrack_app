using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArRetarget;
using System;

public class ImportFaceMeshGraph : MonoBehaviour
{
    public IEnumerator ImportFaceMeshData(MeshDataContainer data)
    {
        yield return new WaitForEndOfFrame();

        int totalFrames = data.meshDataList.Count;
        int totalInputs = data.meshDataList[25].pos.Count;
        int maxValue = 1;

        for (int graph = 0; graph < totalInputs; graph++)
        {
            List<Vector2> GraphData = new List<Vector2>();
            for (int frame = 0; frame < data.meshDataList.Count; frame++)
            {
                Vector3 tmp = new Vector3(data.meshDataList[frame].pos[graph].x, data.meshDataList[frame].pos[graph].y, data.meshDataList[frame].pos[graph].z);
                float mag = tmp.sqrMagnitude * 100;

                var offsetFrame = (int)(frame - (totalFrames / 2));
                var offsetVector = mag - (0.65f);
                GraphData.Add(new Vector2(offsetFrame, offsetVector));
            }


            LineRendererHUD lineRenderer = GenerateGraph();
            lineRenderer.points = GraphData;
            lineRenderer.thickness = 5;
            //grid size = x = frame amout, y max value
            lineRenderer.gridSize = new Vector2Int(totalFrames, maxValue);
            lineRenderer.color = new Color(1, 0, 0, 0.15f);
        }

    }

    public LineRendererHUD GenerateGraph()
    {
        GameObject graph = new GameObject("graph");
        graph.AddComponent<CanvasRenderer>();
        LineRendererHUD lineRenderer = graph.AddComponent<LineRendererHUD>();

        //lower left corner
        RectTransform rectTransform = graph.GetComponent<RectTransform>();
        //skretched mode
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.sizeDelta = new Vector2(0, 0);
        rectTransform.position = new Vector2(0, 0);

        graph.transform.SetParent(this.gameObject.transform, false);

        return lineRenderer;

    }

    public float FindMaxValue(List<Vector2> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Empty list");
        }

        float maxFloat = float.MinValue;
        foreach (Vector2 type in list)
        {
            if (type.y > maxFloat)
            {
                maxFloat = type.y;
            }
        }
        return maxFloat;
    }
}
