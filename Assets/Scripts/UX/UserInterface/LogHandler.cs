using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace ArRetarget
{
    public class LogHandler : MonoBehaviour
    {
        public GameObject displayParent;

        [Header("Message")]
        public TextMeshProUGUI m_titel;
        public TextMeshProUGUI m_message;

        [Header("Icon")]
        public Image icon;
        public Sprite errorIcon;
        public Sprite warningIcon;

        LogManager logger;

        bool logged;

        //listen to log event
        private void Start()
        {
            logged = false;
            displayParent.SetActive(false);
            logger = LogManager.Instance;
            Debug.Log(logger);

            logger.m_Log += OnLogReceived;
        }

        private void OnDisable()
        {
            if (logged && LogManager.Instance != null)
                logger.m_Log -= OnLogReceived;
        }

        public void OnLogReceived(string msg, LogManager.Message type)
        {
            logged = true;
            HandleLog(msg);
        }

        //display log messages (warnings / errors)
        public void HandleLog(string msg)
        {
            switch (LogManager.Instance.msg)
            {
                case LogManager.Message.Notification:
                    break;

                case LogManager.Message.Warning:
                    m_titel.text = "warning";
                    m_message.text = "<br>" + msg;
                    icon.sprite = warningIcon;
                    displayParent.SetActive(true);
                    break;

                case LogManager.Message.Error:
                    m_titel.text = "operation failed";
                    m_message.text = "<br>" + msg;
                    icon.sprite = errorIcon;
                    displayParent.SetActive(true);
                    break;

                case LogManager.Message.Disable:
                    displayParent.SetActive(false);
                    break;
            }
        }
    }
}
