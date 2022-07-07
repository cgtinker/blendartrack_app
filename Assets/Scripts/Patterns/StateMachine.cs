using UnityEngine;
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
		public State AppState
		{
			get { return appState; }
		}

		private State previousState;
		public State PreviousState
		{
			get { return previousState; }
		}
		public void SetState(State state)
		{
			appState = state;

			UpdateState();
		}

		#region State Event

		#endregion

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			Debug.Log("Started State Machine");

			SetState(State.StartUp);
		}

		private void UpdateState()
		{
			OnPreviousArState();
			XRSessionComponentProvider xrSessionComponentProvider = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<XRSessionComponentProvider>();


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
				AsyncSceneManager.LoadScene("Tutorial");
				if (isFirstTimeUser())
					setUserPrefOnFinishTutorial();
				break;

				case State.RecentTracking:
				string recentTrackingScene = PlayerPrefsHandler.Instance.GetString("scene", "Camera Tracker");

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
				StartCoroutine(xrSessionComponentProvider.OnEnableFaceDetection());
				ScreenOrientationManager.setOrientation = ScreenOrientationManager.Orientation.Auto;
				ResetTrackerInterfaces();
				break;

				case State.CameraTracking:
				AsyncSceneManager.LoadScene("Camera Tracker");
				StartCoroutine(ARSessionState.EnableAR(enabled: true));
				StartCoroutine(xrSessionComponentProvider.OnEnablePlaneDetection());
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
			if (isFirstTimeUser())
			{
				if (DeviceManager.Instance.device == DeviceManager.Device.Android)
					StateMachine.Instance.SetState(StateMachine.State.ArCoreSupport);
				else
					StateMachine.Instance.SetState(StateMachine.State.Tutorial);
			}

			else if ((PlayerPrefsHandler.Instance.GetInt("tutorial", 1) == 1))
				StateMachine.Instance.SetState(StateMachine.State.Tutorial);

			else
				StateMachine.Instance.SetState(StateMachine.State.RecentTracking);

			Resources.UnloadUnusedAssets();
		}
		#endregion

		#region player pref access
		private static int firstTimeUserPref = 1;
		private static bool isFirstTimeUser()
		{
			if (PlayerPrefsHandler.Instance.GetInt("firstTimeUser", firstTimeUserPref) == firstTimeUserPref)
				return true;
			else
				return false;
		}

		private void setUserPrefOnFinishTutorial()
		{
			PlayerPrefsHandler.Instance.SetFirstTimeUserPrefs(-firstTimeUserPref);
		}
		#endregion

		#region Reset ar tracking state
		private void OnPreviousArState()
		{
			XRSessionComponentProvider xrSessionComponentProvider = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<XRSessionComponentProvider>();

			switch (previousState)
			{
				case State.FaceTracking:
				xrSessionComponentProvider.OnDisableFaceDetection();
				StartCoroutine(ARSessionState.EnableAR(enabled: false));
				ResetTrackerInterfaces();
				break;
				case State.CameraTracking:
				xrSessionComponentProvider.OnDisablePlaneDetection();
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

