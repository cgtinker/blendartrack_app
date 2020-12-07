using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    [System.Serializable]
    public class ObjectsToTrack
    {
        public List<CameraPoseContainer> objectToTrack;
    }

    public class ObjectTrackerHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix, IStop
    {
        public List<GameObject> objToTrack = new List<GameObject>();
        private List<List<PoseData>> masterList = new List<List<PoseData>>();

        /*
        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
        }
        */

        public void StopTracking()
        {
            var obj = GameObject.FindGameObjectWithTag("arSessionOrigin");
            var m_ref = obj.GetComponent<AnchorCreator>();
            var objs = m_ref.AnchorObjects;

            foreach (var m_obj in objs)
            {
                objToTrack.Add(m_obj);
            }

            for (int i = 0; i < objToTrack.Count; i++)
            {
                List<PoseData> tmp = new List<PoseData>();
                masterList.Add(tmp);
            }
        }

        //obj to track
        public void Init()
        {
            var obj = GameObject.FindGameObjectWithTag("arSessionOrigin");
            var planes = GameObject.FindGameObjectsWithTag("retarget");
            var m_ref = obj.GetComponent<AnchorCreator>();
            var objs = m_ref.AnchorObjects;

            foreach (var m_obj in objs)
            {
                objToTrack.Add(m_obj);
            }

            foreach (var plane in planes)
            {
                objToTrack.Add(plane);

            }

            for (int i = 0; i < objToTrack.Count; i++)
            {
                List<PoseData> tmp = new List<PoseData>();
                masterList.Add(tmp);
            }
        }

        //get data at a specific frame
        public void GetFrameData(int frame)
        {
            //Debug.Log("Get Camera Data");
            for (int i = 0; i < objToTrack.Count; i++)
            {
                var cameraPose = GetPoseData(obj: objToTrack[i].transform, frame);
                masterList[i].Add(cameraPose);
            }
        }

        //tracked data to json
        public string GetJsonString()
        {
            ObjectsToTrack m_objs = new ObjectsToTrack();
            List<CameraPoseContainer> trckedObject = new List<CameraPoseContainer>();

            foreach (List<PoseData> list in masterList)
            {
                CameraPoseContainer cameraPoseContainer = new CameraPoseContainer()
                {
                    cameraPoseList = list
                };

                trckedObject.Add(cameraPoseContainer);
            }

            m_objs.objectToTrack = trckedObject;

            //create json string
            var json = JsonUtility.ToJson(m_objs);
            return json;
        }

        //json file prefix
        public string GetJsonPrefix()
        {
            return "SO";
        }

        //receiving pose data from obj at frame
        public static PoseData GetPoseData(Transform obj, int frame)
        {
            var pos = new Vector()
            {
                x = obj.position.x,
                y = obj.position.y,
                z = obj.position.z
            };

            var rot = new Vector()
            {
                x = obj.eulerAngles.x,
                y = obj.eulerAngles.y,
                z = obj.eulerAngles.z,
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
