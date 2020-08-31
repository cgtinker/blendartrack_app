using UnityEngine;
using System.Collections.Generic;

namespace ArRetarget
{
    public class CameraPoseHandler : MonoBehaviour
    {
        [HideInInspector]
        public List<CameraPoseData> cameraDataList = new List<CameraPoseData>();
        private GameObject MainCamera;

        public DataHandler dataHandler;

        private void Start()
        {
            dataHandler.SetDataType(DataHandler.RecData.ArCore_CameraPose);
        }

        public void InitCameraData()
        {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        public void SetCameraData(int f)
        {
            var p = new vector()
            {
                x = MainCamera.transform.position.x,
                y = MainCamera.transform.position.y,
                z = MainCamera.transform.position.z
            };

            var r = new vector()
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
