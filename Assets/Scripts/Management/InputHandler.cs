using UnityEngine;
using System.Collections.Generic;

namespace ArRetarget
{
    public class InputHandler : MonoBehaviour
    {
        public GameObject SceneButtonPrefab;
        public Transform SceneButtonParent;
        public Transform Canvas;

        DataManager dataManager;

        private void Awake()
        {
            dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<DataManager>();
        }

        //generating buttons for scene handling
        private void Start()
        {
            //generating scene btns depending on the target device
            Dictionary<int, string> sceneButtonDict = AdditiveSceneManager.Instance.GetDeviceScenes();
            foreach (KeyValuePair<int, string> entry in sceneButtonDict)
            {
                GameObject btn = Instantiate(SceneButtonPrefab);
                btn.name = entry.Value;

                //as the canvas depends on targets device screen size and the btn is a child of the canvas
                Vector3 tmpCanvasScale = Canvas.transform.localScale;
                btn.transform.localScale = tmpCanvasScale;

                RuntimeButton rbbtn = btn.GetComponent<RuntimeButton>();
                rbbtn.Init(name: entry.Value, key: entry.Key);
                btn.transform.SetParent(SceneButtonParent);
            }
        }

        public void TrackData()
        {
            dataManager.ToggleRecording();
        }

        public void SerializeAndShare()
        {
            dataManager.ToggleRecording();
            dataManager.SerializeJson();
        }

        public void ReloadScene()
        {
            Debug.Log("attempt to reload the scene");
            AdditiveSceneManager.Instance.ReloadScene();
        }
    }
}
