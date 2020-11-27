using System.Collections.Generic;

namespace ArRetarget
{
    /// <summary>
    /// container for json serialization containing camera intrinsics
    /// </summary>
    public class CameraIntrinsicsContainer
    {
        public List<CameraIntrinsics> cameraIntrinsics;
        public CameraConfig cameraConfig;
        public ScreenResolution resolution;
    }

    /// <summary>
    /// camera intrinsics at a given frame
    /// </summary>
    [System.Serializable]
    public class CameraIntrinsics
    {
        public float flX;
        public float flY;

        public float ppX;
        public float ppY;

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