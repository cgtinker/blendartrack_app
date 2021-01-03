using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    /// <summary>
    /// only show button text while in portrait mode
    /// </summary>
    public class HideInLandscapeMode : MonoBehaviour
    {
        [Header("Don't display in Landscape Mode")]
        public List<GameObject> objectsToHide = new List<GameObject>();

        private void OnEnable()
        {
            var rect = this.gameObject.GetComponent<RectTransform>();

            bool landscape = rect.sizeDelta.x > rect.sizeDelta.y;

            if (landscape)
            {
                foreach (var obj in objectsToHide)
                {
                    obj.SetActive(false);
                }
            }
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
