using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using ArRetarget;

public class PointCloudHandler : MonoBehaviour
{
    ARPointCloud arPointCloud;
    public List<Vector> points;


    public void Init()
    {
        arPointCloud = GameObject.FindGameObjectWithTag("pointCloud").GetComponent<ARPointCloud>();
        ReceivePointCloud();
    }

    public void ReceivePointCloud()
    {
        //arPointCloud.updated += OnPointCloudChanged;
    }

    public void Stop()
    {
        //arPointCloud.updated -= OnPointCloudChanged;
    }

    private void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
    {
        //GetCurrentPoints();
    }

    public void GetCurrentPoints()
    {
        NativeSlice<Vector3>? pos = arPointCloud.positions;

        //getting point cloud data
        foreach (Vector3 tmp in pos)
        {
            var point = new Vector()
            {
                x = tmp.x,
                y = tmp.y,
                z = tmp.z
            };

            points.Add(point);
        }
    }

    public string GetJsonString()
    {
        PointCloud container = new PointCloud();
        container.points = points;
        var json = JsonUtility.ToJson(container);

        //return json;
        return "test";
    }
}

[System.Serializable]
public class PointCloud
{
    public List<Vector> points;
}
