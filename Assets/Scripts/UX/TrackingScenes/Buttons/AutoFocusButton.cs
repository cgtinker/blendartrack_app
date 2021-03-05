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

			bool strike = IsFixedFocus();
			StrikeThoughText(strike);
		}

		public void ToggleFocusMode()
		{
			bool fixedFocus = IsFixedFocus();
			SetAutoFocus(!fixedFocus);
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

		private void StrikeThoughText(bool fixedFocus)
		{
			if (fixedFocus)
				text.fontStyle = FontStyles.Strikethrough;

			else
				text.fontStyle = FontStyles.Normal;
		}
	}
}
