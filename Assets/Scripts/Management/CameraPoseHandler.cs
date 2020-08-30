using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

public class CameraPoseHandler : MonoBehaviour
{
    [HideInInspector]
    public CameraPoseDataList cameraDataList;
    private GameObject MainCamera;

    public DataHandler dataHandler;

    private void Start()
    {
        dataHandler.SetDataType(DataHandler.RecData.ArCore_CameraPose);
    }

    public void InitCameraData()
    {
        cameraDataList.poseList.Clear();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraDataList = new CameraPoseDataList();
    }

    public void SetCameraData(int f)
    {
        CameraPoseData tmp = new CameraPoseData()
        {
            pos = new vector()
            {
                x = MainCamera.transform.position.x,
                y = MainCamera.transform.position.y,
                z = MainCamera.transform.position.z
            },

            rot = new vector()
            {
                x = MainCamera.transform.eulerAngles.x,
                y = MainCamera.transform.eulerAngles.y,
                z = MainCamera.transform.eulerAngles.y,
            },

            frame = f
        };

        cameraDataList.poseList.Add(tmp);
    }
}
