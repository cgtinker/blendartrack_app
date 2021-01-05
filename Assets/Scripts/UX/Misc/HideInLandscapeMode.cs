using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArRetarget
{
    /// <summary>
    /// only show button text while in portrait mode
    /// </summary>
    public class HideInLandscapeMode : MonoBehaviour
    {
        [Header("Don't display objs in Landscape Mode")]
        public List<GameObject> objectsToHide = new List<GameObject>();

        private void OnEnable()
        {
            var orientation = Input.deviceOrientation;
            if (orientation == DeviceOrientation.LandscapeLeft ||
                orientation == DeviceOrientation.LandscapeRight ||
                orientation == DeviceOrientation.Unknown)
            {
                foreach (var obj in objectsToHide)
                {
                    obj.SetActive(false);
                }
            }

            Debug.Log(Input.deviceOrientation);
        }

        private void OnDisable()
        {
            foreach (var obj in objectsToHide)
            {
                obj.SetActive(true);
            }
        }
    }

}
