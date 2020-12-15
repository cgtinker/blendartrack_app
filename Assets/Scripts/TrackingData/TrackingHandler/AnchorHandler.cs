using UnityEngine;
using System.Collections.Generic;

namespace ArRetarget
{
    public class AnchorHandler : MonoBehaviour, IJson, IInit, IPrefix, IStop
    {
        ReferenceCreator referenceCreator;
        List<PoseData> anchorPosList = new List<PoseData>();

        public void Init()
        {
            referenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();
            anchorPosList.Clear();
        }

        public string GetJsonString()
        {
            RefereceData container = new RefereceData();
            container.poseDatas = anchorPosList;
            var json = JsonUtility.ToJson(container);

            return json;
        }

        public string GetJsonPrefix()
        {
            return "AH";
        }

        public void StopTracking()
        {
            foreach (GameObject anchor in referenceCreator.anchors)
            {
                var pose = DataHelper.GetPoseData(anchor, 0);
                anchorPosList.Add(pose);
            }

        }
    }
}
