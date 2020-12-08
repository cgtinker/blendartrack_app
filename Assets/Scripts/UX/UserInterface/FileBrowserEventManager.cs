using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

namespace ArRetarget
{
    public class FileBrowserEventManager : MonoBehaviour
    {
        #region references
        private string persistentPath;

        [Header("Json File Button")]
        public GameObject JsonFileButtonPrefab;
        public Transform JsonFileButtonParent;

        [Header("Json Data Info List")]
        //public List<JsonFileData> JsonFileDataList = new List<JsonFileData>();
        public List<JsonDirectory> JsonDirectories = new List<JsonDirectory>();

        [Header("Json Viewer")]
        public GameObject JsonViewerPrefab;
        private GameObject jsonViewerReference;

        //different buttons depending on json viewer state
        [Header("Back Button in Viewer and Filebrowser")]
        public GameObject ViewerAcitveBackButton;
        public GameObject ViewerInactiveBackButton;
        public GameObject FileBrowserBackground;
        //public GameObject SelectAllFilesBtn;

        [Header("Footer")]
        public GameObject SupportFooter;
        public GameObject MenuFooter;
        #endregion

        private void Start()
        {
            persistentPath = Application.persistentDataPath;
        }

        #region input events
        //delete selected files
        public void OnDeleteSelectedFiles()
        {
            List<string> selectedFiles = GetSelectedDirectories();

            if (selectedFiles.Count <= 0)
                return;

            else
                FileManagement.DeleteDirectories(selectedFiles);
        }

        //native share event for selected files
        public void OnShareSelectedFiles()
        {
            //ref selected file
            List<string> selectedDirNames = GetSelectedDirectoryNames();
            List<string> selectedDirPaths = GetSelectedDirectories();

            if (selectedDirNames.Count <= 0)
                return;

            else
            {
                //date time reference
                string localDate = FileManagement.GetDateTimeText();

                //path to generated zip
                string zip = FileManagement.CompressDirectories(selectedDirPaths);

                //listing files to transfer for subject message
                string filenames = "";
                var paragraph = FileManagement.GetParagraph();
                foreach (string filename in selectedDirPaths)
                {
                    var curFilename = filename + paragraph;
                    filenames += curFilename;
                }

                //setting up share message / text
                string subject = "Retarget " + localDate;
                string text = "Retarget " + localDate + paragraph + paragraph + "Attached Files: " + paragraph + filenames;

                //share data
                FileManagement.ShareZip(zip, subject, text);
                StartCoroutine(DeleteZip(zip));
            }
        }

        private bool selectFilesBtnState = false;
        public void OnToggleSelectFiles()
        {
            selectFilesBtnState = !selectFilesBtnState;

            foreach (JsonDirectory data in JsonDirectories)
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
                //SelectAllFilesBtn.SetActive(false);

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
                foreach (JsonDirectory data in JsonDirectories)
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
            //SelectAllFilesBtn.SetActive(true);

            //changing the back buttons
            ViewerAcitveBackButton.SetActive(false);
            ViewerInactiveBackButton.SetActive(true);

            //changing footer
            MenuFooter.SetActive(true);
            SupportFooter.SetActive(false);

            //destroying the viewer
            Destroy(jsonViewerReference);

            //reactivating the buttons
            foreach (JsonDirectory data in JsonDirectories)
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
            JsonDirectories = GetDirectories();

            for (int i = 0; i < JsonDirectories.Count; i++)
            {
                //set file data index
                JsonDirectories[i].index = i;
                //set json file data obj
                var jsonFileBtnObj = Instantiate(JsonFileButtonPrefab, Vector3.zero, Quaternion.identity);
                JsonDirectories[i].obj = jsonFileBtnObj;
                jsonFileBtnObj.name = JsonDirectories[i].dirName;
                //setting parent
                jsonFileBtnObj.transform.SetParent(JsonFileButtonParent);
                //setting scale
                jsonFileBtnObj.transform.localScale = Vector3.one;

                //passing data to the button script
                var fileButtonScript = jsonFileBtnObj.GetComponent<JsonFileButton>();
                fileButtonScript.InitFileButton(JsonDirectories[i], gameObject.GetComponent<FileBrowserEventManager>());
            }
        }


        #region Get Directories
        //dir
        private List<JsonDirectory> GetDirectories()
        {
            //checking for sub dirs
            string[] dirs = FileManagement.GetDirectories(persistentPath);
            List<JsonDirectory> jsonDirectories = new List<JsonDirectory>();

            if (dirs.Length > 0)
            {
                for (int i = 0; i < dirs.Length; i++)
                {
                    //adding all dirs to list for visibilty
                    JsonDirectory m_dir = new JsonDirectory();
                    m_dir.dirName = "empty or corrupted";
                    m_dir.dirPath = dirs[i];

                    jsonDirectories.Add(m_dir);

                    //only create pointers to folders including following suffixes
                    string[] suffixes = { "CP", "FM", "BS" };

                    foreach (string suffix in suffixes)
                    {
                        if (FileManagement.StringEndsWith(dirs[i], suffix))
                        {
                            //create new dir obj pointing to json
                            m_dir = SetupDirectoryPointerToJson(dirs[i], suffix);
                            //overwritting
                            jsonDirectories[i] = m_dir;
                        }
                    }
                }
            }

            return jsonDirectories;
        }

        //subdir
        private JsonDirectory SetupDirectoryPointerToJson(string path, string suffix)
        {
            JsonDirectory m_dir = new JsonDirectory();

            //looping through sub dir
            if (FileManagement.ValidateDirectory(path))
            {
                //assign for empty folders
                m_dir.dirName = "empty folder";
                m_dir.dirPath = path;

                //jsons in sub dir
                FileInfo[] jsonFiles = FileManagement.GetJsonsAtPath(path);
                foreach (FileInfo json in jsonFiles)
                {
                    string jsonFilename = json.Name;

                    //setup JsonDirectoryData
                    if (FileManagement.StringEndsWith(jsonFilename, suffix + ".json"))
                    {
                        //setting up the serializeable JsonDir obj
                        m_dir.dirName = FileManagement.RemoveFromEnd(json.Name, ".json");
                        m_dir.active = false;
                        m_dir.jsonFilePath = json.FullName;
                    }
                }

                return m_dir;
            }

            else
            {
                Debug.LogError("Directory doesn't contain a valid json");
                return m_dir;
            }
        }
        #endregion

        #endregion

        #region get selection file info
        /// <summary>
        /// get all selected dir names
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedDirectoryNames()
        {
            List<string> tmp_list = new List<string>();

            foreach (JsonDirectory data in JsonDirectories)
            {
                if (data.active)
                {
                    tmp_list.Add(data.dirName);
                }
            }

            return tmp_list;
        }
        /// <summary>
        /// get all selected directory paths
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedDirectories()
        {
            List<string> tmp_list = new List<string>();

            foreach (JsonDirectory data in JsonDirectories)
            {
                if (data.active)
                {
                    tmp_list.Add(data.dirPath);
                }
            }

            return tmp_list;
        }
        #endregion

        /// <summary>
        /// clearing the generated buttons when closing the filebrowser
        /// </summary>
        public void ClearButtonList()
        {
            Debug.Log("Clearing preview buttons");
            foreach (JsonDirectory obj in JsonDirectories)
            {
                Destroy(obj.obj);
            }

            JsonDirectories.Clear();
        }

        public IEnumerator DeleteZip(string zipPath)
        {
            yield return new WaitForSeconds(5.0f);
            FileManagement.DeleteFile(zipPath);
        }
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

    [System.Serializable]
    public class JsonDirectory
    {
        public string dirPath;
        public string dirName;
        public string jsonFilePath;
        public GameObject obj;
        public bool active;
        public int index;
    }
}