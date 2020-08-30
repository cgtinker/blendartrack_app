using System.Collections.Generic;

namespace ArRetarget
{
    [System.Serializable]
    public class CameraPoseDataList
    {
        public List<CameraPoseData> poseList;
    }

    [System.Serializable]
    public class CameraPoseData
    {
        public vector pos;
        public vector rot;
        public int frame;
    }
}