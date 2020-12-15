using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    /// <summary>
    /// container for json serialization containing camera intrinsics
    /// </summary>
    public class CameraIntrinsicsContainer
    {
        //public List<CameraIntrinsics> cameraIntrinsics;
        public List<CameraProjectionMatrix> cameraProjection;
        public CameraConfig cameraConfig;
        public ScreenResolution resolution;
    }

    [System.Serializable]
    public class CameraProjectionMatrix
    {
        public Matrix4x4 cameraProjectionMatrix;
        public int frame;
    }

    [System.Serializable]
    public class ScreenResolution
    {
        public float screenWidth;
        public float screenHeight;
    }

    [System.Serializable]
    public class CameraConfig
    {
        public int fps;
        public float width;
        public float height;
    }
}