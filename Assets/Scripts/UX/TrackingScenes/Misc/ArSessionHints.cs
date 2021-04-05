using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

namespace ArRetarget
{
	public class ArSessionHints : MonoBehaviour
	{
		InputHandler inputHandler;

		#region enums with callbacks
		public enum TrackingType
		{
			PlaneTracking,
			FaceTracking,
			none
		}

		public TrackingType type;

		private enum TimeState
		{
			Started,
			Detecting,
			Deteceted,
			None,
			infinite
		}

		private TimeState timeState;
		private TimeState TimeStateCallback
		{
			get { return timeState; }
			set
			{
				timeState = value;
				SessionHintMessages();
			}
		}
		#endregion

		public event Action<string> TrackingStateChanged;

		public void OnTrackingStateChanged(string message)
		{
			inputHandler.GeneratedFilePopup(message, "");
		}

		#region references
		ARFaceManager m_FaceManager;
		ARCameraManager m_CameraManager;
		ARPlaneManager m_PlaneManager;
		ReferenceCreator m_ReferenceCreator;

		float minimumBrightness = 0.38f;
		#endregion

		bool stateResetted;
		private string m_msg;
		public void SessionHintMessages()
		{
			if (InputHandler.recording)
				return;

			switch (timeState)
			{
				case TimeState.Started:
				switch (type)
				{
					case TrackingType.PlaneTracking:
					m_msg = "Move your device around slowly.";
					break;

					case TrackingType.FaceTracking:
					m_msg = $"Try to fit your face in {br}the camera frame";
					break;
				}
				break;

				// checking lightning conditions and if sufficient features have been detected
				case TimeState.Detecting:
				switch (type)
				{
					case TrackingType.PlaneTracking:
					//planes found a sufficient lightning condifitions
					if (PlanesFound() && !insufficientLightning)
					{
						m_msg = $"Double Tap a location to{br} place a Reference Object. {br}{br}Tap long to remove it.";
						//m_msg = $"Double Tap a location {br}to place a Reference Object.";
					}

					//planes found with insufficient lightning conditions
					else if (PlanesFound() && insufficientLightning)
					{
						m_msg = $"Try turning on more lights{br} and moving around.";
						ResetTimerState();
					}

					//no planes found with sufficient lightning conditions
					else if (!PlanesFound() && !insufficientLightning && !stateResetted)
					{
						m_msg = $"Unable to find a surface. {br}{br}Try moving to the side or {br}repositioning your phone.";
						ResetTimerState();
					}

					//no planes found and insufficient lightning conditions
					else if (!PlanesFound() && !insufficientLightning && stateResetted)
					{
						m_msg = $"Try moving around, turning on{br} more lights, and making sure{br} your phone is pointed at a {br}sufficiently textured surface.";
						ResetTimerState();
					}

					else if (!PlanesFound() && !insufficientLightning && !stateResetted)
					{
						m_msg = $"Try turning on more lights{br} and moving around.";
						ResetTimerState();
					}

					break;

					case TrackingType.FaceTracking:
					//face added and sufficient lightning conditions
					if (faceAdded && !insufficientLightning)
					{
						m_msg = $"Ready for capturing{br} facial expressions";
						timeState = TimeState.infinite;
					}

					//face added and insufficient lightning conditions
					else if (faceAdded && insufficientLightning)
					{
						m_msg = $"Try turning on more lights and {br}capturing facial expressions";
						ResetTimerState();
					}

					//no face found and sufficient lightning conditions
					else if (!faceAdded && !insufficientLightning)
					{
						m_msg = $"Try to position your face {br}in the screen center.";
						ResetTimerState();
					}

					//no face found and insufficient lightning conditions
					else
					{
						m_msg = $"Try turning on more lights and {br}positing your face in the screen center";
						ResetTimerState();
					}
					break;
				}
				break;
				//sufficient lightning conditions and features detected
				case TimeState.Deteceted:
				switch (type)
				{
					case TrackingType.PlaneTracking:

						{
							m_msg = $"Double Tap a location to{br} place a Reference Object. {br}{br}Tap and hold to remove it.";
						}

						break;

					case TrackingType.FaceTracking:
					if (faceAdded && faceRemoved)
					{
						m_msg = "Try to avoid rapid movements.";
						ResetTimerState();
					}

					else if (!faceAdded && faceRemoved)
					{
						m_msg = $"Try keeping your face withing {br}the camera screen and {br}avoiding rapid movements.";
						ResetTimerState();
					}
					break;
				}
				break;
				case TimeState.infinite:
				switch (type)
				{
					case TrackingType.PlaneTracking:
					m_msg = "Ready for tracking device position.";
					break;
					case TrackingType.FaceTracking:
					m_msg = "Ready for capturing facial expressions";
					break;
				}

				break;
			}
			if (!string.IsNullOrEmpty(m_msg))
				TrackingStateChanged(m_msg);
			else
				TrackingStateChanged("empty");
		}


		#region initialize and subscribe to events
		static string br;
		void Start()
		{
			br = FileManagement.GetParagraph();

			if (inputHandler == null)
				inputHandler = GameObject.FindGameObjectWithTag("interfaceManager").GetComponent<InputHandler>();

			TrackingStateChanged += OnTrackingStateChanged;
			ClearCached();

			if (PlayerPrefsHandler.Instance.GetInt("hints", -1) == -1)
			{
				type = TrackingType.none;
			}

			//init the tracking type
			switch (type)
			{
				case TrackingType.PlaneTracking:
				InitPlaneTrackingReferences();
				//start to use the time enum
				m_timeStamp = Time.time;
				trackTime = true;
				break;
				case TrackingType.FaceTracking:
				InitFaceTrackingReferences();
				m_timeStamp = Time.time;
				trackTime = true;
				break;
				case TrackingType.none:
				DisableSessionHints();
				trackTime = false;
				break;
			}

			timeState = TimeState.None;
		}

		private void InitPlaneTrackingReferences()
		{
			//planeState = PlaneTrackingState.NewSession;
			m_CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
			m_PlaneManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARPlaneManager>();
			m_ReferenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();

			if (m_CameraManager != null & enabled)
			{
				m_CameraManager.frameReceived += FrameChanged;
				m_CameraManager.lightEstimationMode = UnityEngine.XR.ARSubsystems.LightEstimationMode.AmbientIntensity;
			}
			if (m_ReferenceCreator != null & enabled)
				m_ReferenceCreator.CreatedMarker += OnPlacedObject;
		}

		private void InitFaceTrackingReferences()
		{
			//faceState = FaceTrackingState.NewSession;
			m_CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
			m_FaceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();

			if (m_CameraManager != null & enabled)
			{
				m_CameraManager.frameReceived += FrameChanged;
				m_CameraManager.lightEstimationMode = UnityEngine.XR.ARSubsystems.LightEstimationMode.AmbientIntensity;
			}
			if (m_FaceManager != null & enabled)
				m_FaceManager.facesChanged += OnFaceUpdate;
		}

		private void DisableSessionHints()
		{
			m_CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
			if (m_CameraManager != null & enabled)
			{
				m_CameraManager.lightEstimationMode = UnityEngine.XR.ARSubsystems.LightEstimationMode.Disabled;
			}
		}

		private void ClearCached()
		{
			m_CameraManager = null;
			m_FaceManager = null;
			m_PlaneManager = null;
			m_ReferenceCreator = null;
			m_timeStamp = 0.0f;
			trackTime = false;
			faceAdded = false;
			faceRemoved = false;
			placedObject = false;
			stateResetted = false;
		}

		//unsubscribe from events
		void OnDisable()
		{
			ClearCached();

			if (m_CameraManager != null)
				m_CameraManager.frameReceived -= FrameChanged;
			if (m_ReferenceCreator != null)
				m_ReferenceCreator.CreatedMarker -= OnPlacedObject;
			if (m_FaceManager != null)
				m_FaceManager.facesChanged -= OnFaceUpdate;
		}
		#endregion

		#region enum setters
		[Header("Session Hint update frequency")]
		//helper
		private int timer_ticker;
		private bool trackTime = false;
		/// <summary>
		/// interval to show a session hint in seconds
		/// </summary>
		private float freq = 8f;
		/// <summary>
		/// offset between the interval
		/// </summary>
		private float m_offset = 3f;
		/// <summary>
		/// timeStamp = time.Time at hint start
		/// gets reseted if the conditions of a time state arent reached
		/// </summary>
		private float m_timeStamp;

		private void Update()
		{
			if (InputHandler.recording)
				return;

			timer_ticker++;
			if (timer_ticker == 10)
			{
				if (trackTime)
				{
					SetTimeState();
				}

				timer_ticker = 0;
			}
		}

		//gets called about 3-6 times per second depending on fps
		public void SetTimeState()
		{

			switch (timeState)
			{
				case TimeState.None:
				if (m_timeStamp + m_offset <= Time.time &&
					m_timeStamp + m_offset + 1 >= Time.time)
				{
					if (timeState != TimeState.Started)
						TimeStateCallback = TimeState.Started;
				}
				break;
				case TimeState.Started:
				if (m_timeStamp + m_offset * 2 + freq <= Time.time &&
					m_timeStamp + m_offset * 2 + freq + 1 >= Time.time)
				{
					if (timeState != TimeState.Detecting)
						TimeStateCallback = TimeState.Detecting;
				}
				break;
				case TimeState.Detecting:
				if (m_timeStamp + m_offset * 3 + freq * 2 <= Time.time &&
					m_timeStamp + m_offset * 3 + freq * 2 + 1 >= Time.time)
				{
					if (timeState != TimeState.Deteceted)
						TimeStateCallback = TimeState.Deteceted;
				}
				break;
				case TimeState.Deteceted:
				if (m_timeStamp + m_offset * 4 + freq * 3 <= Time.time &&
					m_timeStamp + m_offset * 4 + freq * 3 + 1 >= Time.time)
				{
					if (timeState != TimeState.infinite)
						TimeStateCallback = TimeState.infinite;
				}
				break;
				case TimeState.infinite:
				break;
			}
		}

		private void ResetTimerState()
		{
			switch (timeState)
			{
				case TimeState.Detecting:
				m_timeStamp = Time.time + freq;
				timeState = TimeState.None;
				break;
				case TimeState.Deteceted:
				m_timeStamp = Time.time + m_offset + freq * 2;
				timeState = TimeState.Started;
				break;
			}

			insufficientLightning = false;
			faceRemoved = false;

			if (!stateResetted)
				stateResetted = true;
		}

		#region tracked data
		//on received frame from ar camera
		int cam_ticker = 0;
		bool insufficientLightning = false;
		List<float> lightningData = new List<float>();
		float currentLightning = 0;

		//check lightning conditions
		private void FrameChanged(ARCameraFrameEventArgs args)
		{
			if (InputHandler.recording)
				return;

			cam_ticker++;
			{
				if (cam_ticker == 5)
				{
					if (args.lightEstimation.averageBrightness.HasValue)
					{
						if (lightningData.Count < 10)
						{
							lightningData.Add(args.lightEstimation.averageBrightness.Value);
						}

						else
						{
							foreach (float data in lightningData)
								currentLightning += data;

							if (currentLightning / lightningData.Count < minimumBrightness)
								insufficientLightning = true;

							else
								insufficientLightning = false;

							currentLightning = 0;
							lightningData.Clear();
						}

					}
					cam_ticker = 0;
				}
			}
		}

		bool faceRemoved = false;
		bool faceAdded = false;
		//on ar face update received
		private void OnFaceUpdate(ARFacesChangedEventArgs args)
		{
			if (args.added.Count > 0)
			{
				faceAdded = true;
			}

			if (args.removed.Count > 0)
			{
				faceRemoved = true;
			}
		}

		//checks ar plane manger plane count
		bool PlanesFound()
		{
			if (m_PlaneManager == null)
				return false;

			return m_PlaneManager.trackables.count > 2;
		}

		bool placedObject = false;
		//if objects gets placed event from reference creator script
		void OnPlacedObject()
		{
			placedObject = true;
		}
		#endregion
		#endregion
	}
}