using UnityEngine;
using System.Collections.Generic;

namespace ArRetarget
{
    public class AnchorHandler : MonoBehaviour, IJson, IInit<string>, IPrefix, IStop
    {
        ReferenceCreator referenceCreator;
        List<Vector3> anchorPosList = new List<Vector3>();

        public void Init(string path)
        {
            referenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();
            anchorPosList.Clear();
        }

        public string j_String()
        {
            RefereceData container = new RefereceData();
            container.anchorData = anchorPosList;
            var json = JsonUtility.ToJson(container);

            return json;
        }

        public string j_Prefix()
        {
            return "anchor";
        }

        public void StopTracking()
        {
            foreach (GameObject anchor in referenceCreator.anchors)
            {
                var vector = anchor.transform.position;
                anchorPosList.Add(vector);
            }

        }
    }
}
