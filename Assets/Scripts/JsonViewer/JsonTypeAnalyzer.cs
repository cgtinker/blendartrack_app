using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArRetarget;
using System;
using System.Linq;

namespace ArRetarget
{

    public class JsonTypeAnalyzer : MonoBehaviour
    {
        public string path;
        public TextAsset jsonFile;

        private static Dictionary<string, string> JsonType = new Dictionary<string, string>()
    {
        { "ImportPoseData", "cameraPoseList" },
        { "ImportFaceMeshData", "meshDataList" },
        { "ImportBlendShapeData", "blend" }
    };

        private void Start()
        {
            string fileContent = jsonFile.text;
            OpenFile(fileContent);
        }

        public IEnumerator ImportPoseData(string fileContent)
        {
            yield return new WaitForEndOfFrame();
            CameraPoseContainer data = JsonUtility.FromJson<CameraPoseContainer>(fileContent);

            var importParent = new GameObject("importParent");
            importParent.transform.SetParent(this.gameObject.transform);
            var importer = importParent.AddComponent<CameraPoseImporter>();
            importer.viewHandler = this.gameObject.GetComponent<UpdateViewerDataHandler>();
            StartCoroutine(importer.Init(data));
        }

        public IEnumerator ImportBlendShapeData(string fileContent)
        {
            yield return new WaitForEndOfFrame();
            BlendShapeContainter data = JsonUtility.FromJson<BlendShapeContainter>(fileContent);
        }

        public IEnumerator ImportFaceMeshData(string fileContent)
        {
            yield return new WaitForEndOfFrame();
            MeshDataContainer data = JsonUtility.FromJson<MeshDataContainer>(fileContent);

            //var importParent = new GameObject("importParent");
            //importParent.transform.SetParent(this.gameObject.transform);
            //var importer = importParent.AddComponent<FaceMeshImporter>();
            //importer.viewHandler = this.gameObject.GetComponent<UpdateViewerDataHandler>();
            //StartCoroutine(importer.InitJsonViewer(data));


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

        #region validation
        public void OpenFile(string fileContent)
        {
            ValidationResponse response = ValidateJsonFile(fileContent);
            Debug.Log("method: " + response.MethodName + ", title: " + response.StringTitle + " valid: " + response.Successful);

            if (response.Successful)
            {
                StartCoroutine(response.MethodName, fileContent);
            }

            else
            {
                Debug.LogError("Cannot invoke Input method - Json File probaly corrupted");
            }
        }

        private static ValidationResponse ValidateJsonFile(string jsonString)
        {
            ValidationResponse validationResponse = new ValidationResponse()
            {
                StringTitle = "",
                Successful = false
            };

            foreach (KeyValuePair<string, string> pair in JsonType)
            {
                if (jsonString.Contains(pair.Value) == true)
                {
                    validationResponse.StringTitle = pair.Value;
                    validationResponse.MethodName = pair.Key;
                    validationResponse.Successful = true;
                }
            }

            return validationResponse;
        }
        #endregion
    }

    public class ValidationResponse
    {
        public bool Successful { get; set; }
        public string StringTitle { get; set; }
        public string MethodName { get; set; }
    }
}