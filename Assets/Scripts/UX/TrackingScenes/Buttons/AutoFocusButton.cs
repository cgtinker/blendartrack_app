using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
	public class AutoFocusButton : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;
		private XRCameraAutofocus xrCameraAutofocus;

		private void Start()
		{
			var cam = GameObject.FindGameObjectWithTag(TagManager.MainCamera);
			xrCameraAutofocus = cam.GetComponent<XRCameraAutofocus>();

			bool isAuto = xrCameraAutofocus.IsCameraAutoFocusMode();
			AdjustTextDisplay(isAuto);
		}

		public void ToggleFocusMode()
		{
			bool isAuto = xrCameraAutofocus.IsCameraAutoFocusMode();
			xrCameraAutofocus.SetAutoFocus(!isAuto);
			AdjustTextDisplay(isAuto);
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
