using UnityEngine;
using System.Collections.Generic;

public class CameraIntrinsicsContainer
{
    public List<CameraIntrinsics> cameraIntrinsics;
    public List<CameraFrameArgs> cameraFrameArgs;
    public List<int> counter;

    public CameraConfig cameraConfig;
    public Resolution resolution;
}

[System.Serializable]
public class CameraIntrinsics
{
    public float flX;
    public float flY;

    public float ppX;
    public float ppY;
}

[System.Serializable]
public class CameraFrameArgs
{
    public long timestampNs;
    public Matrix4x4 projectionMatrix;
    public Matrix4x4 displayMatrix;
}

[System.Serializable]
public class Resolution
{
    public float screenWidth;
    public float screenHeight;

    public float captureResX;
    public float captureResY;
}

[System.Serializable]
public class CameraConfig
{
    public int? fps;
    public float width;
    public float height;
    public float resX;
    public float resY;
}