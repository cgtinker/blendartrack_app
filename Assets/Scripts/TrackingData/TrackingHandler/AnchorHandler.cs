using UnityEngine;
using System.Collections.Generic;

namespace ArRetarget
{
    public class AnchorHandler : MonoBehaviour, IJson, IInit, IPrefix, IStop
    {
        ReferenceCreator referenceCreator;
        List<Vector> anchorPosList = new List<Vector>();

        public void Init()
        {
            referenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();
            anchorPosList.Clear();
        }

        public string GetJsonString()
        {
            RefereceData container = new RefereceData();
            container.anchorData = anchorPosList;
            var json = JsonUtility.ToJson(container);

            return json;
        }

        public string GetJsonPrefix()
        {
            return "anchor";
        }

        public void StopTracking()
        {
            foreach (GameObject anchor in referenceCreator.anchors)
            {
                var vector = DataHelper.GetVector(anchor.transform.position);
                anchorPosList.Add(vector);
            }

        }
    }
}
