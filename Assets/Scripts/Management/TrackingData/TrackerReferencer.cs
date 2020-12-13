using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class TrackerReferencer : MonoBehaviour
    {
        [Header("Trackers enabled based on User Preferences")]
        public List<TrackerReference> Trackers = new List<TrackerReference>();
        [Header("World to screen pos requieres anchor")]
        public string motionAnchorTag;
        public float offset;
        public bool assigned = false;

        public void Init()
        {
            if (assigned == false)
            {
                StartCoroutine(SetReferences());
            }

            assigned = true;
        }

        private void Awake()
        {
            //checking in player prefs (set in user prefers)
            foreach (TrackerReference tracker in Trackers)
            {
                tracker.value = PlayerPrefs.GetInt(tracker.nameInPlayerPrefs, -1);
            }
        }

        private IEnumerator SetReferences()
        {
            Debug.Log("Started Referencing");
            yield return new WaitForEndOfFrame();
            var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();

            for (int i = 0; i < Trackers.Count; i++)
            {
                if (Trackers[i].value == 1)
                {
                    dataManager.SetRecorderReference(Trackers[i].obj);

                    var screenPosTracker = Trackers[i].obj.GetComponent<WorldToScreenPosHandler>();
                    if (screenPosTracker != null)
                    {
                        screenPosTracker.motionAnchor = GameObject.FindGameObjectWithTag(motionAnchorTag);
                        screenPosTracker.motionAnchorTag = motionAnchorTag;
                        screenPosTracker.offset = offset;
                    }
                }
            }
        }
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