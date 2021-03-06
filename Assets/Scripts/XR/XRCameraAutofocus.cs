using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
	public class XRCameraAutofocus : MonoBehaviour
	{

		private ARCameraManager cameraManager;

		private void Start()
		{
			var cam = GameObject.FindGameObjectWithTag("MainCamera");
			cameraManager = cam.GetComponent<ARCameraManager>();

			bool prefsAutofocus = IsAutoFocusPlayerPrefs();
			bool curAutofocus = IsCameraAutoFocusMode();

			if (prefsAutofocus != curAutofocus)
			{
				SetAutoFocus(prefsAutofocus);
			}
		}

		private void SetAutoFocus(bool auto)
		{
			if (auto)
				cameraManager.focusMode = UnityEngine.XR.ARSubsystems.CameraFocusMode.Auto;
			else
				cameraManager.focusMode = UnityEngine.XR.ARSubsystems.CameraFocusMode.Fixed;
		}

		private bool IsCameraAutoFocusMode()
		{
			var focusmode = cameraManager.focusMode;

			bool autofocus;
			switch (focusmode)
			{
				case UnityEngine.XR.ARSubsystems.CameraFocusMode.Auto:
				autofocus = true;
				break;

				case UnityEngine.XR.ARSubsystems.CameraFocusMode.Fixed:
				autofocus = false;
				break;

				default:
				autofocus = false;
				break;
			}

			return autofocus;
		}

		private bool IsAutoFocusPlayerPrefs()
		{
			int cur = PlayerPrefs.GetInt("autofocus", -1);
			if (cur == 1)
				return true;

			else
				return false;
		}
	}
}

