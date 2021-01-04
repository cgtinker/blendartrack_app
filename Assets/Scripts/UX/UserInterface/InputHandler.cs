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
        public GameObject OnFinishRecordingPrefab;
        public Transform PopupParent;
        public GameObject FileBrowserButton;

        private List<GameObject> popupList = new List<GameObject>();

        [Header("Scene Management")]
        public TextMeshProUGUI SceneTitle;

        TrackingDataManager dataManager;
        AdditiveSceneManager sceneManager;

        public bool recording = false;

        private void Awake()
        {
            GameObject obj = GameObject.FindGameObjectWithTag("manager");
            dataManager = obj.GetComponent<TrackingDataManager>();
            sceneManager = obj.GetComponent<AdditiveSceneManager>();

            OnFinishRecordingPrefab.SetActive(false);
        }

        #region tracking
        public void StartTracking()
        {
            recording = true;
            dataManager.ToggleRecording();
        }

        public void StopTrackingAndSerializeData()
        {
            recording = false;
            dataManager.ToggleRecording();
            string filename = dataManager.SerializeJson();
            //string message = "tracking successfull!";
            //GeneratedFilePopup(message, filename);

            OnFinishRecordingPrefab.SetActive(true);
        }
        #endregion

        #region UI Events
        public void GeneratedFilePopup(string message, string filename)
        {
            if (OnFinishRecordingPrefab.activeSelf)
                return;

            //generating popup element
            var m_popup = Instantiate(PopupPrefab) as GameObject;
            popupList.Add(m_popup);

            //script reference to set contents
            var popupDisplay = m_popup.GetComponent<PopUpDisplay>();

            if (filename.Length != 0)
            {
                popupDisplay.type = PopUpDisplay.PopupType.Notification;
                //travel timings
                popupDisplay.travelDuration = 5f;
                popupDisplay.staticDuration = 5f;

                popupDisplay.desitionation = FileBrowserButton;
                popupDisplay.text = $"{message}{filename}";
            }

            else
            {
                popupDisplay.type = PopUpDisplay.PopupType.Notification;
                popupDisplay.staticDuration = 8f;
                popupDisplay.text = message;
            }

            popupDisplay.DisplayPopup(PopupParent);
        }

        public void PurgeOrphanPopups()
        {
            OnFinishRecordingPrefab.SetActive(false);
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
