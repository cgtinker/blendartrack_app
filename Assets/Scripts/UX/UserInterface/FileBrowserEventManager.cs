using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;
using TMPro;
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
        public List<JsonDirectory> JsonDirectories = new List<JsonDirectory>();

        [Header("Json Viewer")]
        public GameObject JsonViewerPrefab;
        private GameObject jsonViewerReference;

        //different buttons depending on json viewer state
        [Header("Back Button in Viewer and Filebrowser")]
        public GameObject ViewerAcitveBackButton;
        public GameObject ViewerInactiveBackButton;
        public GameObject FileBrowserBackground;

        //select buttons
        [Header("Select Buttons")]
        public TextMeshProUGUI noneBtnTxt;  // 2
        public TextMeshProUGUI allBtnTxt;   // 1
        public TextMeshProUGUI todayBtnTxt; // 0

        [Header("Popup")]
        public GameObject popupPrefab;
        private GameObject popupReference;

        [HideInInspector]
        public List<TextMeshProUGUI> selectBtnTextList = new List<TextMeshProUGUI>();

        [Header("Footer")]
        public GameObject SupportFooter;
        public GameObject MenuFooter;
        #endregion

        private void Start()
        {
            persistentPath = Application.persistentDataPath;

            selectBtnTextList.Add(todayBtnTxt); // 0
            selectBtnTextList.Add(allBtnTxt);   // 1
            selectBtnTextList.Add(noneBtnTxt);  // 2
        }

        #region input events

        #region delete & share
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
        #endregion

        #region select files
        public void SelectTodaysFiles()
        {
            if (JsonDirectories.Count == 0)
            {
                return;
            }

            string daytime = FileManagement.GetDateTime();
            var curTime = FileManagement.StringToInt(daytime);    //conversion to get rid of signs
            string today = FileManagement.RemoveLengthFromEnd(curTime.ToString(), 6);

            foreach (JsonDirectory data in JsonDirectories)
            {
                //get only year / month / day
                string day = FileManagement.RemoveLengthFromEnd(data.value.ToString(), 6);

                if (today == day)
                {
                    var btn = data.obj.GetComponent<JsonFileButton>();
                    btn.ChangeSelectionToggleStatus(true);
                }

                else
                {
                    var btn = data.obj.GetComponent<JsonFileButton>();
                    btn.ChangeSelectionToggleStatus(false);
                }
            }

            HighlightSelectBtnText(0);
        }

        public void DeselectAllFiles()
        {
            if (JsonDirectories.Count == 0)
            {
                return;
            }

            foreach (JsonDirectory data in JsonDirectories)
            {
                var btn = data.obj.GetComponent<JsonFileButton>();
                btn.ChangeSelectionToggleStatus(false);
            }

            HighlightSelectBtnText(2);
        }

        public void SelectAllFiles()
        {
            if (JsonDirectories.Count == 0)
            {
                return;
            }

            foreach (JsonDirectory data in JsonDirectories)
            {
                var btn = data.obj.GetComponent<JsonFileButton>();
                btn.ChangeSelectionToggleStatus(true);
            }

            HighlightSelectBtnText(1);
        }

        public void HighlightSelectBtnText(int index)
        {
            for (int i = 0; i < selectBtnTextList.Count; i++)
            {
                if (i == index)
                {
                    if (selectBtnTextList[i].fontStyle != FontStyles.Underline)
                    {
                        selectBtnTextList[i].fontStyle = FontStyles.Underline;
                    }
                }

                else
                {
                    if (selectBtnTextList[i].fontStyle != FontStyles.Normal)
                        selectBtnTextList[i].fontStyle = FontStyles.Normal;
                }
            }
        }
        #endregion
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
            JsonDirectories.Sort((JsonDirectory x, JsonDirectory y) => y.value.CompareTo(x.value));
            HighlightSelectBtnText(2); //none selected

            if (JsonDirectories.Count != 0)
            {
                //create buttons
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

            else
            {
                //new popup
                var popup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
                var m_pop = popup.GetComponent<PopUpDisplay>();

                //message
                var para = FileManagement.GetParagraph();
                m_pop.text = $"Wow, such empty!{para}Please record something to preview and share files";
                m_pop.type = PopUpDisplay.PopupType.Static;

                //transforms
                popup.transform.localScale = Vector3.one;
                m_pop.DisplayPopup(JsonFileButtonParent);

                //reference for cleanup
                popupReference = popup;
            }
        }


        #region Get Directories
        //dir
        private List<JsonDirectory> GetDirectories()
        {
            //checking for dirs
            string[] dirs = FileManagement.GetDirectories(persistentPath);
            List<JsonDirectory> jsonDirectories = new List<JsonDirectory>();

            if (dirs.Length > 0)
            {
                for (int i = 0; i < dirs.Length; i++)
                {
                    //plugin "NatSuite" generates an empty "gallery-folder" on recording
                    //adding all dirs to list for visibilty (if not contain suffix)
                    if (!FileManagement.StringEndsWith(dirs[i], "Gallery"))
                    {
                        JsonDirectory m_dir = new JsonDirectory();
                        m_dir.dirName = FileManagement.GetDirectoryName(dirs[i]);
                        m_dir.dirPath = dirs[i];

                        jsonDirectories.Add(m_dir);
                        //only create pointers to folders including following suffixes
                        string[] suffixes = { "cam", "face" };

                        foreach (string suffix in suffixes)
                        {
                            if (FileManagement.StringEndsWith(dirs[i], suffix))
                            {
                                //create new dir obj pointing to json and overwrite previous jsonDir
                                var updatedDir = SetupDirectoryPointerToJson(dirs[i], suffix, m_dir);
                                jsonDirectories[i] = updatedDir;
                            }
                        }
                    }
                }
            }

            return jsonDirectories;
        }

        //subdir
        private JsonDirectory SetupDirectoryPointerToJson(string path, string suffix, JsonDirectory m_dir)
        {
            //JsonDirectory m_dir = new JsonDirectory();

            //looping through sub dir
            if (FileManagement.ValidateDirectory(path))
            {
                //assign for empty folders
                m_dir.value = 0;

                //jsons in sub dir
                FileInfo[] jsonFiles = FileManagement.GetJsonsAtPath(path);
                foreach (FileInfo json in jsonFiles)
                {
                    string jsonFilename = json.Name;

                    //setup JsonDirectoryData
                    if (FileManagement.StringEndsWith(jsonFilename, suffix + ".json"))
                    {
                        //setting up the serializeable JsonDir obj
                        m_dir.dirName = FileManagement.RemoveSuffixFromEnd(json.Name, ".json");
                        m_dir.value = FileManagement.StringToInt(m_dir.dirName);
                        m_dir.active = false;
                        m_dir.jsonFilePath = json.FullName;
                    }
                }

                return m_dir;
            }

            else
            {
                Debug.LogWarning("Directory doesn't contain a valid json");
                return m_dir;
            }
        }
        #endregion

        #endregion

        #region get selected files
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

        #region cleanup
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

            if (popupReference != null)
            {
                Destroy(popupReference);
            }
            JsonDirectories.Clear();
        }

        public IEnumerator DeleteZip(string zipPath)
        {
            yield return new WaitForSeconds(5.0f);
            FileManagement.DeleteFile(zipPath);
        }
        #endregion
    }

    [System.Serializable]
    public class JsonDirectory
    {
        /// <summary>
        /// path to the directory
        /// </summary>
        public string dirPath;
        /// <summary>
        /// name of the directory
        /// </summary>
        public string dirName;
        /// <summary>
        /// path to the json file
        /// </summary>
        public string jsonFilePath;
        /// <summary>
        /// instantiated button object
        /// </summary>
        public GameObject obj;
        /// <summary>
        /// bool to determine if toggle has been selected
        /// </summary>
        public bool active;
        /// <summary>
        /// index for reference
        /// </summary>
        public int index;
        /// <summary>
        /// value for sorting
        /// </summary>
        public Int64 value;
    }
}