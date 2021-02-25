using UnityEngine;
using System.Collections;

public static class ScreenOrientationManager
{
	public static void SetAutoRotation()
	{
		SetOrientation("auto");
	}

	public static void SetPortraitMode()
	{
		SetOrientation("portrait");
	}

	private static void SetOrientation(string orientation)
	{
		try
		{
			Debug.Log($"Attempt to set {orientation} screen orientation");

			if (orientation == "auto")
			{
				SetAutoRotation(true);
				Screen.orientation = ScreenOrientation.AutoRotation;
			}

			else
			{
				SetAutoRotation(false);
				Screen.orientation = ScreenOrientation.Portrait;
			}
		}

		catch
		{
			Debug.LogError($"Cannot set {orientation} screen orientation");
		}
	}

	private static void SetAutoRotation(bool cur)
	{
		Screen.autorotateToLandscapeLeft = cur;
		Screen.autorotateToLandscapeRight = cur;
	}
}
