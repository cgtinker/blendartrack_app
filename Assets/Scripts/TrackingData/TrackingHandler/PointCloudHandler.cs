using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

namespace ArRetarget
{

    public class PointCloudHandler : MonoBehaviour, IInit<string>, IStop, IPrefix
    {
        ARPointCloud arPointCloud;
        public List<Vector> points;
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

        //TODO: implement last frame logic
        public void GetCurrentPoints()
        {
            NativeSlice<Vector3>? pos = arPointCloud.positions;

            if (!lastFrame)
            {
                //getting point cloud data
                foreach (Vector3 vec in pos)
                {
                    var point = DataHelper.GetVector(vec);

                    string json = JsonUtility.ToJson(point);
                    JsonFileWriter.WriteDataToFile(path: filePath, text: json, title: "", lastFrame: lastFrame);
                }
            }

            else
            {
                string m_j = "";

                foreach (Vector3 vec in pos)
                {
                    var point = DataHelper.GetVector(vec);
                    m_j = JsonUtility.ToJson(point);
                    break;
                }

                string par = "]}";
                string json = $"{m_j}{par}";

                JsonFileWriter.WriteDataToFile(path: filePath, text: json, title: "", lastFrame: lastFrame);
            }

        }

        public string j_Prefix()
        {
            return "cloud";
        }
    }
}