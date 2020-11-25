using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class CameraContextInfo : MonoBehaviour
{
    private ARCameraManager arCameraManager;

    private List<CameraIntrinsics> cameraIntrinsics = new List<CameraIntrinsics>();
    private List<CameraConfig> cameraConfigs = new List<CameraConfig>();
    private List<CameraFrameArgs> cameraFrameArgs = new List<CameraFrameArgs>();
    private List<int> counter = new List<int>();

    private int frame = 0;

    public void CacheCameraRelatedSettings()
    {
        if (arCameraManager == null)
        {
            arCameraManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARCameraManager>();
            ReceiveCameraFrameUpdate();
            return;
        }

        frame++;

        CameraIntrinsics intrinsics = GetIntrinsics();
        CameraConfig config = GetCameraConfiguration();

        counter.Add(frame);
        cameraIntrinsics.Add(intrinsics);
        cameraConfigs.Add(config);
    }

    public CameraIntrinsicsContainer GetCameraIntrinsicsContainer()
    {
        DisponseCameraFrameUpdate();

        CameraIntrinsicsContainer container = new CameraIntrinsicsContainer();
        container.cameraIntrinsics = this.cameraIntrinsics;
        container.cameraConfigs = this.cameraConfigs;
        container.resolution = GetResolution();

        frame = 0;
        cameraIntrinsics.Clear();
        cameraConfigs.Clear();
        cameraFrameArgs.Clear();
        counter.Clear();

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
    private void ReceiveCameraFrameUpdate()
    {
        arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void DisponseCameraFrameUpdate()
    {
        arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        CameraFrameArgs cameraFrameArg = new CameraFrameArgs();

        cameraFrameArg.displayMatrix = eventArgs.displayMatrix;
        cameraFrameArg.projectionMatrix = eventArgs.projectionMatrix;
        cameraFrameArg.timestampNs = eventArgs.timestampNs;

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
