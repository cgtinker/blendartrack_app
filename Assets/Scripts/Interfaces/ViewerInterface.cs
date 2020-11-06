using UnityEngine;
using System.Collections;

namespace ArRetarget
{
    interface IInitViewer
    {
        IEnumerator Init();
    }

    interface IUpdate
    {
        IEnumerator UpdateData();
    }
}
