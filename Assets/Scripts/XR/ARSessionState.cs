using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;

namespace ArRetarget
{
	public static class ARSessionState
	{
		public static IEnumerator EnableAR(bool enabled)
		{
			var _arSession = GameObject.FindGameObjectWithTag("arSession");
			var _arSessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");
			var _arCamera = _arSessionOrigin.transform.GetChild(0).gameObject;

			Debug.LogWarning($"Trying to change ArSession State - enabling: {enabled}");
			if (_arSession != null)
			{
				var arSession = _arSession.GetComponent<ARSession>();
				var arCoreExtension = _arSession.GetComponent<ARCoreExtensions>();
				var arSessionOrigin = _arSessionOrigin.GetComponent<ARSessionOrigin>();

				if (enabled)
				{
					arSession.Reset();
				}

				yield return new WaitForEndOfFrame();


				if (enabled == true)
				{
					_arCamera.SetActive(enabled);
					arSession.enabled = enabled;
					arSessionOrigin.enabled = enabled;
					arCoreExtension.enabled = enabled;
				}

				else
				{
					arCoreExtension.enabled = enabled;
					arSession.enabled = enabled;
					arSessionOrigin.enabled = enabled;
					_arCamera.SetActive(enabled);
				}
			}

			else
				Debug.LogError("ArSession getting called and cannot be found");
		}
	}
}
