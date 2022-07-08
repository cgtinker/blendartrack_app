using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
	public class TrackingDataManager : MonoBehaviour
	{
		#region refs
		private string persistentPath;
		public bool _recording = false;
		public bool captureIntrinsics = true;
		private int frame = 0;
		private ARSession arSession;

		private List<IGet<int, bool>> getters = new List<IGet<int, bool>>();
		private List<IJson> jsons = new List<IJson>();
		private List<IInit<string, string>> inits = new List<IInit<string, string>>();
		private List<IStop> stops = new List<IStop>();
		private List<IPrefix> prefixs = new List<IPrefix>();
		#endregion

		#region initialize tracking session
		void Start()
		{
			//set persistent path
			persistentPath = Application.persistentDataPath;
			_recording = false;

			//match the frame rate of ar and unity updates
			if (arSession == null)
			{
				var ob = GameObject.FindGameObjectWithTag("arSession");

				if (ob)
					arSession = ob.GetComponent<ARSession>();
				else
					Debug.Log("No ARSession obj available");
			}

			if (arSession && !arSession.matchFrameRateRequested)
				arSession.matchFrameRateRequested = true;

			Debug.Log("Session started");
		}

		//resetting as not all tracking models include all tracker interfaces
		public void ResetTrackerInterfaces()
		{
			var referencer = GameObject.FindGameObjectWithTag("referencer");
			if (referencer != null)
			{
				var script = referencer.GetComponent<TrackerReferencer>();
				script.assigned = false;
			}

			getters.Clear();
			jsons.Clear();
			inits.Clear();
			stops.Clear();
			prefixs.Clear();
		}

		//the tracking references always contains some of the following interfaces
		public void SetRecorderReference(GameObject obj)
		{
			if (obj.GetComponent<IInit<string, string>>() != null)
			{
				inits.Add(obj.GetComponent<IInit<string, string>>());
			}

			if (obj.GetComponent<IPrefix>() != null)
			{
				prefixs.Add(obj.GetComponent<IPrefix>());
			}

			if (obj.GetComponent<IJson>() != null)
			{

				jsons.Add(obj.GetComponent<IJson>());
			}

			if (obj.GetComponent<IGet<int, bool>>() != null)
			{
				getters.Add(obj.GetComponent<IGet<int, bool>>());
			}

			if (obj.GetComponent<IStop>() != null)
			{
				stops.Add(obj.GetComponent<IStop>());
			}
		}
		#endregion

		#region capturing
		//called by the recording button in the capture interface
		public void ToggleRecording()
		{
			if (!_recording)
			{
				lastFrame = false;
				StartCoroutine(OnInitRetargeting());
			}

			else
			{
				OnStopRetargeting();
			}

			_recording = !_recording;

			Debug.Log($"Recording: {_recording}");
		}

		WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();
		string folderPath = "";
		public IEnumerator OnInitRetargeting()
		{
			//folder to store files
			string curTime = FileManagement.GetDateTime();

			folderPath = $"{persistentPath}/{curTime}_{prefixs[0].j_Prefix()}";
			FileManagement.CreateDirectory(folderPath);

			//time to create dir
			yield return waitForFrame;
			//initialize trackers
			InitTrackers(folderPath, "/" + curTime);

			//time to init - starting at frame 1 because of the delay
			yield return waitForFrame;
			frame = 1;
			OnEnableTracking();
		}

		//each tracker creates a file to write json data while tracking
		private void InitTrackers(string path, string title)
		{
			Debug.Log("Attempt to init");
			foreach (var init in inits)
			{
				init.Init(path, title);
			}
		}

		//called by toggle button event
		private void OnStopRetargeting()
		{
			if (stops.Count > 0)
			{
				Debug.Log("stops: " + stops.Count);
				foreach (var stop in stops)
				{
					stop.StopTracking();
				}
			}
		}

		protected virtual void OnEnableTracking()
		{
			Debug.Log("Enabled Tracking");
			Application.onBeforeRender += OnBeforeRenderPreformUpdate;
		}

		protected virtual void OnDisableTracking()
		{
			Debug.Log("Disabled Tracking");
			Application.onBeforeRender -= OnBeforeRenderPreformUpdate;
		}

		private bool lastFrame;
		//update tracking data before the render event
		protected virtual void OnBeforeRenderPreformUpdate()
		{
			if (_recording && getters.Count > 0 && !lastFrame)
			{
				GetFrameData();
			}

			else if (!_recording && getters.Count > 0 && !lastFrame)
			{
				lastFrame = true;
				GetFrameData();
				OnDisableTracking();
			}
		}

		private void GetFrameData()
		{
			frame++;
			foreach (var getter in getters)
			{
				getter.GetFrameData(frame, lastFrame);
			}
		}
		#endregion
	}
}