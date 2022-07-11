using UnityEngine;
using System.Collections;

namespace ArRetarget
{
	public class StateMachine : Singleton<StateMachine>
	{
		#region App States
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
		public State AppState
		{
			get { return appState; }
		}

		private State previousState;
		public State PreviousState
		{
			get { return previousState; }
		}
		#endregion


		#region State Event

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();

			SetState(State.StartUp);
		}

		public void SetState(State state)
		{
			appState = state;
			UpdateState();
		}

		private void UpdateState()
		{
			// Previous State has been an Face or Camera Tracking Session
			switch (previousState)
			{
				case State.FaceTracking:
				StartCoroutine(XRSessionComponentProvider.Instance.OnDisableFaceDetection());
				ResetTrackerInterfaces();
				break;
				case State.CameraTracking:
				StartCoroutine(XRSessionComponentProvider.Instance.OnDisablePlaneDetection());
				ResetTrackerInterfaces();
				break;
				default:
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Portrait;
				break;
			}

			// Updating the app state
			switch (appState)
			{
				case State.StartUp:
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Portrait;
				AsyncSceneManager.LoadScene(AsyncSceneManager.StartUp);
				break;

				case State.PostStartUp:
				if (PlayerPrefsHandler.Instance.IsFirstimeUser())
				{
					if (DeviceManager.Instance.device == DeviceManager.Device.Android)
						SetState(StateMachine.State.ArCoreSupport);
					else
						SetState(StateMachine.State.Tutorial);
				}

				else if ((PlayerPrefsHandler.Instance.GetInt(PlayerPrefsHandler.Tutorial, 1) == 1))
					SetState(StateMachine.State.Tutorial);

				else
					SetState(StateMachine.State.RecentTracking);

				Resources.UnloadUnusedAssets(); break;

				case State.ArCoreSupport:
				AsyncSceneManager.LoadScene(AsyncSceneManager.ArCoreSupport);
				break;

				case State.Tutorial:
				AsyncSceneManager.LoadScene(AsyncSceneManager.Tutorial);
				if (PlayerPrefsHandler.Instance.IsFirstimeUser())
					PlayerPrefsHandler.Instance.SetFirstTimeUserPrefs();
				break;

				case State.RecentTracking:
				string recentTrackingScene = PlayerPrefsHandler.Instance.GetString(
					PlayerPrefsHandler.Scene, AsyncSceneManager.CameraTracking);

				if (recentTrackingScene == AsyncSceneManager.CameraTracking)
					SetState(State.CameraTracking);
				else
					SetState(State.FaceTracking);
				break;

				case State.SwitchTrackingType:
				// switching tracking type based on previous app state
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
				AsyncSceneManager.LoadScene(AsyncSceneManager.FaceTracking);
				StartCoroutine(XRSessionComponentProvider.Instance.OnEnableFaceDetection());
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Auto;
				ResetTrackerInterfaces();
				break;

				case State.CameraTracking:
				AsyncSceneManager.LoadScene(AsyncSceneManager.CameraTracking);
				StartCoroutine(XRSessionComponentProvider.Instance.OnEnablePlaneDetection());
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Auto;
				ResetTrackerInterfaces();
				break;

				case State.Filebrowser:
				AsyncSceneManager.LoadScene(AsyncSceneManager.Filebrowser);
				FileManager.InstantPreview = false;

				GameObject[] orphans = GameObject.FindGameObjectsWithTag(TagManager.Viewer);

				foreach (GameObject go in orphans)
				{
					Destroy(go);
				}
				break;

				case State.JsonViewer:
				AsyncSceneManager.LoadScene(AsyncSceneManager.JsonViewer);
				break;

				case State.Settings:
				AsyncSceneManager.LoadScene(AsyncSceneManager.Settings);
				break;

				default:
				Debug.LogError($"Setting state failed.\nCurrent: {appState}, Previous: {previousState}");
				break;
			}

			previousState = appState;
		}
		#endregion
		#region Reset ar tracking state
		// todo: MOVE
		private void ResetTrackerInterfaces()
		{
			var go = GameObject.FindGameObjectWithTag(TagManager.TrackingDataManager);

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

