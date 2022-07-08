using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
	public class XRCameraAutofocus : MonoBehaviour
	{

		private ARCameraManager cameraManager;

		private IEnumerator Start()
		{
			yield return new WaitForSeconds(0.2f);
			cameraManager = this.gameObject.GetComponent<ARCameraManager>();

			bool prefsAutofocus = IsAutoFocusPlayerPrefs();
			bool curAutofocus = IsCameraAutoFocusMode();

			if (prefsAutofocus != curAutofocus)
			{
				SetAutoFocus(prefsAutofocus);
			}
		}

		private void SetAutoFocus(bool auto)
		{
			cameraManager.autoFocusRequested = auto;
			// if (auto)
			// 	cameraManager.focusMode = UnityEngine.XR.ARSubsystems.CameraFocusMode.Auto;
			// else
			// 	cameraManager.focusMode = UnityEngine.XR.ARSubsystems.CameraFocusMode.Fixed;
		}

		private bool IsCameraAutoFocusMode()
		{
			bool focusmode = cameraManager.autoFocusEnabled;
			return focusmode;
			// var focusmode = cameraManager.focusMode;

			// bool autofocus;
			// switch (focusmode)
			// {
			// 	case UnityEngine.XR.ARSubsystems.CameraFocusMode.Auto:
			// 	autofocus = true;
			// 	break;

			// 	case UnityEngine.XR.ARSubsystems.CameraFocusMode.Fixed:
			// 	autofocus = false;
			// 	break;

			// 	default:
			// 	autofocus = false;
			// 	break;
			// }

			// return autofocus;
		}

		private bool IsAutoFocusPlayerPrefs()
		{
			int cur = PlayerPrefsHandler.Instance.GetInt("autofocus", -1);
			if (cur == 1)
				return true;

			else
				return false;
		}
	}
}

