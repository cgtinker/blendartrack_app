using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Collections.Generic;
using ArRetarget;

public class AnchorTracker : MonoBehaviour
{
    AnchorCreator anchorCreator;
    List<PoseData> cameraPoseList = new List<PoseData>();

    public void Init()
    {
        anchorCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<AnchorCreator>();
    }

    public string GetJsonString()
    {
        CameraPoseContainer container = new CameraPoseContainer();
        container.cameraPoseList = cameraPoseList;
        var json = JsonUtility.ToJson(container);

        return json;
    }

    public void GetFrameData(int frame)
    {
        PoseData anchor = GetAnchorPos(frame);
        cameraPoseList.Add(anchor);
    }

    public PoseData GetAnchorPos(int frame)
    {
        var pose = GetPoseData(anchorCreator.AnchorObjects[0], frame);

        return pose;
    }

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