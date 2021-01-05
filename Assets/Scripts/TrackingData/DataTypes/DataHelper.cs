using UnityEngine;
using System.Collections;
using System;
namespace ArRetarget
{
    public static class DataHelper
    {
        public static PoseData GetPoseData(GameObject obj, int frame)
        {
            var m_pose = new PoseData()
            {
                pos = obj.transform.position,
                rot = obj.transform.eulerAngles,
                frame = frame
            };

            return m_pose;
        }

        /// <summary>
        /// Helps to time the async writing process of the .json
        /// </summary>
        /// <param name="lastFrame"></param> bool if it's the last frame to record
        /// <param name="curTick"></param> current tick
        /// <param name="reqTick"></param> ticks required till writing
        /// <param name="contents"></param> string with stored json contents
        /// <param name="json"></param> added json data
        /// <returns></returns>
        /// returns updated contents, current tick and a bool to determine if to write to disk
        public static (string, int, bool) JsonContentTicker(bool lastFrame, int curTick, int reqTick, string contents, string json)
        {
            if (!lastFrame)
            {
                //storing json contents before writing to disk to prevent async overflow
                curTick++;
                if (curTick > reqTick)
                {
                    contents += $"{json},";
                    return (contents, curTick, false);
                }

                //writing data to disk
                else
                {
                    contents += json;
                    return (contents, 0, true);
                }
            }

            else
            {
                //closing json file
                string par = "]}";
                json += par;
                contents += json;
                return (contents, 0, true);
            }
        }
    }
}
