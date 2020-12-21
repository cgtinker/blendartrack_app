using UnityEngine;
using System.Collections;
using System;
namespace ArRetarget
{
    public static class DataHelper
    {
        public static PoseData GetPoseData(GameObject obj, int frame)
        {
            //var pos = GetVector(obj.transform.position);
            //var rot = GetVector(obj.transform.eulerAngles);

            var m_pose = new PoseData()
            {
                pos = obj.transform.position,
                rot = obj.transform.eulerAngles,
                //pos = pos,
                //rot = rot,
                frame = frame
            };

            return m_pose;
        }

        public static Vector GetVector(Vector3 vec)
        {
            var m_vector = new Vector()
            {
                x = vec.x,
                y = vec.y,
                z = vec.z
            };

            return m_vector;
        }
    }
}

