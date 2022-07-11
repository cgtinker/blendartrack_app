using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using TMPro;
using System.Collections.Generic;

namespace ArRetarget
{
	public class InputHandler : MonoBehaviour
	{
		[Header("Pop Up Display")]
		[SerializeField]
		private GameObject PopupPrefab = null;
		[SerializeField]
		private Transform PopupParent = null;

		private List<GameObject> popupList = new List<GameObject>();

		[Header("on finish rec Display")]
		[SerializeField]
		private GameObject OnFinishRecordingPrefab = null;
		private TrackingDataManager dataManager;

		[SerializeField]
		private GameObject Timer;

		public static bool recording = false;

		private void Awake()
		{
			GameObject obj = GameObject.FindGameObjectWithTag(TagManager.TrackingDataManager);
			dataManager = obj.GetComponent<TrackingDataManager>();
			OnFinishRecordingPrefab.SetActive(false);
			Timer.SetActive(false);
		}

		#region tracking
		public void StartTracking()
		{
			recording = true;
			dataManager.ToggleRecording();
			Timer.SetActive(true);
		}

		public void StopTrackingAndSerializeData()
		{
			recording = false;
			dataManager.ToggleRecording();
			Timer.SetActive(false);
			OnFinishRecordingPrefab.SetActive(true);
		}
		#endregion

		#region UI Events
		public void GeneratedFilePopup(string message, string filename)
		{
			if (OnFinishRecordingPrefab.activeSelf)
				return;

			var m_popup = Instantiate(PopupPrefab) as GameObject;
			popupList.Add(m_popup);

			var popupDisplay = m_popup.GetComponent<PopUpDisplay>();

			popupDisplay.type = PopUpDisplay.PopupType.Notification;
			popupDisplay.staticDuration = 8f;
			popupDisplay.text = message;

			popupDisplay.DisplayPopup(PopupParent);
		}

		public void PurgeOrphanPopups()
		{
			OnFinishRecordingPrefab.SetActive(false);
			foreach (GameObject popup in popupList)
			{
				Destroy(popup);
			}

			popupList.Clear();
		}
		#endregion
	}
}
