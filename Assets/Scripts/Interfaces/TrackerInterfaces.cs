using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public interface IInit
    {
        void Init();
    }

    public interface IGet<T>
    {
        void GetFrameData(T frame);
    }

    public interface IJson
    {
        string GetJsonString();
    }

    public interface IStop
    {
        void StopTracking();
    }
}
