using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using ArRetarget;
using System.Collections;

public class PointCloudHandler : MonoBehaviour, IInit, IStop, IJson, IPrefix
{
    ARPointCloud arPointCloud;
    public List<Vector> points;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
        dataManager.SetRecorderReference(this.gameObject);
    }

    public void Init()
    {
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

    public string GetJsonPrefix()
    {
        return "PC";
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
