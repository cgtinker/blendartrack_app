using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

namespace ArRetarget
{
    public class FileBrowserEventManager : MonoBehaviour
    {
        private string persistentPath;

        [Header("Json File Button")]
        public GameObject JsonFileButtonPrefab;
        public Transform JsonFileButtonParent;

        [Header("Json Data Info List")]
        public List<JsonFileData> JsonFileDataList = new List<JsonFileData>();

        [Header("Json Viewer")]
        public GameObject JsonViewerPrefab;
        private GameObject jsonViewerReference;

        //different buttons depending on json viewer state
        [Header("Back Button in Viewer and Filebrowser")]
        public GameObject ViewerAcitveBackButton;
        public GameObject ViewerInactiveBackButton;
        public GameObject FileBrowserBackground;
        public GameObject SelectAllFilesBtn;

        [Header("Footer")]
        public GameObject SupportFooter;
        public GameObject MenuFooter;

        private void Start()
        {
            persistentPath = Application.persistentDataPath;
        }

        #region input events
        //delete selected files
        public void OnDeleteSelectedFiles()
        {
            List<string> selectedFiles = GetSelectedFilePaths();

            if (selectedFiles.Count <= 0)
                return;

            else
                FileManagement.DeleteFilesInDir(selectedFiles);
        }

        //native share event for selected files
        public void OnShareSelectedFiles()
        {
            //ref selected file
            List<string> selectedFilesNames = GetSelectedFileNames();
            List<string> selectedFilePaths = GetSelectedFilePaths();

            if (selectedFilesNames.Count <= 0)
                return;

            else
            {
                //naming
                string localDate = FileManagement.GetDateTimeText();

                string filenames = "";
                var paragraph = FileManagement.GetParagraph();

                //listing files to transfer
                foreach (string filename in selectedFilesNames)
                {
                    var curFilename = filename + paragraph;
                    filenames += curFilename;
                }

                //transfer message
                string subject = "Retarget " + localDate;
                string text = "Retarget " + localDate + paragraph + paragraph + "Attached Files: " + paragraph + filenames;

                FileManagement.ShareJsonFiles(selectedFilePaths, subject, text);
            }

        }

        private bool selectFilesBtnState = false;
        public void OnToggleSelectFiles()
        {
            selectFilesBtnState = !selectFilesBtnState;

            foreach (JsonFileData data in JsonFileDataList)
            {
                var btn = data.obj.GetComponent<JsonFileButton>();
                btn.ChangeSelectionToggleStatus(selectFilesBtnState);
            }
        }
        #endregion

        #region json viewer
        //setting all other buttons inactive
        public void OnToggleViewer(int btnIndex, bool activateViewer, string fileContents)
        {
            if (activateViewer)
            {
                Debug.Log("attempt to preview data");
                FileBrowserBackground.SetActive(false);
                SelectAllFilesBtn.SetActive(false);

                //changing the back buttons
                ViewerAcitveBackButton.SetActive(true);
                ViewerInactiveBackButton.SetActive(false);

                //changing footer
                MenuFooter.SetActive(false);
                SupportFooter.SetActive(true);

                //instantiating the viewer
                jsonViewerReference = Instantiate(JsonViewerPrefab, Vector3.zero, Quaternion.identity);
                var jsonDataImporter = jsonViewerReference.GetComponent<JsonDataImporter>();

                //open the json file and import the data to preview it
                jsonDataImporter.OpenFile(fileContents);

                //deactivating the other buttons
                foreach (JsonFileData data in JsonFileDataList)
                {
                    if (data.index != btnIndex)
                    {
                        data.obj.SetActive(false);
                    }
                }
            }

            else
            {
                DeactivateViewer();
            }
        }

        //single for back button
        public void DeactivateViewer()
        {
            Debug.Log("stop viewing data");
            FileBrowserBackground.SetActive(true);
            SelectAllFilesBtn.SetActive(true);

            //changing the back buttons
            ViewerAcitveBackButton.SetActive(false);
            ViewerInactiveBackButton.SetActive(true);

            //changing footer
            MenuFooter.SetActive(true);
            SupportFooter.SetActive(false);

            //destroying the viewer
            Destroy(jsonViewerReference);

            //reactivating the buttons
            foreach (JsonFileData data in JsonFileDataList)
            {
                if (!data.obj.activeSelf)
                {
                    data.obj.SetActive(true);
                }
            }
        }
        #endregion

        #region json file buttons
        /// <summary>
        /// generating the buttons on opening file browser
        /// </summary>
        public void GenerateButtons()
        {
            Debug.Log("Generating preview buttons");
            JsonFileDataList = GenerateList();

            //generating buttons based on the files at the persistent data path
            for (int i = 0; i < JsonFileDataList.Count; i++)
            {
                //set file data index
                JsonFileDataList[i].index = i;
                //set json file data obj
                var jsonFileBtnObj = Instantiate(JsonFileButtonPrefab, Vector3.zero, Quaternion.identity);
                JsonFileDataList[i].obj = jsonFileBtnObj;
                jsonFileBtnObj.name = JsonFileDataList[i].filename;
                //setting parent
                jsonFileBtnObj.transform.SetParent(JsonFileButtonParent);
                //setting scale
                jsonFileBtnObj.transform.localScale = Vector3.one;

                //passing data to the button script
                var fileButton = jsonFileBtnObj.GetComponent<JsonFileButton>();
                fileButton.InitFileButton(JsonFileDataList[i], gameObject.GetComponent<FileBrowserEventManager>());
            }
        }

        private List<JsonFileData> GenerateList()
        {
            List<JsonFileData> jsonFileDataList = new List<JsonFileData>();
            //receiving all files at the persistent data path
            FileInfo[] files = FileManagement.JsonsInDir(persistentPath);

            if (files.Length > 0)
            {
                foreach (FileInfo file in files)
                {
                    //validating the path
                    if (FileManagement.ValidatePath(mediaPath: file.FullName))
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
        /// get all selected files
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedFileNames()
        {
            List<string> tmp_list = new List<string>();

            foreach (JsonFileData data in JsonFileDataList)
            {
                if (data.active)
                {
                    tmp_list.Add(data.filename);
                }
            }

            return tmp_list;
        }

        public List<string> GetSelectedFilePaths()
        {
            List<string> tmp_list = new List<string>();

            foreach (JsonFileData data in JsonFileDataList)
            {
                if (data.active)
                {
                    tmp_list.Add(data.path);
                }
            }

            return tmp_list;
        }

        /// <summary>
        /// clearing the generated buttons when closing the filebrowser
        /// </summary>
        public void ClearButtonList()
        {
            Debug.Log("Clearing preview buttons");
            foreach (JsonFileData obj in JsonFileDataList)
            {
                Destroy(obj.obj);
            }

            JsonFileDataList.Clear();
        }
        #endregion
    }

    /// <summary>
    /// data type for displayed .json files in the user interface
    /// </summary>
    [System.Serializable]
    public class JsonFileData
    {
        public string path;
        public string filename;
        public bool active;
        public GameObject obj;
        public int index;
    }

}