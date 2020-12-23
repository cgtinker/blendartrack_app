using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

namespace ArRetarget
{

    public class PointCloudHandler : MonoBehaviour, IInit<string>, IStop, IPrefix
    {
        ARPointCloud arPointCloud;
        public List<Vector3> points;
        private string filePath;
        private bool lastFrame;

        public void Init(string path)
        {
            filePath = $"{path}_{j_Prefix()}.json";
            lastFrame = false;
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "points", lastFrame: false);
            arPointCloud = GameObject.FindGameObjectWithTag("pointCloud").GetComponent<ARPointCloud>();
            ReceivePointCloud();
        }

        public void ReceivePointCloud()
        {
            arPointCloud.updated += OnPointCloudChanged;
        }

        public void StopTracking()
        {
            lastFrame = true;
        }

        private void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
        {
            if (!lastFrame)
                GetCurrentPoints();

            else
            {
                GetCurrentPoints();
                arPointCloud.updated -= OnPointCloudChanged;
            }
        }

        bool write;
        int curTick;
        static string contents;
        public void GetCurrentPoints()
        {
            NativeSlice<Vector3>? pointCloud = arPointCloud.positions;

            foreach (Vector3 point in pointCloud)
            {
                string json = JsonUtility.ToJson(point);
                (contents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: false, curTick: curTick, reqTick: 21, contents: contents, json: json);

                if (write)
                {
                    JsonFileWriter.WriteDataToFile(path: filePath, text: contents, title: "", lastFrame: lastFrame);
                    contents = "";
                }
            }
        }

        public string j_Prefix()
        {
            return "cloud";
        }
    }
}