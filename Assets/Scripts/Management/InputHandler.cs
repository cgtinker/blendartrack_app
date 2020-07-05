using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArRetarget
{
    public class InputHandler : MonoBehaviour
    {
        DataHandler dataHandler;
        public string sceneName = "Sample Scene";

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
            dataHandler.JsonSerialization();
            RestartSession();
        }

        public void RestartSession()
        {
            Debug.Log("Restarting Session");
            SceneManager.LoadScene(sceneName);
        }
    }
}