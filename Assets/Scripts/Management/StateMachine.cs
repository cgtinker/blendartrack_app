using UnityEngine;

namespace ArRetarget
{
	public class StateMachine : Singleton<StateMachine>
	{
		#region App State
		public enum State
		{
			StartUp,
			Update,
			Tutorial,

			RecentTracking,
			FaceTracking,
			CameraTracking,
			SwitchTrackingType,

			Filebrowser,
			Settings
		}

		private State appState;
		private State previousState;

		public void SetState(State state)
		{
			appState = state;
			UpdateState();
		}

		private void Awake()
		{
			SetState(State.StartUp);
		}

		private void UpdateState()
		{
			OnPreviousArState();
			Debug.Log(appState);

			switch (appState)
			{
				case State.StartUp:
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Portrait;
				AsyncSceneManager.LoadScene("StartUp");
				break;

				case State.Update:
				break;

				case State.Tutorial:
				AsyncSceneManager.LoadScene("Tutorial");
				break;

				case State.RecentTracking:
				string recentTrackingScene = PlayerPrefs.GetString("scene", "Pose Data Tracker");

				if (recentTrackingScene == "Pose Data Tracker")
					SetState(State.CameraTracking);
				else
					SetState(State.FaceTracking);

				Debug.LogWarning(recentTrackingScene);
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
				AsyncSceneManager.LoadScene("Pose Data Tracker");
				StartCoroutine(ARSessionState.EnableAR(enabled: true));
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Auto;
				ResetTrackerInterfaces();
				break;

				case State.Filebrowser:
				AsyncSceneManager.LoadScene("Filebrowser");
				break;

				case State.Settings:
				AsyncSceneManager.LoadScene("Settings");
				break;

				default:
				break;
			}

			previousState = appState;
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

