using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ArRetarget
{
    public class CameraPoseHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        private List<PoseData> cameraPoseList = new List<PoseData>();
        private GameObject mainCamera;

        private void Start()
        {
            //execute onle if it's not in the tracker referencer (not necessary atm)
            if (transform.parent != null)
            {
                return;
            }

            else
            {
                var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
                dataManager.SetRecorderReference(this.gameObject);
                var referencer = GameObject.FindGameObjectWithTag("referencer").GetComponent<TrackerReferencer>();
                referencer.Init();
            }
        }

        //obj to track
        public void Init()
        {
            cameraPoseList.Clear();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        //get data at a specific frame
        public void GetFrameData(int frame)
        {
            var pose = DataHelper.GetPoseData(mainCamera, frame);
            cameraPoseList.Add(pose);
        }

        //tracked data to json
        public string GetJsonString()
        {
            CameraPoseContainer cameraPoseContainer = new CameraPoseContainer()
            {
                cameraPoseList = cameraPoseList
            };

            //create json string
            var json = JsonUtility.ToJson(cameraPoseContainer);
            return json;
        }

        //json file prefix
        public string GetJsonPrefix()
        {
            return "cam";
        }
    }
}
