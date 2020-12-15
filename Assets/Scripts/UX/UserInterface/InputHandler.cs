using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using TMPro;
using System.Collections.Generic;

namespace ArRetarget
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Runtime Button")]
        //public GameObject SceneButtonPrefab;
        public GameObject MainMenu;

        [Header("Pop Up Display")]
        public GameObject PopupPrefab;
        public Transform PopupParent;
        public GameObject FileBrowserButton;

        private List<GameObject> popupList = new List<GameObject>();

        [Header("Scene Management")]
        public TextMeshProUGUI SceneTitle;

        TrackingDataManager dataManager;
        AdditiveSceneManager sceneManager;

        private void Awake()
        {
            GameObject obj = GameObject.FindGameObjectWithTag("manager");
            dataManager = obj.GetComponent<TrackingDataManager>();
            sceneManager = obj.GetComponent<AdditiveSceneManager>();
        }

        #region tracking
        public void StartTracking()
        {
            dataManager.ToggleRecording();
        }

        public void StopTrackingAndSerializeData()
        {
            dataManager.ToggleRecording();
            string filename = dataManager.SerializeJson();
            string message = "tracking successfull!";
            GeneratedFilePopup(message, filename);
        }
        #endregion

        #region UI Events
        public void GeneratedFilePopup(string message, string filename)
        {
            //generating popup element
            var m_popup = Instantiate(PopupPrefab) as GameObject;
            popupList.Add(m_popup);

            //script reference to set contents
            var popupDisplay = m_popup.GetComponent<PopUpDisplay>();

            if (filename.Length != 0)
            {
                popupDisplay.type = PopUpDisplay.PopupType.Notification;
                //travel timings
                popupDisplay.travelDuration = 10f;
                popupDisplay.staticDuration = 5f;

                popupDisplay.desitionation = FileBrowserButton;
                popupDisplay.text = $"{message}{filename}";
            }

            else
            {
                popupDisplay.type = PopUpDisplay.PopupType.Notification;
                popupDisplay.staticDuration = 5f;
                popupDisplay.text = message;
            }

            popupDisplay.DisplayPopup(PopupParent);
        }

        public void PurgeOrphanPopups()
        {
            foreach (GameObject popup in popupList)
            {
                Destroy(popup);
            }

            popupList.Clear();
        }
        #endregion

        #region ar management helper
        //resetting the ar session, reloading can lead to bugs
        public void ReloadScene()
        {
            Debug.Log("attempt to reload the scene");
            //sceneManager.ResetArScene();
            StartCoroutine(ARSessionState.EnableAR(enabled: true));
            sceneManager.ReloadScene();
        }

        //disabling the ar session during scene changes / settings
        public void DisableArSession()
        {
            StartCoroutine(ARSessionState.EnableAR(enabled: false));
        }
        #endregion
    }
}
