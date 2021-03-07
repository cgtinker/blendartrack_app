using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
	public class AutoFocusButton : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;
		private ARCameraManager cameraManager;

		private void Start()
		{
			var cam = GameObject.FindGameObjectWithTag("MainCamera");
			cameraManager = cam.GetComponent<ARCameraManager>();

			bool fixedFocus = IsFixedFocus();
			AdjustTextDisplay(!fixedFocus);
		}

		public void ToggleFocusMode()
		{
			bool fixedFocus = IsFixedFocus();
			SetAutoFocus(!fixedFocus);
			AdjustTextDisplay(fixedFocus);
		}

		private bool IsFixedFocus()
		{
			var focusmode = cameraManager.focusMode;

			bool fixedFocus;
			switch (focusmode)
			{
				case UnityEngine.XR.ARSubsystems.CameraFocusMode.Auto:
				fixedFocus = false;
				break;

				case UnityEngine.XR.ARSubsystems.CameraFocusMode.Fixed:
				fixedFocus = true;
				break;

				default:
				fixedFocus = true;
				break;
			}

			return fixedFocus;
		}

		private void SetAutoFocus(bool auto)
		{
			Debug.Log("Set Auto Focus: " + auto);

			if (auto)
				cameraManager.focusMode = UnityEngine.XR.ARSubsystems.CameraFocusMode.Fixed;
			else
				cameraManager.focusMode = UnityEngine.XR.ARSubsystems.CameraFocusMode.Auto;
		}

		private void AdjustTextDisplay(bool auto)
		{
			if (auto)
				text.text = "auto";

			else
				text.text = "fixed";
		}
	}
}
