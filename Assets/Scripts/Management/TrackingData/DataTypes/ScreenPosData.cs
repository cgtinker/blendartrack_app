using System.Collections.Generic;

namespace ArRetarget
{
    /// <summary>
    /// contains a list of screen pos data (vector + frame)
    /// and an Vector storing the anchor pos
    /// </summary>
    [System.Serializable]
    public class ScreenPosContainer
    {
        //world to screen position data
        public List<ScreenPosData> screenPosData;
        public Vector anchor;
    }

    /// <summary>
    /// contains a vector "screen pos" and an int "frame"
    /// </summary>
    [System.Serializable]
    public class ScreenPosData
    {
        public Vector screenPos;
        public int frame;
    }
}
