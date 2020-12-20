using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

namespace ArRetarget
{

    public class PointCloudHandler : MonoBehaviour, IInit, IStop, IJson, IPrefix
    {
        ARPointCloud arPointCloud;
        public List<Vector> points;

        public void Init()
        {
            points.Clear();
            arPointCloud = GameObject.FindGameObjectWithTag("pointCloud").GetComponent<ARPointCloud>();
            ReceivePointCloud();
        }

        public void ReceivePointCloud()
        {
            arPointCloud.updated += OnPointCloudChanged;
        }

        public void StopTracking()
        {
            arPointCloud.updated -= OnPointCloudChanged;
        }

        private void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
        {
            GetCurrentPoints();
        }

        public void GetCurrentPoints()
        {
            NativeSlice<Vector3>? pos = arPointCloud.positions;

            //getting point cloud data
            foreach (Vector3 vec in pos)
            {
                var point = DataHelper.GetVector(vec);
                points.Add(point);
            }
        }

        public string GetJsonPrefix()
        {
            return "cloud";
        }

        public string GetJsonString()
        {
            PointCloud container = new PointCloud();
            container.points = points;
            var json = JsonUtility.ToJson(container);

            //return json;
            return json;
        }
    }

    [System.Serializable]
    public class PointCloud
    {
        public List<Vector> points;
    }
}