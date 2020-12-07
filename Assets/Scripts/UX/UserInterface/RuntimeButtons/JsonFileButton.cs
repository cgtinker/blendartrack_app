using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArRetarget
{
    public class JsonFileButton : MonoBehaviour
    {
        //file name of the json
        public TextMeshProUGUI filenameText;

        //visual selection state
        public Toggle selectToggleBtn;

        //reference for the json viewer
        private bool viewerActive = false;
        private FileBrowserEventManager jsonFileButtonManager;

        //info about the safed json file
        //public JsonFileData m_jsonDirData;
        public JsonDirectory m_jsonDirData;
        //public GameObject dropdownViewerBtnImg;

        public void InitFileButton(JsonDirectory jsonFileData, FileBrowserEventManager fileBtnManager)
        {
            jsonFileButtonManager = fileBtnManager;
            m_jsonDirData = new JsonDirectory();
            m_jsonDirData = jsonFileData;
            //setting title
            filenameText.text = m_jsonDirData.dirName;
            //init btns are inactive
            selectToggleBtn.isOn = false;
        }
        /*
        public void InitFileButton(JsonFileData jsonFileData, FileBrowserEventManager fileBtnManager)
        {
            jsonFileButtonManager = fileBtnManager;
            m_jsonFileData = new JsonFileData();
            m_jsonFileData = jsonFileData;
            //setting title
            filenameText.text = m_jsonFileData.filename;
            //init btns are inactive
            selectToggleBtn.isOn = false;
        }
        */
        public void OnTouchViewData()
        {
            //toggling the viewer, deactivating other btns
            viewerActive = !viewerActive;
            //dropdownViewerBtnImg.SetActive(viewerActive);
            string fileContents = FileManagement.FileContents(m_jsonDirData.jsonFilePath);
            jsonFileButtonManager.OnToggleViewer(this.m_jsonDirData.index, viewerActive, fileContents);
        }

        public void ChangeSelectionToggleStatus(bool status)
        {
            if (m_jsonDirData.active == status)
            {
                return;
            }

            //changed state and update the json file data status in btn manager
            m_jsonDirData.active = status;
            jsonFileButtonManager.JsonFileDataList[m_jsonDirData.index].active = status;

            //changing btn image
            this.selectToggleBtn.isOn = status;

            Debug.Log($"activated {m_jsonDirData.dirName}: {m_jsonDirData.active}");
        }
    }
}
