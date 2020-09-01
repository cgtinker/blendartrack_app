using UnityEngine;
using System.Collections.Generic;

namespace ArRetarget
{
    public class CameraPoseHandler : MonoBehaviour
    {
        [HideInInspector]
        public List<CameraPoseData> cameraDataList = new List<CameraPoseData>();
        private GameObject MainCamera;
        private DataManager dataManager;

        private void Start()
        {
            dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<DataManager>();
            DeviceManager.Instance.SetDataType(DeviceManager.RecData.ArCore_CameraPose);
            dataManager.AssignDataType();
        }

        public void InitCamera()
        {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        public void GetCameraPoseData(int f)
        {
            var p = new Vector()
            {
                x = MainCamera.transform.position.x,
                y = MainCamera.transform.position.y,
                z = MainCamera.transform.position.z
            };

            var r = new Vector()
            {
                x = MainCamera.transform.eulerAngles.x,
                y = MainCamera.transform.eulerAngles.y,
                z = MainCamera.transform.eulerAngles.y,
            };

            var tmp = new CameraPoseData()
            {
                pos = p,
                rot = r,

                frame = f
            };

            cameraDataList.Add(tmp);
        }
    }
}
