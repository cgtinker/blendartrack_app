using UnityEngine;
using System.Collections;

namespace ArRetarget
{
    interface IInitViewer<T>
    {
        IEnumerator InitViewer(T data);
    }

    interface IUpdate<T, C>
    {
        IEnumerator UpdateData(T obj, C data);
    }
}
