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

        public Transform SettingsObjParent;

        public List<SettingButtonData> cameraSettings = new List<SettingButtonData>();
        public List<SettingButtonData> faceSettings = new List<SettingButtonData>();
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
            //camera settings
            GenerateSettingsTitel("Camera Tracking");
            GenerateButtons(cameraSettings, false);
            GenerateEmptySpace();

            //face settings
            GenerateSettingsTitel("Face Tracking");
            GenerateButtons(faceSettings, false);
            GenerateEmptySpace();

            //video
            GenerateSettingsTitel("Video Settings");

            //getting available configs from user prefs
            for (int i = 0; i < UserPreferences.Instance.CameraConfigList.Count; i++)
            {
                SettingButtonData tmp = new SettingButtonData()
                {
                    displayName = UserPreferences.Instance.CameraConfigList[i],
                    userPrefsName = UserPreferences.Instance.CameraConfigList[i]
                };

                recordingSettings.Add(tmp);
            }

            GenerateButtons(recordingSettings, true);
        }

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

        #region ui generation
        private void GenerateSettingsTitel(string displayName)
        {
            var tmp = Instantiate(SettingsTitelPrefab, Vector3.zero, Quaternion.identity);
            var script = tmp.GetComponent<UserSettingsTitel>();
            script.Init(displayName);
            tmp.transform.SetParent(SettingsObjParent);
            tmp.transform.localScale = Vector3.one;
        }

        private void GenerateButtons(List<SettingButtonData> buttons, bool isToggleGroup)
        {
            foreach (SettingButtonData button in buttons)
            {
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
        #endregion
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