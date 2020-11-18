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
        public GameObject SceneButtonPrefab;
        public Transform SceneButtonParent;
        public GameObject MainMenu;
        public GameObject SceneMenu;

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

        //generating buttons for scene handling
        private void Start()
        {
            GenerateSceneButtons();
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
            GeneratedFilePopup(filename);
        }
        #endregion

        #region UI Events
        private void GenerateSceneButtons()
        {
            //assigning the running scene title to the player prefs
            int sceneIndex = UserPreferences.Instance.GetIntPref("scene");
            string sceneName = sceneManager.GetScene(sceneIndex);
            SceneTitle.text = sceneName;

            //generating scene btns depending on the target device
            Dictionary<int, string> sceneButtonDict = sceneManager.GetDeviceScenes();
            foreach (KeyValuePair<int, string> entry in sceneButtonDict)
            {
                GameObject btn = Instantiate(SceneButtonPrefab);
                btn.name = entry.Value;

                SceneButton rbbtn = btn.GetComponent<SceneButton>();
                rbbtn.Init(name: entry.Value, key: entry.Key, sceneManager: sceneManager, mainMenu: MainMenu, sceneMenu: SceneMenu, mainMenuSceneTitle: SceneTitle);
                btn.transform.SetParent(SceneButtonParent);

                //as the canvas depends on targets device screen size and the btn is a child of the canvas
                btn.transform.localScale = Vector3.one;
            }
        }

        private void GeneratedFilePopup(string filename)
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
                popupDisplay.staticDuration = 3f;

                popupDisplay.desitionation = FileBrowserButton;
                popupDisplay.text = $"tracking successfull {filename}";
            }

            else
            {
                popupDisplay.type = PopUpDisplay.PopupType.Notification;
                popupDisplay.staticDuration = 3f;
                popupDisplay.text = "Something went wrong...";
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

        #region scene management
        //resetting the ar session, reloading can lead to bugs
        public void ReloadScene()
        {
            Debug.Log("attempt to reload the scene");
            //sceneManager.ResetArScene();
            StartCoroutine(ArFunctionalityHelper.SetAR(enabled: true));
            sceneManager.ReloadScene();
        }

        //disabling the ar session during scene changes / settings
        public void DisableArSession()
        {
            //StartCoroutine("DisableRoutine");
            StartCoroutine(ArFunctionalityHelper.SetAR(enabled: false));
        }
        /*

        public IEnumerator DisableRoutine()
        {
            var m_arSession = GameObject.FindGameObjectWithTag("arSession");
            var arSessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");

            if (m_arSession != null)
            {
                var arSession = m_arSession.GetComponent<ARSession>();
                var inputManager = m_arSession.GetComponent<ARInputManager>();
                inputManager.enabled = false;
                yield return new WaitForEndOfFrame();
                arSession.enabled = false;
            }

            if (arSessionOrigin != null)
            {
                arSessionOrigin.SetActive(false);
            }
        }
        */
        #endregion
    }
}
