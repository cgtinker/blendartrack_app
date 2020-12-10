using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class TrackerReferencer : MonoBehaviour
    {
        public List<TrackerReference> Trackers = new List<TrackerReference>();
        /*
        private void Awake()
        {
            //checking in player prefs (set in user prefers)
            foreach (TrackerReference tracker in Trackers)
            {
                tracker.value = UserPreferences.Instance.GetIntPref(tracker.nameInPlayerPrefs);
            }
        }

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();

            foreach (TrackerReference tracker in Trackers)
            {
                //-1 if tracker isn't used || 1 if it's used
                if (tracker.value == 1)
                {
                    dataManager.SetRecorderReference(tracker.obj);
                }
            }

            dataManager.SetRecorderReference(this.gameObject);
        }
        */
    }

    [System.Serializable]
    public class TrackerReference
    {
        public GameObject obj;
        public string nameInPlayerPrefs;
        /// <summary>
        /// int used as bool -> -1 = false, +1 = true
        /// </summary>
        public int value;
    }
}