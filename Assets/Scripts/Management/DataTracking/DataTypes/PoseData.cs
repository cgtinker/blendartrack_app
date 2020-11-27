using System.Collections.Generic;

namespace ArRetarget
{
    /// <summary>
    /// descripes the camera position and rotation at a given frame
    /// </summary>
    [System.Serializable]
    public class PoseData
    {
        public Vector pos;
        public Vector rot;
        public int frame;
    }

    /// <summary>
    /// pose movement container
    /// </summary>
    [System.Serializable]
    public class CameraPoseContainer
    {
        public List<PoseData> cameraPoseList;
    }
}