using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ArRetarget
{
    public class CameraPoseHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        private List<PoseData> cameraPoseList = new List<PoseData>();
        private GameObject mainCamera;

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
        }

        //obj to track
        public void Init()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        //get data at a specific frame
        public void GetFrameData(int frame)
        {
            //Debug.Log("Get Camera Data");
            var cameraPose = GetPoseData(mainCamera, frame);
            cameraPoseList.Add(cameraPose);
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
            return "CP";
        }

        //receiving pose data from obj at frame
        public static PoseData GetPoseData(GameObject obj, int frame)
        {
            var pos = new Vector()
            {
                x = obj.transform.position.x,
                y = obj.transform.position.y,
                z = obj.transform.position.z
            };

            var rot = new Vector()
            {
                x = obj.transform.eulerAngles.x,
                y = obj.transform.eulerAngles.y,
                z = obj.transform.eulerAngles.z,
            };

            var cameraPose = new PoseData()
            {
                pos = pos,
                rot = rot,

                frame = frame
            };

            return cameraPose;
        }
    }
}
