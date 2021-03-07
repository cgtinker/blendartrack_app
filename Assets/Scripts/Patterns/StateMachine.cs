using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace ArRetarget
{
	public class StateMachine : Singleton<StateMachine>
	{
		#region App State
		public enum State
		{
			StartUp,
			PostStartUp,
			ArCoreSupport,
			Tutorial,

			RecentTracking,
			FaceTracking,
			CameraTracking,
			SwitchTrackingType,

			Filebrowser,
			Settings,
			JsonViewer
		}

		private State appState;
		private State previousState;

		public void SetState(State state)
		{
			appState = state;
			UpdateState();
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			Debug.Log("Started State Machine");

			SetState(State.StartUp);
		}

		private void UpdateState()
		{
			OnPreviousArState();

			switch (appState)
			{
				case State.StartUp:
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Portrait;
				AsyncSceneManager.LoadScene("StartUp");
				break;

				case State.PostStartUp:
				GetPostStartUpCase();
				break;

				case State.ArCoreSupport:
				AsyncSceneManager.LoadScene("ArCoreSupport");
				break;

				case State.Tutorial:
				if (PlayerPrefs.GetInt("firstTimeUser", -1) == -1)
					FirstTimeUser.SetPlayerPrefs();

				AsyncSceneManager.LoadScene("Tutorial");
				break;

				case State.RecentTracking:
				string recentTrackingScene = PlayerPrefs.GetString("scene", "Camera Tracker");

				if (recentTrackingScene == "Camera Tracker")
					SetState(State.CameraTracking);
				else
					SetState(State.FaceTracking);
				break;

				case State.SwitchTrackingType:
				switch (previousState)
				{
					case State.FaceTracking:
					SetState(State.CameraTracking);
					break;
					case State.CameraTracking:
					SetState(State.FaceTracking);
					break;
				}
				break;

				case State.FaceTracking:
				AsyncSceneManager.LoadScene("Face Mesh Tracker");
				StartCoroutine(ARSessionState.EnableAR(enabled: true));
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Auto;
				ResetTrackerInterfaces();
				break;

				case State.CameraTracking:
				AsyncSceneManager.LoadScene("Camera Tracker");
				StartCoroutine(ARSessionState.EnableAR(enabled: true));
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Auto;
				ResetTrackerInterfaces();
				break;

				case State.Filebrowser:
				AsyncSceneManager.LoadScene("Filebrowser");
				FileManager.InstantPreview = false;

				GameObject[] orphans = GameObject.FindGameObjectsWithTag("viewer");
				foreach (GameObject go in orphans)
				{
					Destroy(go);
				}
				break;

				case State.JsonViewer:
				AsyncSceneManager.LoadScene("JsonViewer");
				break;

				case State.Settings:
				AsyncSceneManager.LoadScene("Settings");
				break;

				default:
				break;
			}

			previousState = appState;
		}

		private static void GetPostStartUpCase()
		{
			if (PlayerPrefs.GetInt("firstTimeUser", -1) == -1)
			{
				if (DeviceManager.Instance.device == DeviceManager.Device.Android)
					StateMachine.Instance.SetState(StateMachine.State.ArCoreSupport);

				else
					StateMachine.Instance.SetState(StateMachine.State.Tutorial);
			}

			else if ((PlayerPrefs.GetInt("tutorial", 1) == 1))
				StateMachine.Instance.SetState(StateMachine.State.Tutorial);

			else
				StateMachine.Instance.SetState(StateMachine.State.RecentTracking);
		}
		#endregion

		#region Reset ar tracking state
		private void OnPreviousArState()
		{
			switch (previousState)
			{
				case State.FaceTracking:
				StartCoroutine(ARSessionState.EnableAR(enabled: false));
				ResetTrackerInterfaces();
				break;
				case State.CameraTracking:
				StartCoroutine(ARSessionState.EnableAR(enabled: false));
				ResetTrackerInterfaces();
				break;
				default:
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Portrait;
				break;
			}
		}

		private void ResetTrackerInterfaces()
		{
			var go = GameObject.FindGameObjectWithTag("manager");

			if (go)
			{
				Debug.Log("Resetting Tracking Data");
				var trackingDataManager = go.GetComponent<TrackingDataManager>();
				trackingDataManager.ResetTrackerInterfaces();
			}
		}
		#endregion
	}
}

