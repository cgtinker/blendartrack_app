﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
	public class TrackingButtonEventListner : MonoBehaviour
	{
		#region Start
		[SerializeField]
		private InputHandler inputHandler;
		[SerializeField]
		private GameObject FlexRecorderEvent;

		private void Start()
		{
			trackingState = TrackingState.Pendling;
			inputHandler = this.gameObject.GetComponent<InputHandler>();
			paragraph = FileManagement.GetParagraph();
		}
		#endregion

		#region Input Event

		#region Structure
		[System.Serializable]
		public enum ButtonInput
		{
			Record,
			Switch,
			Filebrowser,
			Settings
		}

		private ButtonInput buttonInput;

		public void TrackingSceneButtonInput(TrackingButtonComponent input)
		{
			Debug.Log(input.type);
			ButtonInputEvent = input.type;
		}

		private ButtonInput ButtonInputEvent
		{
			get
			{
				return buttonInput;
			}
			set
			{
				buttonInput = value;
				OnInputEvent();
			}
		}

		#region Input Event based on Tracking State
		private enum TrackingState
		{
			Pendling,
			Tracking
		}

		private TrackingState trackingState;

		private void OnInputEvent()
		{
			switch (trackingState)
			{
				case TrackingState.Pendling:
				WhilePendlingInputEvent();
				break;
				case TrackingState.Tracking:
				WhileTrackingInputEvent();
				break;
			}
		}
		#endregion
		#endregion

		#region Methods
		private void WhilePendlingInputEvent()
		{
			inputHandler.PurgeOrphanPopups();

			switch (buttonInput)
			{
				case ButtonInput.Record:
				inputHandler.StartTracking();
				trackingState = TrackingState.Tracking;
				break;

				case ButtonInput.Switch:
				// todo: may adjust str comp
				if (AsyncSceneManager.loadedScene == "Camera Tracker")
					switch (DeviceManager.Instance.device)
					{
						//todo: case for unsupported android
						case DeviceManager.Device.iOS:
						inputHandler.GeneratedFilePopup($"your device doesn't feature face tracking", DeviceManager.Instance.device.ToString());
						break;

						default:
						FlexRecorderEvent.SetActive(false);
						StateMachine.Instance.SetState(StateMachine.State.SwitchTrackingType);
						break;
					}

				else
				{
					FlexRecorderEvent.SetActive(false);
					StateMachine.Instance.SetState(StateMachine.State.SwitchTrackingType);
				}

				break;

				case ButtonInput.Filebrowser:
				FlexRecorderEvent.SetActive(false);
				StateMachine.Instance.SetState(StateMachine.State.Filebrowser);
				break;

				case ButtonInput.Settings:
				FlexRecorderEvent.SetActive(false);
				StateMachine.Instance.SetState(StateMachine.State.Settings);
				break;
			}
		}

		private string paragraph;
		private void WhileTrackingInputEvent()
		{
			switch (buttonInput)
			{
				case ButtonInput.Record:
				inputHandler.StopTrackingAndSerializeData();
				trackingState = TrackingState.Pendling;
				break;

				case ButtonInput.Switch:
				inputHandler.GeneratedFilePopup($"failed to switch tracking type{paragraph}",
					"please finish recording");
				break;

				case ButtonInput.Filebrowser:
				inputHandler.GeneratedFilePopup($"failed to open file browser{paragraph}",
					"please finish recording");
				break;

				case ButtonInput.Settings:
				inputHandler.GeneratedFilePopup($"failed to open settings{paragraph}",
					"please finish recording");
				break;
			}
		}
		#endregion
		#endregion
	}
}
