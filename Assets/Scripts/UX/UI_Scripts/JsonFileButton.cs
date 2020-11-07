using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArRetarget
{
    public class JsonFileButton : MonoBehaviour
    {
        //file name of the json
        public TextMeshProUGUI filename;

        //visual selection state
        public Image buttonImage;
        public Sprite activeBtnImage;
        public Sprite inactiveBtnImage;

        //to view the data
        private JsonDataImporter jsonDataImporter;
        //to manage safe and delete options
        private JsonFileButtonManager recFileButtonManager;

        //info about the safed json file
        public JsonFileData jsonFileData
        {
            get; private set;
        }

        public void InitFileButton(JsonFileData jsonFileData)
        {
            this.jsonFileData = jsonFileData;
            //setting title
            filename.text = jsonFileData.filename;
            //init btns are inactive
            buttonImage.sprite = inactiveBtnImage;
        }

        public void OnTouchViewData()
        {
            string fileContents = FileManagementHelper.FileContents(jsonFileData.path);
            Debug.Log("Open File");
            //jsonDataImporter.OpenFile(fileContents);
        }

        public void OnTouchChangeActive()
        {
            //changed state in btn manager
            jsonFileData.active = !jsonFileData.active;
            recFileButtonManager.JsonFileDataList[jsonFileData.index].active = jsonFileData.active;

            //changing btn image
            if (jsonFileData.active)
                buttonImage.sprite = activeBtnImage;

            else
                buttonImage.sprite = inactiveBtnImage;
        }
    }
}
