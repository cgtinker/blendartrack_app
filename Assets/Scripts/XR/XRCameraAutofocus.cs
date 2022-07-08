using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
	[RequireComponent(typeof(ARCameraManager))]
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
		}

		private bool IsCameraAutoFocusMode()
		{
			return cameraManager.autoFocusEnabled;
		}

		private bool IsAutoFocusPlayerPrefs()
		{
			if (PlayerPrefsHandler.Instance.GetInt("autofocus", -1) == 1)
				return true;
			return false;
		}
	}
}

