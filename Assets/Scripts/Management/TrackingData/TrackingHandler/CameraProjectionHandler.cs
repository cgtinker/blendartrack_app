using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using ArRetarget;
public class CameraProjectionHandler : MonoBehaviour
{
    ARCameraManager arCameraManager;
    List<CameraProjectionMatrix> m_list = new List<CameraProjectionMatrix>();
    int frame = 0;

    public void EnableFrameUpdate()
    {
        arCameraManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<ARCameraManager>();
        arCameraManager.frameReceived += OnFrameReceived;
    }

    public void DisponseFrameUpdate()
    {
        arCameraManager.frameReceived += OnFrameReceived;
    }

    public void OnFrameReceived(ARCameraFrameEventArgs args)
    {
        frame++;
        Matrix4x4 m_matrix = (Matrix4x4)args.projectionMatrix;

        CameraProjectionMatrix tmp = new CameraProjectionMatrix();
        tmp.frame = frame;
        tmp.cameraProjectionMatrix = m_matrix;

        m_list.Add(tmp);
    }
    /*
    public string GetJsonString()
    {
        CameraProjectionContainer container = new CameraProjectionContainer();
        container.cameraProjectionList = m_list;
        var json = JsonUtility.ToJson(container);
        return json;
    }
    */
}

