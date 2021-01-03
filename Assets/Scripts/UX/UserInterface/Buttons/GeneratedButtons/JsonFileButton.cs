using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArRetarget
{
    public class JsonFileButton : MonoBehaviour
    {
        public GameObject ViewDataButton;
        public Image viewedDataImage;
        public Sprite viewedDataIcon;
        public Sprite unviewedDataIcon;

        //file name of the json
        public TextMeshProUGUI filenameText;

        //visual selection state
        //public Toggle selectToggleBtn;
        //public Button selectToggleBtn;
        public bool btnIsOn = false;

        public GameObject selected;
        public GameObject deselected;

        //reference for the json viewer
        private bool viewerActive = false;
        private FileBrowserEventManager jsonFileButtonManager;

        //info about the safed json file
        public JsonDirectory m_jsonDirData;

        private void OnEnable()
        {
            ViewDataButton.SetActive(true);
        }

        public void InitFileButton(JsonDirectory jsonFileData, FileBrowserEventManager fileBtnManager)
        {
            //ref to manager
            jsonFileButtonManager = fileBtnManager;
            //paste data
            m_jsonDirData = new JsonDirectory();
            m_jsonDirData = jsonFileData;
            //setting title
            filenameText.text = m_jsonDirData.dirName;
            //init btns are inactive
            btnIsOn = false;
            selected.SetActive(false);
            deselected.SetActive(true);
        }

        public void OnTouchViewData()
        {
            if (string.IsNullOrEmpty(m_jsonDirData.jsonFilePath))
            {
                return;
            }
            viewedDataImage.sprite = viewedDataIcon;
            ViewDataButton.SetActive(false);
            //toggling the viewer, deactivating other btns
            viewerActive = !viewerActive;
            //dropdownViewerBtnImg.SetActive(viewerActive);
            string fileContents = FileManagement.FileContents(m_jsonDirData.jsonFilePath);
            jsonFileButtonManager.OnToggleViewer(this.m_jsonDirData.index, viewerActive, fileContents);
        }

        public void OnToggleButton()
        {
            btnIsOn = !btnIsOn;
            ChangeSelectionToggleStatus(btnIsOn);
        }

        public void ChangeSelectionToggleStatus(bool status)
        {
            selected.SetActive(status);
            deselected.SetActive(!status);

            if (m_jsonDirData.active == status)
            {
                return;
            }

            //changed state and update the json file data status in btn manager
            m_jsonDirData.active = status;
            jsonFileButtonManager.JsonDirectories[m_jsonDirData.index].active = status;

            //remove the all / none / today highlight if there is any
            jsonFileButtonManager.HighlightSelectBtnText(10);
        }
    }
}
