using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using TMPro;
using System.Collections;

namespace ArRetarget
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Runtime Button")]
        public GameObject SceneButtonPrefab;
        public Transform SceneButtonParent;
        public Transform Canvas;
        public GameObject MainMenu;
        public GameObject SceneMenu;

        [Header("Scene Management")]
        public TextMeshProUGUI SceneTitle;
        DataManager dataManager;
        public AdditiveSceneManager sceneManager;

        private void Awake()
        {
            dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<DataManager>();
        }

        //generating buttons for scene handling
        private void Start()
        {
            //assigning the scene title
            int sceneIndex = UserPreferences.Instance.GetIntPref("scene");
            string sceneName = sceneManager.GetScene(sceneIndex);
            SceneTitle.text = sceneName;

            //generating scene btns depending on the target device
            Dictionary<int, string> sceneButtonDict = sceneManager.GetDeviceScenes();
            foreach (KeyValuePair<int, string> entry in sceneButtonDict)
            {
                GameObject btn = Instantiate(SceneButtonPrefab);
                btn.name = entry.Value;

                //as the canvas depends on targets device screen size and the btn is a child of the canvas
                Vector3 tmpCanvasScale = Canvas.transform.localScale;
                btn.transform.localScale = tmpCanvasScale;

                RuntimeButton rbbtn = btn.GetComponent<RuntimeButton>();
                rbbtn.Init(name: entry.Value, key: entry.Key, sceneManager: sceneManager, mainMenu: MainMenu, sceneMenu: SceneMenu, mainMenuSceneTitle: SceneTitle);
                btn.transform.SetParent(SceneButtonParent);
            }
        }
        #region tracking
        public void TrackData()
        {
            dataManager.ToggleRecording();
        }

        public void SerializeAndShare()
        {
            dataManager.ToggleRecording();
            dataManager.SerializeJson();
        }
        #endregion

        public void ReloadScene()
        {
            Debug.Log("attempt to reload the scene");

            //AdditiveSceneManager.Instance.ReloadScene();
            sceneManager.ResetScene();
            /*
            var obj = GameObject.FindGameObjectWithTag("arSession");

            if (obj != null)
            {
                var arSession = obj.GetComponent<ARSession>();
                var inputManager = obj.GetComponent<ARInputManager>();

                arSession.Reset();
                arSession.enabled = true;
                inputManager.enabled = true;
            }
            */
        }

        public void DisableArSession()
        {
            StartCoroutine("DisableRoutine");
        }

        public IEnumerator DisableRoutine()
        {
            var obj = GameObject.FindGameObjectWithTag("arSession");

            if (obj != null)
            {
                var arSession = obj.GetComponent<ARSession>();
                var inputManager = obj.GetComponent<ARInputManager>();
                inputManager.enabled = false;
                yield return new WaitForEndOfFrame();
                arSession.enabled = false;
            }
        }
    }
}
