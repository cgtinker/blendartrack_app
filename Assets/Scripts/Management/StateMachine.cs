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
			FaceTracking,
			CameraTracking,
			Filebrowser,
			Viewer,
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
			switch (appState)
			{
				case State.StartUp:
				AsyncSceneManager.LoadScene("StartUp");
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				setOrientation = Orientation.Portrait;
				break;

				case State.Update:
				break;

				case State.Tutorial:
				break;

				case State.FaceTracking:
				ResetTrackerInterfaces();
				StartCoroutine(ARSessionState.EnableAR(enabled: true));
				setOrientation = Orientation.Auto;
				break;

				case State.CameraTracking:
				ResetTrackerInterfaces();
				StartCoroutine(ARSessionState.EnableAR(enabled: true));
				setOrientation = Orientation.Auto;
				break;

				case State.Filebrowser:
				break;

				case State.Viewer:
				break;

				case State.Settings:
				break;

				default:
				break;
			}

			previousState = appState;
		}
		#endregion

		TrackingDataManager trackingDataManager;
		private void ResetTrackerInterfaces()
		{
			if (!trackingDataManager)
				trackingDataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();

			trackingDataManager.ResetTrackerInterfaces();
		}

		#region Screen Orientation
		private enum Orientation
		{
			Portrait,
			Auto
		}

		private Orientation m_orientation;

		private Orientation setOrientation
		{
			get
			{
				return m_orientation;
			}
			set
			{
				m_orientation = value;
				SetScreenOrientation();
			}
		}

		private void SetScreenOrientation()
		{
			switch (m_orientation)
			{
				case Orientation.Portrait:
				ScreenOrientationManager.SetPortraitMode();
				break;

				case Orientation.Auto:
				ScreenOrientationManager.SetAutoRotation();
				break;
			}
		}
		#endregion
	}
}

