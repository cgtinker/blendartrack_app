using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArRetarget
{
    public class InputHandler : MonoBehaviour
    {
        DataHandler dataHandler;

        public string ArCore_CameraPose = "Sample Scene";
        public string ArCore_FaceMesh = "Sample Scene";
        public string ArKit_CameraPose = "Sample Scene";
        public string ArKit_ShapeKeys = "Sample Scene";

        private void Awake()
        {
            dataHandler = this.gameObject.GetComponent<DataHandler>();
        }

        public void TrackData()
        {
            dataHandler.ToggleRecording();
        }

        public void SendMail()
        {
            dataHandler.ToggleRecording();
            dataHandler.SerializeJson();
            RestartSession();
        }

        public void RestartSession()
        {
            Debug.Log("Restarting Session");
            string scene = null;
            switch (dataHandler.dataType)
            {
                case DataHandler.RecData.ArCore_CameraPose:
                    scene = ArCore_CameraPose;
                    break;
                case DataHandler.RecData.ArCore_FaceMesh:
                    scene = ArCore_FaceMesh;
                    break;
                case DataHandler.RecData.ArKit_CameraPose:
                    scene = ArKit_CameraPose;
                    break;
                case DataHandler.RecData.ArKit_ShapeKeys:
                    scene = ArKit_ShapeKeys;
                    break;
            }

            if (scene != null)
                SceneManager.LoadScene(scene);
            else
                Debug.Log("Load main menu");
        }
    }
}