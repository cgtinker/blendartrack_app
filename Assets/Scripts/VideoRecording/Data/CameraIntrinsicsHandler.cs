using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class CameraIntrinsicsHandler : MonoBehaviour
{
    private ARCameraManager arCameraManager;
    private Camera arCamera;

    private List<CameraIntrinsics> cameraIntrinsics = new List<CameraIntrinsics>();
    private List<CameraFrameArgs> cameraFrameArgs = new List<CameraFrameArgs>();

    private List<int> counter = new List<int>();
    private int frame = 0;

    public void CacheCameraRelatedSettings()
    {
        if (arCameraManager == null)
        {
            arCameraManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<ARCameraManager>();
            arCamera = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<Camera>();

            ClearCache();
            OnReceivedFrameUpdate();
            return;
        }

        frame++;

        CameraIntrinsics intrinsics = GetIntrinsics();
        //ReceiveFrameUpdate();
        counter.Add(frame);
        cameraIntrinsics.Add(intrinsics);
    }

    public void ClearCache()
    {
        frame = 0;
        cameraIntrinsics.Clear();
        cameraFrameArgs.Clear();
        counter.Clear();
    }

    public CameraIntrinsicsContainer GetCameraIntrinsicsContainer()
    {
        OnDisponseFrameUpdate();

        CameraIntrinsicsContainer container = new CameraIntrinsicsContainer();
        container.cameraIntrinsics = this.cameraIntrinsics;
        container.cameraConfig = GetCameraConfiguration();
        container.resolution = GetResolution();
        container.counter = counter;
        container.cameraFrameArgs = cameraFrameArgs;

        return container;
    }

    #region prepare subsystem data
    private CameraConfig GetCameraConfiguration()
    {
        CameraConfig config = new CameraConfig();
        NativeArray<XRCameraConfiguration> xrCameraConfig = GetXRCameraConfigurations();

        config.fps = xrCameraConfig[0].framerate;
        config.resX = xrCameraConfig[0].resolution.x;
        config.resY = xrCameraConfig[0].resolution.y;
        config.height = xrCameraConfig[0].height;
        config.width = xrCameraConfig[0].width;

        return config;
    }

    private CameraIntrinsics GetIntrinsics()
    {
        CameraIntrinsics intrinsics = new CameraIntrinsics();

        var tmp_intrinsics = GetCameraIntrinsics();
        intrinsics.flX = tmp_intrinsics.focalLength.x;
        intrinsics.flY = tmp_intrinsics.focalLength.y;
        intrinsics.ppX = tmp_intrinsics.principalPoint.x;
        intrinsics.ppY = tmp_intrinsics.principalPoint.y;

        return intrinsics;
    }

    private Resolution GetResolution()
    {
        Resolution res = new Resolution();

        XRCameraIntrinsics intrinsics = GetCameraIntrinsics();
        res.captureResX = intrinsics.resolution.x;
        res.captureResY = intrinsics.resolution.y;

        Vector2 captureRes = GetScreenResolution();
        res.screenWidth = captureRes.x;
        res.screenHeight = captureRes.y;

        return res;
    }

    private Vector2 GetScreenResolution()
    {
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        return screenResolution;
    }
    #endregion

    #region receive subsystem data
    private void OnReceivedFrameUpdate()
    {
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisponseFrameUpdate()
    {
        arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs frame)
    {
        CameraFrameArgs cameraFrameArg = new CameraFrameArgs();

        Matrix4x4 displayMatrix;
        displayMatrix = (Matrix4x4)frame.displayMatrix;
        Matrix4x4 projectionMatrix;
        projectionMatrix = (Matrix4x4)frame.projectionMatrix;
        long timestamp;
        timestamp = (long)frame.timestampNs;

        cameraFrameArg.displayMatrix = displayMatrix;
        cameraFrameArg.projectionMatrix = projectionMatrix;
        cameraFrameArg.timestampNs = timestamp;

        cameraFrameArgs.Add(cameraFrameArg);
    }

    private XRCameraIntrinsics GetCameraIntrinsics()
    {
        XRCameraIntrinsics intrinsics;
        arCameraManager.TryGetIntrinsics(out intrinsics);

        return intrinsics;
    }

    private NativeArray<XRCameraConfiguration> GetXRCameraConfigurations()
    {
        NativeArray<XRCameraConfiguration> xrCameraConfigurations;
        xrCameraConfigurations = arCameraManager.GetConfigurations(allocator: Unity.Collections.Allocator.Temp);

        return xrCameraConfigurations;
    }
    #endregion
}
