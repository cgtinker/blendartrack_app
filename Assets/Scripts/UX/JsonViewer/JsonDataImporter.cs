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

        private IEnumerator ImportPoseData(string fileContent)
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

        private IEnumerator ImportBlendShapeData(string fileContent)
        {
            yield return new WaitForEndOfFrame();
            BlendShapeContainter data = JsonUtility.FromJson<BlendShapeContainter>(fileContent);
        }

        private IEnumerator ImportFaceMeshData(string fileContent)
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

        private GameObject ParentGameObject()
        {
            GameObject importParent = new GameObject("importParent");
            importParent.transform.SetParent(this.gameObject.transform);

            return importParent;
        }

        #region validation
        public void OpenFile(string fileContent)
        {
            //validation for imported files (only)
            ValidationResponse response = ValidateJsonFile(fileContent);
            Debug.Log("method: " + response.MethodName + ", title: " + response.JsonDataType + " valid: " + response.Successful);

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

        /// <summary>
        /// json data type has to be defined in the JsonType-Dict
        /// the json string may contains a list with the json data type
        /// returns the method to invoke, the json data type
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private static ValidationResponse ValidateJsonFile(string jsonString)
        {
            ValidationResponse validationResponse = new ValidationResponse()
            {
                JsonDataType = "",
                Successful = false
            };
            //dict key = method name || value = title in json
            foreach (KeyValuePair<string, string> refDict in JsonType)
            {
                if (jsonString.Contains(refDict.Value) == true)
                {
                    validationResponse.JsonDataType = refDict.Value;
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
        public string JsonDataType { get; set; }
        public string MethodName { get; set; }
    }

}