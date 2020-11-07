using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ArRetarget
{
    public class JsonFileButtonManager : MonoBehaviour
    {
        private string persistentPath;
        public GameObject recordedFileButton;
        public List<JsonFileData> JsonFileDataList = new List<JsonFileData>();

        private void Start()
        {
            persistentPath = Application.persistentDataPath;
        }

        private List<JsonFileData> GenerateList()
        {
            List<JsonFileData> jsonFileDataList = new List<JsonFileData>();
            //receiving all files at the persistent data path
            FileInfo[] files = FileManagementHelper.JsonsInDir(persistentPath);

            if (files.Length > 0)
            {
                foreach (FileInfo file in files)
                {
                    //validating the path
                    if (FileManagementHelper.ValidatePath(mediaPath: file.FullName))
                    {
                        //generating a ref
                        JsonFileData tmp = new JsonFileData()
                        {
                            filename = file.Name,
                            path = file.FullName,
                            active = false
                        };

                        jsonFileDataList.Add(tmp);
                    }
                }
            }
            return jsonFileDataList;
        }

        /// <summary>
        /// clearing the generated buttons when closing the filebrowser
        /// </summary>
        public void ClearButtonList()
        {
            foreach (JsonFileData obj in JsonFileDataList)
            {
                Destroy(obj.obj);
            }

            JsonFileDataList.Clear();
        }

        /// <summary>
        /// generating the buttons on opening file browser
        /// </summary>
        public void GenerateButtons()
        {
            JsonFileDataList = GenerateList();
            //generating buttons based on the files at the persistent data path
            for (int i = 0; i < JsonFileDataList.Count; i++)
            {
                //set file data index
                JsonFileDataList[i].index = i;
                //set json file data obj
                var obj = new GameObject();
                JsonFileDataList[i].obj = obj;

                var fileButton = new JsonFileButton();

                fileButton.InitFileButton(JsonFileDataList[i]);
            }
        }
    }

    /// <summary>
    /// data type for displayed .json files in the user interface
    /// </summary>
    public class JsonFileData
    {
        public string path;
        public string filename;
        public bool active;
        public GameObject obj;
        public int index;
    }

}