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
        public Image selectButtonImage;
        public Sprite selectActiveSprite;
        public Sprite selectInactiveSprite;

        //reference for the json viewer
        private bool viewerActive = false;
        private FileBrowserEventManager jsonFileButtonManager;

        //info about the safed json file
        public JsonFileData m_jsonFileData;


        public void InitFileButton(JsonFileData jsonFileData, FileBrowserEventManager fileBtnManager)
        {
            jsonFileButtonManager = fileBtnManager;
            m_jsonFileData = new JsonFileData();
            m_jsonFileData = jsonFileData;
            //setting title
            filenameText.text = m_jsonFileData.filename;
            //init btns are inactive
            selectButtonImage.sprite = selectInactiveSprite;
        }

        public void OnTouchViewData()
        {
            //toggling the viewer, deactivating other btns
            viewerActive = !viewerActive;
            string fileContents = FileManagementHelper.FileContents(m_jsonFileData.path);
            jsonFileButtonManager.OnToggleViewer(this.m_jsonFileData.index, viewerActive, fileContents);
        }

        public void OnTouchChangeActive()
        {
            //changed state in btn manager
            m_jsonFileData.active = !m_jsonFileData.active;
            jsonFileButtonManager.JsonFileDataList[m_jsonFileData.index].active = m_jsonFileData.active;

            //changing btn image
            if (m_jsonFileData.active)
                selectButtonImage.sprite = selectActiveSprite;

            else
                selectButtonImage.sprite = selectInactiveSprite;
        }
    }
}
