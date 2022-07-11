using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
	public class TrackerReferencer : MonoBehaviour
	{
		[Header("Trackers enabled based on User Preferences")]
		public List<TrackerReference> Trackers = new List<TrackerReference>();
		public bool assigned;

		#region init
		public IEnumerator Start()
		{
			assigned = false;
			yield return new WaitForEndOfFrame();
			StartCoroutine(SetReferences());
			assigned = true;
		}

		private void Awake()
		{
			//checking in player prefs (set in user prefers)
			foreach (TrackerReference tracker in Trackers)
			{
				tracker.value = PlayerPrefsHandler.Instance.GetInt(tracker.nameInPlayerPrefs, -1);
			}
		}
		#endregion

		private IEnumerator SetReferences()
		{
			Debug.Log("Tracker References initialized");
			yield return new WaitForEndOfFrame();
			var dataManager = GameObject.FindGameObjectWithTag(TagManager.TrackingManager).GetComponent<TrackingDataManager>();

			for (int i = 0; i < Trackers.Count; i++)
			{
				if (Trackers[i].value >= 1)
				{
					dataManager.SetRecorderReference(Trackers[i].obj);
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
		[HideInInspector]
		public int value;
	}
}