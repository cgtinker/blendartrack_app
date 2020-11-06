using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
    public class JsonDataImporter : MonoBehaviour
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
            //deserialzing the data
            CameraPoseContainer data = JsonUtility.FromJson<CameraPoseContainer>(fileContent);
            //generating a parent game object
            GameObject parent = ParentGameObject();
            //adding the importer component - assigning the update method
            var importer = parent.AddComponent<CameraPoseImporter>();
            importer.viewHandler = this.gameObject.GetComponent<UpdateViewerDataHandler>();

            StartCoroutine(importer.InitViewer(data));
        }

        public IEnumerator ImportBlendShapeData(string fileContent)
        {
            yield return new WaitForEndOfFrame();
            BlendShapeContainter data = JsonUtility.FromJson<BlendShapeContainter>(fileContent);
        }

        public IEnumerator ImportFaceMeshData(string fileContent)
        {
            yield return new WaitForEndOfFrame();
            //deserialzing the data
            MeshDataContainer data = JsonUtility.FromJson<MeshDataContainer>(fileContent);
            //generating a parent game object
            GameObject parent = ParentGameObject();
            //adding the importer component - assigning the update method
            var importer = parent.AddComponent<FaceMeshImporter>();
            importer.viewHandler = this.gameObject.GetComponent<UpdateViewerDataHandler>();

            StartCoroutine(importer.InitViewer(data));
        }

        public GameObject ParentGameObject()
        {
            GameObject importParent = new GameObject("importParent");
            importParent.transform.SetParent(this.gameObject.transform);

            return importParent;
        }

        #region validation
        public void OpenFile(string fileContent)
        {
            ValidationResponse response = ValidateJsonFile(fileContent);
            Debug.Log("method: " + response.MethodName + ", title: " + response.StringTitle + " valid: " + response.Successful);

            if (response.Successful)
            {
                //starting the coroutine based on the dict
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
            //dict key = method name || value = title in json
            foreach (KeyValuePair<string, string> refDict in JsonType)
            {
                if (jsonString.Contains(refDict.Value) == true)
                {
                    validationResponse.StringTitle = refDict.Value;
                    validationResponse.MethodName = refDict.Key;
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