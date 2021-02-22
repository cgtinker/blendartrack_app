using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ArRetarget
{
    public class SettingsButtonManager : MonoBehaviour
    {
        #region refs
        public GameObject SettingsButtonPrefab;
        public GameObject SettingsTitelPrefab;
        public GameObject empty;

        private GameObject videoSettigsTitle;

        public Transform SettingsObjParent;

        public List<SettingButtonData> cameraSettings = new List<SettingButtonData>();
        public List<SettingButtonData> faceSettings = new List<SettingButtonData>();
        public List<SettingButtonData> generalSettings = new List<SettingButtonData>();
        private List<SettingButtonData> recordingSettings = new List<SettingButtonData>();
        #endregion

        private IEnumerator Start()
        {
            //timed with startup + 0.25f
            yield return new WaitForSeconds(3.5f);
            GenerateSettingsButtons();
        }

        public void GenerateSettingsButtons()
        {
            //general
            if (generalSettings.Count > 0)
            {
                GenerateSettingsTitel("General");
                GenerateButtons(generalSettings, false);
                GenerateEmptySpace();
            }

            //camera settings
            if (cameraSettings.Count > 0)
            {
                GenerateSettingsTitel("Camera Tracking");
                GenerateButtons(cameraSettings, false);
                GenerateEmptySpace();
            }

            switch (DeviceManager.Instance.device)
            {
                case DeviceManager.Device.iOS:
                    break;
                case DeviceManager.Device.iOSX:
                    //face settings
                    if (faceSettings.Count > 0)
                    {
                        GenerateSettingsTitel("Face Tracking");
                        GenerateButtons(faceSettings, false);
                        GenerateEmptySpace();
                    }
                    break;
                case DeviceManager.Device.Android:
                    //face settings
                    if (faceSettings.Count > 0)
                    {
                        GenerateSettingsTitel("Face Tracking");
                        GenerateButtons(faceSettings, false);
                        GenerateEmptySpace();
                    }
                    break;
            }

            //video settings generated at runtime (device dependant)
            if (UserPreferences.Instance.CameraConfigList.Count > 0)
            {
                videoSettigsTitle = GenerateSettingsTitel("Video Settings");
                GenerateRecordingSettingsButtons();
            }
        }


        #region ui generation
        private GameObject GenerateSettingsTitel(string displayName)
        {
            var tmp = Instantiate(SettingsTitelPrefab, Vector3.zero, Quaternion.identity);
            var script = tmp.GetComponent<UserSettingsTitel>();
            script.Init(displayName);
            tmp.transform.SetParent(SettingsObjParent);
            tmp.transform.localScale = Vector3.one;

            return tmp;
        }

        private void GenerateButtons(List<SettingButtonData> buttons, bool isToggleGroup)
        {
            foreach (SettingButtonData button in buttons)
            {
                //if button is already instantiate
                if (button.obj)
                    return;

                //generate button
                var tmp = Instantiate(SettingsButtonPrefab, Vector3.zero, Quaternion.identity);
                button.obj = tmp;
                var script = tmp.GetComponent<UserSettingsButton>();
                script.Init(button.displayName, button.userPrefsName, isToggleGroup, this.gameObject.GetComponent<SettingsButtonManager>());
                tmp.transform.SetParent(SettingsObjParent);
                tmp.transform.localScale = Vector3.one;
            }
        }

        private void GenerateEmptySpace()
        {
            var tmp = Instantiate(empty, Vector3.zero, Quaternion.identity);
            tmp.transform.SetParent(SettingsObjParent);
            tmp.transform.localScale = Vector3.one;
        }

        //can be different for face / cam (face recording currently not possible)
        public void GenerateRecordingSettingsButtons()
        {
            //same settings available
            if (UserPreferences.Instance.CameraConfigList.Count == recordingSettings.Count)
            {
                return;
            }

            else
            {
                if (videoSettigsTitle == null)
                {
                    videoSettigsTitle = GenerateSettingsTitel("Video Settings");
                }

                //removing current
                foreach (SettingButtonData data in recordingSettings)
                {
                    Destroy(data.obj);
                }
                recordingSettings.Clear();

                //referencing the available configs
                for (int i = 0; i < UserPreferences.Instance.CameraConfigList.Count; i++)
                {
                    SettingButtonData tmp = new SettingButtonData()
                    {
                        displayName = UserPreferences.Instance.CameraConfigList[i],
                        userPrefsName = UserPreferences.Instance.CameraConfigList[i]
                    };

                    recordingSettings.Add(tmp);
                }

                //generating the buttons
                GenerateButtons(recordingSettings, true);
            }
        }
        #endregion

        //custom toggle group
        public void OnToggleXRCameraSetting(string name)
        {
            foreach (SettingButtonData button in recordingSettings)
            {
                if (button.userPrefsName != name)
                {
                    var script = button.obj.GetComponent<UserSettingsButton>();
                    script.ChangeSelectionToggleStatus(false);
                    script.SetUserPreference(button.userPrefsName, false);
                    script.btnIsOn = false;
                }
            }
        }
    }

    [System.Serializable]
    public class SettingButtonData
    {
        public string displayName;
        public string userPrefsName;
        public GameObject obj;
        public bool status;
    }
}